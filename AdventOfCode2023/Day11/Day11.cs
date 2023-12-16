class Day11
{
    public async Task Run()
    {
        var input = await File.ReadAllLinesAsync($"{GetType()}/input.txt");

        Console.WriteLine("Running Day 11 - Part 1");

        List<(int x, int y)> galaxies = [];
        for (int y = 0; y < input.Length; ++y)
            for (int x = 0; x < input[y].Length; ++x)
                if (input[y][x] == '#')
                    galaxies.Add((x, y));

        HashSet<int> galaxyRows = galaxies.Select(g => g.y).ToHashSet();
        HashSet<int> galaxyCols = galaxies.Select(g => g.x).ToHashSet();

        int lengthSum = 0;
        for (int i = 0; i < galaxies.Count - 1; ++i)
            for (int j = i + 1; j < galaxies.Count; ++j)
            {
                var (ax, ay) = galaxies[i];
                var (bx, by) = galaxies[j];
                LowHigh(ref ax, ref bx);
                LowHigh(ref ay, ref by);
                int dx = bx - ax;
                int dy = by - ay;

                dx += Enumerable.Range(ax, dx).Where(x => !galaxyCols.Contains(x)).Count();
                dy += Enumerable.Range(ay, dy).Where(x => !galaxyRows.Contains(x)).Count();

                lengthSum += dx + dy;             
            }

        Console.WriteLine($"Sum of shortest path lengths = {lengthSum}");

        Console.WriteLine("Running Day 11 - Part 2");
    }

    private static void LowHigh(ref int a, ref int b)
    {
        if (a > b)
            (a, b) = (b, a);
    }
}
