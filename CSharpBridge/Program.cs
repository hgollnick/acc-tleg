using AssettoCorsaSharedMemory;
using System;

class Program
{
    static void Main(string[] args)
    {
        var ac = new AssettoCorsa();
        ac.PhysicsUpdated += Ac_PhysicsUpdated;
        ac.StaticInfoUpdated += ac_StaticInfoUpdated;
        ac.Start();

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();

        ac.Stop();
    }

    private static void Ac_PhysicsUpdated(object sender, PhysicsEventArgs e)
    {
        var physics = e.Physics;

        Console.WriteLine($"=== Physics Data ===");
        Console.WriteLine($"Speed: {physics.SpeedKmh} km/h");
        Console.WriteLine($"Gas/Throttle: {physics.Gas:F3}");
        Console.WriteLine($"Brake: {physics.Brake:F3}");
        Console.WriteLine($"Clutch: {physics.Clutch:F3}");
        Console.WriteLine($"Gear: {physics.Gear}");
        Console.WriteLine($"RPM: {physics.Rpms}");
        Console.WriteLine($"Steering Angle: {physics.SteerAngle:F3}");
        Console.WriteLine($"G-Force Lateral: {physics.AccG[0]:F3}");
        Console.WriteLine($"G-Force Longitudinal: {physics.AccG[1]:F3}");
        Console.WriteLine($"G-Force Vertical: {physics.AccG[2]:F3}");
        Console.WriteLine($"Wheel Angular Speed: FL:{physics.WheelAngularSpeed[0]:F1} FR:{physics.WheelAngularSpeed[1]:F1} RL:{physics.WheelAngularSpeed[2]:F1} RR:{physics.WheelAngularSpeed[3]:F1}");
        Console.WriteLine($"Slip Angle: FL:{physics.SlipAngle[0]:F1} FR:{physics.SlipAngle[1]:F1} RL:{physics.SlipAngle[2]:F1} RR:{physics.SlipAngle[3]:F1}");
        Console.WriteLine($"Slip Ratio: FL:{physics.SlipRatio[0]:F1} FR:{physics.SlipRatio[1]:F1} RL:{physics.SlipRatio[2]:F1} RR:{physics.SlipRatio[3]:F1}");
        Console.WriteLine($"Tyre Wear: FL:{physics.TyreWear[0]:F1} FR:{physics.TyreWear[1]:F1} RL:{physics.TyreWear[2]:F1} RR:{physics.TyreWear[3]:F1}");
        Console.WriteLine($"Tyre Temp: FL:{physics.TyreTemp[0]:F1} FR:{physics.TyreTemp[1]:F1} RL:{physics.TyreTemp[2]:F1} RR:{physics.TyreTemp[3]:F1}");
        Console.WriteLine($"Brake Temp: FL:{physics.BrakeTemp[0]:F1} FR:{physics.BrakeTemp[1]:F1} RL:{physics.BrakeTemp[2]:F1} RR:{physics.BrakeTemp[3]:F1}");
        Console.WriteLine($"Timestamp: {DateTime.UtcNow}");
        Console.WriteLine("==================\n");
    }

    private static void ac_StaticInfoUpdated(object sender, StaticInfoEventArgs e)
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
}
