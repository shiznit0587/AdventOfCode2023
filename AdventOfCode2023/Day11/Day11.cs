class Day11
{
    private readonly List<(int x, int y)> galaxies = [];
    private HashSet<int> galaxyRows;
    private HashSet<int> galaxyCols;

    public async Task Run()
    {
        var input = await File.ReadAllLinesAsync($"{GetType()}/input.txt");

        Console.WriteLine("Running Day 11 - Part 1");

        for (int y = 0; y < input.Length; ++y)
            for (int x = 0; x < input[y].Length; ++x)
                if (input[y][x] == '#')
                    galaxies.Add((x, y));

        galaxyRows = galaxies.Select(g => g.y).ToHashSet();
        galaxyCols = galaxies.Select(g => g.x).ToHashSet();

        Console.WriteLine($"Sum of shortest path lengths = {CalcLengthSum(2)}");

        Console.WriteLine("Running Day 11 - Part 2");

        Console.WriteLine($"Sum of shortest path lengths = {CalcLengthSum(1000000)}");
    }

    private long CalcLengthSum(int age)
    {
        long lengthSum = 0;
        for (int i = 0; i < galaxies.Count - 1; ++i)
            for (int j = i + 1; j < galaxies.Count; ++j)
            {
                var (ax, ay) = galaxies[i];
                var (bx, by) = galaxies[j];
                lengthSum += CalcLength(ax, ay, bx, by, age);         
            }
        return lengthSum;
    }

    private int CalcLength(int ax, int ay, int bx, int by, int age)
    {
        LowHigh(ref ax, ref bx);
        LowHigh(ref ay, ref by);
        int dx = bx - ax;
        int dy = by - ay;

        dx += Enumerable.Range(ax, dx).Count(x => !galaxyCols.Contains(x)) * (age - 1);
        dy += Enumerable.Range(ay, dy).Count(x => !galaxyRows.Contains(x)) * (age - 1);

        return dx + dy;
    }

    private static void LowHigh(ref int a, ref int b)
    {
        if (a > b)
            (a, b) = (b, a);
    }
}
