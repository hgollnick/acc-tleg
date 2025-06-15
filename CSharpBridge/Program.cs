using AssettoCorsaSharedMemory;
using System;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        var ac = new AssettoCorsa();
        ac.PhysicsUpdated += Ac_PhysicsUpdated;
        ac.StaticInfoUpdated += Ac_StaticInfoUpdated;
        ac.GraphicsInterval += ac_GraphicsInterval;
        ac.Start();

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();

        ac.Stop();
    }

    private static void Ac_PhysicsUpdated(object sender, PhysicsEventArgs e)
    {
        var physics = e.Physics;

        Console.WriteLine($"=== Physics Data ===");

        // Basic Vehicle Data
        Console.WriteLine($"PacketId: {physics.PacketId}");
        Console.WriteLine($"Speed: {physics.SpeedKmh} km/h");
        Console.WriteLine($"Gas/Throttle: {physics.Gas:F3}");
        Console.WriteLine($"Brake: {physics.Brake:F3}");
        Console.WriteLine($"Clutch: {physics.Clutch:F3}");
        Console.WriteLine($"Gear: {physics.Gear}");
        Console.WriteLine($"RPMs: {physics.Rpms}");
        Console.WriteLine($"Fuel: {physics.Fuel:F3}");

        // Vehicle Dynamics
        Console.WriteLine($"Steering Angle: {physics.SteerAngle:F3}");
        Console.WriteLine($"Brake Bias: {physics.BrakeBias:F3}");
        Console.WriteLine($"TC: {physics.TC:F3}");
        Console.WriteLine($"ABS: {physics.Abs:F3}");
        Console.WriteLine($"Turbo Boost: {physics.TurboBoost:F3}");

        // Vehicle Status
        Console.WriteLine($"AutoShifter: {physics.AutoShifterOn}");
        Console.WriteLine($"Pit Limiter: {physics.PitLimiterOn}");
        Console.WriteLine($"AI Controlled: {physics.IsAIControlled}");

        // Motion & Forces
        Console.WriteLine($"Velocity XYZ: {physics.Velocity[0]:F1}, {physics.Velocity[1]:F1}, {physics.Velocity[2]:F1}");
        Console.WriteLine($"Local Velocity XYZ: {physics.LocalVelocity[0]:F1}, {physics.LocalVelocity[1]:F1}, {physics.LocalVelocity[2]:F1}");
        Console.WriteLine($"G-Forces XYZ: {physics.AccG[0]:F3}, {physics.AccG[1]:F3}, {physics.AccG[2]:F3}");
        Console.WriteLine($"Local Angular Velocity: {physics.LocalAngularVelocity[0]:F3}, {physics.LocalAngularVelocity[1]:F3}, {physics.LocalAngularVelocity[2]:F3}");
        Console.WriteLine($"Heading: {physics.Heading:F3}");
        Console.WriteLine($"Pitch: {physics.Pitch:F3}");
        Console.WriteLine($"Roll: {physics.Roll:F3}");

        // Wheel & Brake Data
        Console.WriteLine("Wheel Data (FL, FR, RL, RR):");
        Console.WriteLine($"- Wheel Slip: {string.Join(", ", physics.WheelSlip.Select(x => x.ToString("F3")))}");
        Console.WriteLine($"- Wheel Pressure: {string.Join(", ", physics.WheelPressure.Select(x => x.ToString("F3")))}");
        Console.WriteLine($"- Wheel Angular Speed: {string.Join(", ", physics.WheelAngularSpeed.Select(x => x.ToString("F3")))}");
        Console.WriteLine($"- Brake Temp: {string.Join(", ", physics.BrakeTemp.Select(x => x.ToString("F1")))}");
        Console.WriteLine($"- Break Pressure: {string.Join(", ", physics.BreakPressure.Select(x => x.ToString("F1")))}");

        // Tire Data
        Console.WriteLine($"Tyre Core Temp: {string.Join(", ", physics.TyreCoreTemp.Select(x => x.ToString("F1")))}");

        // Environmental
        Console.WriteLine($"Air Temp: {physics.AirTemp:F1}°C");
        Console.WriteLine($"Road Temp: {physics.RoadTemp:F1}°C");
        Console.WriteLine($"Water Temp: {physics.WaterTemp:F1}°C");

        // Force Feedback & Vibrations
        Console.WriteLine($"Final FF: {physics.FinalFF:F3}");
        Console.WriteLine($"Kerb Vibration: {physics.KerbVibration:F3}");
        Console.WriteLine($"Slip Vibrations: {physics.SlipVibrations:F3}");
        Console.WriteLine($"G Vibrations: {physics.GBibrations:F3}");
        Console.WriteLine($"ABS Vibrations: {physics.ABSVibrations:F3}");

        // Car Status
        Console.WriteLine($"Ignition: {physics.IgnitionOn}");
        Console.WriteLine($"Starter: {physics.StarterEngineOn}");
        Console.WriteLine($"Engine Running: {physics.IsEngineRunning}");
        Console.WriteLine($"Break Compounds - Front: {physics.FrontBreakCompound}, Rear: {physics.RearBreakCompount}");
        Console.WriteLine($"Pad Life: {string.Join(", ", physics.PadLife.Select(x => x.ToString("F1")))}");
        Console.WriteLine($"Disc Life: {string.Join(", ", physics.DiscLife.Select(x => x.ToString("F1")))}");
        Console.WriteLine($"Car Damage: {string.Join(", ", physics.CarDamage.Select(x => x.ToString("F1")))}");

        Console.WriteLine($"Timestamp: {DateTime.UtcNow}");
        Console.WriteLine("==================\n");
    }

    private static void Ac_StaticInfoUpdated(object sender, StaticInfoEventArgs e)
    {
        var staticInfo = e.StaticInfo;

        Console.WriteLine($"=== Static Info Data ===");
        // Version Info
        Console.WriteLine($"SM Version: {staticInfo.SMVersion}");
        Console.WriteLine($"AC Version: {staticInfo.ACVersion}");

        // Session Info
        Console.WriteLine($"Number of Sessions: {staticInfo.NumberOfSessions}");
        Console.WriteLine($"Number of Cars: {staticInfo.NumCars}");
        Console.WriteLine($"Sector Count: {staticInfo.SectorCount}");
        Console.WriteLine($"Is Online: {staticInfo.IsOnline}");

        // Player Info
        Console.WriteLine($"Player Name: {staticInfo.PlayerName}");
        Console.WriteLine($"Player Surname: {staticInfo.PlayerSurname}");
        Console.WriteLine($"Player Nick: {staticInfo.PlayerNick}");

        // Car & Track Info
        Console.WriteLine($"Car Model: {staticInfo.CarModel}");
        Console.WriteLine($"Track: {staticInfo.Track}");
        Console.WriteLine($"Track Configuration: {staticInfo.TrackConfiguration}");
        Console.WriteLine($"Max RPM: {staticInfo.MaxRpm}");
        Console.WriteLine($"Max Fuel: {staticInfo.MaxFuel}");

        // Race Settings
        Console.WriteLine($"Penalties Enabled: {staticInfo.PenaltiesEnabled}");
        Console.WriteLine($"Pit Window Start: {staticInfo.PitWindowStart}");
        Console.WriteLine($"Pit Window End: {staticInfo.PitWindowEnd}");

        // Driving Aids
        Console.WriteLine($"Aid Settings:");
        Console.WriteLine($"  - Fuel Rate: {staticInfo.AidFuelRate}");
        Console.WriteLine($"  - Tire Rate: {staticInfo.AidTireRate}");
        Console.WriteLine($"  - Mechanical Damage: {staticInfo.AidMechanicalDamage}");
        Console.WriteLine($"  - Allow Tyre Blankets: {staticInfo.AidAllowTyreBlankets}");
        Console.WriteLine($"  - Stability: {staticInfo.AidStability}");
        Console.WriteLine($"  - Auto Clutch: {staticInfo.AidAutoClutch}");
        Console.WriteLine($"  - Auto Blip: {staticInfo.AidAutoBlip}");

        // Tyre Info
        Console.WriteLine($"Dry Tyres Name: {staticInfo.DryTyresName}");
        Console.WriteLine($"Wet Tyres Name: {staticInfo.WetTyresName}");

        Console.WriteLine($"Timestamp: {DateTime.UtcNow}");
        Console.WriteLine("==================\n");
    }

    private static void ac_GraphicsInterval(object sender, GraphicsEventArgs e)
    {
        var graphics = e.Graphics;

        Console.WriteLine($"=== Graphics Data ===");
        
        // Session Info
        Console.WriteLine($"Status: {graphics.Status}");
        Console.WriteLine($"Session Type: {graphics.Session}");
        Console.WriteLine($"Session Time Left: {graphics.SessionTimeLeft:F1}");
        Console.WriteLine($"Session Index: {graphics.SessionIndex}");
        
        // Lap Times
        Console.WriteLine($"Current Time: {graphics.CurrentTime}");
        Console.WriteLine($"Last Time: {graphics.LastTime}");
        Console.WriteLine($"Best Time: {graphics.BestTime}");
        Console.WriteLine($"Delta Lap Time: {graphics.DeltaLapTime}");
        Console.WriteLine($"Estimated Lap Time: {graphics.EstimatedLapTime}");
        Console.WriteLine($"Is Valid Lap: {graphics.IsValidLap}");
        
        // Race Position
        Console.WriteLine($"Position: {graphics.Position}");
        Console.WriteLine($"Completed Laps: {graphics.CompletedLaps}");
        Console.WriteLine($"Current Sector: {graphics.CurrentSectorIndex}");
        Console.WriteLine($"Last Sector Time: {graphics.LastSectorTime}");
        Console.WriteLine($"Distance Traveled: {graphics.DistanceTraveled:F1}");
        
        // Pit & Track Status
        Console.WriteLine($"In Pit: {graphics.IsInPit}");
        Console.WriteLine($"In Pit Lane: {graphics.IsInPitLane}");
        Console.WriteLine($"Mandatory Pit Done: {graphics.MandatoryPitDone}");
        Console.WriteLine($"Missing Mandatory Pits: {graphics.MissingMandatoryPits}");
        Console.WriteLine($"Track Status: {graphics.TrackStatus}");
        Console.WriteLine($"Surface Grip: {graphics.SurfaceGrip:F3}");
        Console.WriteLine($"Track Grip Status: {graphics.TrackGripStatus}");
        
        // Weather
        Console.WriteLine($"Wind Speed: {graphics.WindSpeed:F1}");
        Console.WriteLine($"Wind Direction: {graphics.WindDirection:F1}");
        Console.WriteLine($"Rain Intensity: {graphics.RainIntensity}");
        Console.WriteLine($"Rain in 10min: {graphics.RainIntensityIn10min}");
        Console.WriteLine($"Rain in 30min: {graphics.RainIntensityIn30min}");
        
        // Car Settings & Status
        Console.WriteLine($"TC: {graphics.TC}");
        Console.WriteLine($"TC Cut: {graphics.TCCUT}");
        Console.WriteLine($"ABS: {graphics.ABS}");
        Console.WriteLine($"Engine Map: {graphics.EngineMap}");
        Console.WriteLine($"Fuel per Lap: {graphics.FuelXLap:F3}");
        Console.WriteLine($"Used Fuel: {graphics.UsedFuel:F3}");
        Console.WriteLine($"Estimated Laps: {graphics.FuelEstimatedLaps:F1}");
        Console.WriteLine($"Tyre Compound: {graphics.TyreCompound}");
        Console.WriteLine($"Rain Tyres: {graphics.RainTyres}");
        
        // Car Lights & Visual
        Console.WriteLine($"Rain Lights: {graphics.RainLights}");
        Console.WriteLine($"Flashing Lights: {graphics.FlashingLights}");
        Console.WriteLine($"Lights Stage: {graphics.LightsStage}");
        Console.WriteLine($"Direction Lights: Left={graphics.DirectionLightsLeft}, Right={graphics.DirectionLightsRight}");
        
        // Flags & Penalties
        Console.WriteLine($"Flag: {graphics.Flag}");
        Console.WriteLine($"Penalty: {graphics.Penalty}");
        Console.WriteLine($"Penalty Time: {graphics.PenaltyTime:F1}");
        
        // MFD (Multi-Function Display) Settings
        Console.WriteLine($"MFD Tyre Set: {graphics.MfdTyreSet}");
        Console.WriteLine($"MFD Fuel to Add: {graphics.MfdFuelToAdd:F1}");
        Console.WriteLine($"MFD Tyre Pressures (LF,RF,LR,RR): {graphics.MfdTyrePressureLF:F1}, {graphics.MfdTyrePressureRF:F1}, {graphics.MfdTyrePressureLR:F1}, {graphics.MfdTyrePressureRR:F1}");

        Console.WriteLine($"Timestamp: {DateTime.UtcNow}");
        Console.WriteLine("==================\n");
    }
}
