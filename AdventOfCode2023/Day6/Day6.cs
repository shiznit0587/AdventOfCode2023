class Day6
{
    public async Task Run()
    {
        var input = await File.ReadAllLinesAsync($"{GetType()}/input.txt");

        Console.WriteLine("Running Day 6 - Part 1");

        var times = input[0][(input[0].IndexOf(':') + 1)..].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
        var distances = input[1][(input[1].IndexOf(':') + 1)..].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();

        var winningWaysProduct = Enumerable.Zip(times, distances)
            .Select(r => CalcWinnersBruteForce(r.First, r.Second))
            .Aggregate(1, (x, y) => x * y);

        Console.WriteLine($"Winning Ways Product = {winningWaysProduct}");

        Console.WriteLine("Running Day 6 - Part 2");

        var raceTime = long.Parse(input[0][(input[0].IndexOf(':') + 1)..].Replace(" ", ""));
        var raceDistance = long.Parse(input[1][(input[1].IndexOf(':') + 1)..].Replace(" ", ""));

        var wins = CalcWinnersQuadratic(raceTime, raceDistance);

        Console.WriteLine($"Winning Ways = {wins}");
    }

    private static int CalcWinnersBruteForce(long raceTime, long raceDistance)
    {
        int wins = 0;
        for (int holdTime = 1; holdTime < raceDistance - 1; ++holdTime)
        {
            var distancePerMs = holdTime;
            var timeRemaining = raceTime - holdTime;
            var distance = timeRemaining * distancePerMs;

            if (distance > raceDistance)
                ++wins;
            else if (wins > 0)
                break;
        }
        return wins;
    }

    private static int CalcWinnersQuadratic(long raceTime, long raceDistance)
    {
        // T = time, X = hold time (our variable), D = distance
        // (T - X) * X > D
        // Solve for X
        // T * X - X^2 > D
        // -1 * X^2 + T * X - D > 0
        // a = -1, b = T, c = -D
        // use the quadratic formula to find the roots
        // (-b +- sqrt(b^2 - 4ac)) / (2a)
        
        long a = -1, b = raceTime, c = -raceDistance;

        var sqrt = Math.Sqrt(b * b - 4 * a * c);
        var root1 = (-a - sqrt) / (2 * a);
        var root2 = (-a + sqrt) / (2 * a);

        return (int)Math.Floor(root1 - root2);
    }
}
