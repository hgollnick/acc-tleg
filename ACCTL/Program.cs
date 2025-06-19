using AssettoCorsaSharedMemory;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using WebSocketSharp;
using WebSocketSharp.Server;

class TelemetryWebSocket : WebSocketBehavior
{
    private static readonly Queue<string> _messageQueue = new Queue<string>();
    private static readonly object _lock = new object();
    private static Timer _timer;
    private static WebSocketSessionManager _sessions;

    public static void StartBroadcast(WebSocketSessionManager sessions)
    {
        _sessions = sessions;
        _timer = new Timer(_ => SendPendingMessages(), null, 0, 100);
    }

    public static void StopBroadcast()
    {
        _timer?.Dispose();
    }

    public static void BroadcastMessage(string message)
    {
        lock (_lock)
        {
            _messageQueue.Enqueue(message);
        }
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        Send("{\"Type\":\"Info\",\"Message\":\"Connected to ACC Telemetry WebSocket\"}");
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        // No-op: server only pushes data
    }

    private static void SendPendingMessages()
    {
        lock (_lock)
        {
            while (_messageQueue.Count > 0)
            {
                var msg = _messageQueue.Dequeue();
                _sessions?.Broadcast(msg);
            }
        }
    }
}

class Program
{
    static WebSocketServer wsServer;

    static void Main(string[] args)
    {
        AssettoCorsa ac = null;
        try
        {
            ac = new AssettoCorsa();

            ac.PhysicsUpdated += Ac_PhysicsUpdated;
            ac.GraphicsUpdated += Ac_GraphicsInterval;
            ac.StaticInfoUpdated += Ac_StaticInfoUpdated;

            ac.Start();

            wsServer = new WebSocketServer("ws://127.0.0.1:8081");
            wsServer.AddWebSocketService<TelemetryWebSocket>("/telemetry");
            wsServer.Start();
            TelemetryWebSocket.StartBroadcast(wsServer.WebSocketServices["/telemetry"].Sessions);

            Console.WriteLine("Telemetry streaming started. Press any key to exit...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Fatal error: {ex.Message}\n{ex.StackTrace}");
        }
        finally
        {
            if (ac != null)
            {
                ac.PhysicsUpdated -= Ac_PhysicsUpdated;
                ac.GraphicsUpdated -= Ac_GraphicsInterval;
                ac.StaticInfoUpdated -= Ac_StaticInfoUpdated;
                ac.Stop();
            }
            TelemetryWebSocket.StopBroadcast();
            wsServer?.Stop();
            Console.WriteLine("Telemetry streaming stopped. Press any key to close.");
            Console.ReadKey();
        }
    }

    private static void Ac_PhysicsUpdated(object sender, PhysicsEventArgs e)
    {
        try
        {
            if (e?.Physics == null) return;
            var physics = e.Physics;
            var data = new
            {
                Type = "Physics",
                physics.SpeedKmh,
                physics.Gas,
                physics.Brake,
                physics.Clutch,
                physics.Gear,
                physics.Rpms,
                physics.Fuel,
                physics.SteerAngle,
                physics.TC,
                physics.Abs,
                Timestamp = DateTime.UtcNow
            };
            var json = JsonSerializer.Serialize(data);
            Console.WriteLine(json);
            TelemetryWebSocket.BroadcastMessage(json);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"PhysicsUpdated error: {ex.Message}");
        }
    }

    private static void Ac_GraphicsInterval(object sender, GraphicsEventArgs e)
    {
        try
        {
            if (e?.Graphics == null) return;
            var graphics = e.Graphics;
            var data = new
            {
                Type = "Graphics",
                graphics.Status,
                graphics.Session,
                graphics.SessionTimeLeft,
                graphics.SessionIndex,
                graphics.CurrentTime,
                graphics.LastTime,
                graphics.BestTime,
                graphics.DeltaLapTime,
                graphics.EstimatedLapTime,
                graphics.IsValidLap,
                graphics.Position,
                graphics.CompletedLaps,
                graphics.CurrentSectorIndex,
                graphics.LastSectorTime,
                graphics.DistanceTraveled,
                graphics.IsInPit,
                graphics.IsInPitLane,
                graphics.MandatoryPitDone,
                graphics.MissingMandatoryPits,
                graphics.TrackStatus,
                graphics.SurfaceGrip,
                graphics.TrackGripStatus,
                graphics.WindSpeed,
                graphics.WindDirection,
                graphics.RainIntensity,
                graphics.RainIntensityIn10min,
                graphics.RainIntensityIn30min,
                graphics.TC,
                graphics.TCCUT,
                graphics.ABS,
                graphics.EngineMap,
                graphics.FuelXLap,
                graphics.UsedFuel,
                graphics.FuelEstimatedLaps,
                graphics.TyreCompound,
                graphics.RainTyres,
                graphics.RainLights,
                graphics.FlashingLights,
                graphics.LightsStage,
                graphics.DirectionLightsLeft,
                graphics.DirectionLightsRight,
                graphics.Flag,
                graphics.Penalty,
                graphics.PenaltyTime,
                graphics.MfdTyreSet,
                graphics.MfdFuelToAdd,
                graphics.MfdTyrePressureLF,
                graphics.MfdTyrePressureRF,
                graphics.MfdTyrePressureLR,
                graphics.MfdTyrePressureRR,
                Timestamp = DateTime.UtcNow
            };
            // var json = JsonSerializer.Serialize(data);
            // Console.WriteLine(json);
            // TelemetryWebSocket.BroadcastMessage(json);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"GraphicsUpdated error: {ex.Message}");
        }
    }

    private static void Ac_StaticInfoUpdated(object sender, StaticInfoEventArgs e)
    {
        try
        {
            if (e?.StaticInfo == null) return;
            var s = e.StaticInfo;
            var data = new
            {
                Type = "StaticInfo",
                s.SMVersion,
                s.ACVersion,
                s.NumberOfSessions,
                s.NumCars,
                s.SectorCount,
                s.IsOnline,
                s.PlayerName,
                s.PlayerSurname,
                s.PlayerNick,
                s.CarModel,
                s.Track,
                s.TrackConfiguration,
                s.MaxRpm,
                s.MaxFuel,
                s.PenaltiesEnabled,
                s.PitWindowStart,
                s.PitWindowEnd,
                s.AidFuelRate,
                s.AidTireRate,
                s.AidMechanicalDamage,
                s.AidAllowTyreBlankets,
                s.AidStability,
                s.AidAutoClutch,
                s.AidAutoBlip,
                s.DryTyresName,
                s.WetTyresName,
                Timestamp = DateTime.UtcNow
            };
            // var json = JsonSerializer.Serialize(data);
            // Console.WriteLine(json);
            // TelemetryWebSocket.BroadcastMessage(json);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"StaticInfoUpdated error: {ex.Message}");
        }
    }
}
