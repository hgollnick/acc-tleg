using AssettoCorsaSharedMemory;
using System;

class Program
{
    static void Main(string[] args)
    {
        var ac = new AssettoCorsa();
        ac.PhysicsUpdated += Ac_PhysicsUpdated;  // subscribe to live telemetry events
        ac.Start();

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();

        ac.Stop();
    }

    private static void Ac_PhysicsUpdated(object sender, PhysicsEventArgs e)
    {
        var physics = e.Physics;

        Console.WriteLine($"Speed: {physics.SpeedKmh} km/h, " +
                          $"Throttle: {physics.Gas}, " +
                          $"Brake: {physics.Brake}, " +
                          $"Gear: {physics.Gear}, " +
                          $"Timestamp: {DateTime.UtcNow}");
    }
}
