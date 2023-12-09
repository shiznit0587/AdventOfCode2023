// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Reflection;

Console.WriteLine("\n🎅🎅🎅🎅🎅 ADVENT OF CODE 2023 🎅🎅🎅🎅🎅\n");

var dayTimer = new Stopwatch();
var yearTimer = new Stopwatch();

static async Task RunDay(Stopwatch w, int day)
{
    Type? dayType = Type.GetType($"Day{day}");
    if (dayType == null)
    {
        return;
    }

    MethodInfo runMethod = dayType.GetMethod("Run")!;
    var dayInstance = Activator.CreateInstance(dayType);

    Console.WriteLine($"Running Day {day}...");
    w.Restart();

    #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
    #pragma warning disable CS8602 // Dereference of a possibly null reference.
    await (Task)runMethod.Invoke(dayInstance, null);
    #pragma warning restore CS8602 // Dereference of a possibly null reference.
    #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    
    w.Stop();
    Console.WriteLine($"Day {day} Time = {w.Elapsed}");
}

yearTimer.Start();

for (int i = 1; i <=25; ++i) {
    await RunDay(dayTimer, i);
}

yearTimer.Stop();

Console.WriteLine($"Total Time = {yearTimer.Elapsed}");
