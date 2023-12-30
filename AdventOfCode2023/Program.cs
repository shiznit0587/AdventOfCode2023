// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Reflection;

Console.WriteLine("\n🎅🎅🎅🎅🎅 ADVENT OF CODE 2023 🎅🎅🎅🎅🎅\n");

var dayTimer = new Stopwatch();
var yearTimer = new Stopwatch();

const bool RUN_ONE_DAY = true; // defaults to running newest day.
const int DAY_TO_RUN = 15; // When running one day, optional specification of which day to run.

static async Task<bool> RunDay(Stopwatch w, int day)
{
    if (DAY_TO_RUN != -1 && DAY_TO_RUN != day)
        return false;

    Type? dayType = Type.GetType($"Day{day}");
    if (dayType == null)
        return false;

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

    return true;
}

yearTimer.Start();

if (RUN_ONE_DAY)
{
    for (int i = 25; i >= 1; --i)
        if (await RunDay(dayTimer, i))
            break;
}
else
{
    for (int j = 1; j <= 25; ++j)
        await RunDay(dayTimer, j);
}

yearTimer.Stop();

Console.WriteLine($"Total Time = {yearTimer.Elapsed}");
