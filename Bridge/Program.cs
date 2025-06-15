using AssettoCorsaSharedMemory;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using WebSocketSharp;

class Program
{
    private static WebSocket _websocket;
    private static ManualResetEvent _quitEvent = new ManualResetEvent(false);

    static void Main(string[] args)
    {
        var ac = new AssettoCorsa();

        // WebSocket setup
        _websocket = new WebSocket("ws://localhost:8765");
        _websocket.Connect();

        ac.PhysicsUpdated += Ac_PhysicsUpdated;
        ac.GraphicsUpdated += Ac_GraphicsInterval;
        ac.StaticInfoUpdated += Ac_StaticInfoUpdated;

        ac.Start();

        Console.WriteLine("Telemetry streaming started. Press Ctrl+C to exit...");
        Console.CancelKeyPress += (sender, eArgs) =>
        {
            eArgs.Cancel = true;
            _quitEvent.Set();
        };

        _quitEvent.WaitOne();

        _websocket.Close();
        ac.Stop();
    }

    private static void Ac_PhysicsUpdated(object sender, PhysicsEventArgs e)
    {
        var physics = e.Physics;
        var data = new
        {
            Type = "Physics",
            physics.PacketId,
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
            physics.TurboBoost,
            physics.AutoShifterOn,
            physics.PitLimiterOn,
            physics.IsAIControlled,
            physics.Velocity,
            physics.LocalVelocity,
            physics.AccG,
            physics.LocalAngularVelocity,
            physics.Heading,
            physics.Pitch,
            physics.Roll,
            physics.WheelSlip,
            physics.WheelPressure,
            physics.WheelAngularSpeed,
            physics.BrakeTemp,
            physics.BreakPressure,
            physics.TyreCoreTemp,
            physics.AirTemp,
            physics.RoadTemp,
            physics.WaterTemp,
            physics.FinalFF,
            physics.KerbVibration,
            physics.SlipVibrations,
            physics.GBibrations,
            physics.ABSVibrations,
            physics.IgnitionOn,
            physics.StarterEngineOn,
            physics.IsEngineRunning,
            physics.FrontBreakCompound,
            physics.RearBreakCompount,
            physics.PadLife,
            physics.DiscLife,
            physics.CarDamage,
            Timestamp = DateTime.UtcNow
        };

        SendTelemetryData(data);
    }

    private static void Ac_GraphicsInterval(object sender, GraphicsEventArgs e)
    {
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

        SendTelemetryData(data);
    }

    private static void Ac_StaticInfoUpdated(object sender, StaticInfoEventArgs e)
    {
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

        SendTelemetryData(data);
    }

    private static void SendTelemetryData(object data)
    {
        try
        {
            var json = JsonSerializer.Serialize(data);
            Console.WriteLine(json);
            _websocket.Send(json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error sending telemetry data: {ex.Message}");
        }
    }
}
