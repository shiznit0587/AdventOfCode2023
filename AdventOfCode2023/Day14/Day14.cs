class Day14
{
    List<List<Rock>>? platform;

    public async Task Run()
    {
        var input = await File.ReadAllLinesAsync($"{GetType()}/input.txt");

        Console.WriteLine("Running Day 14 - Part 1");

        platform = input.Select(line => line.Select(FromChar).ToList()).ToList();

        RollNorth();

        Console.WriteLine($"Total load = {CalculateLoad()}");

        Console.WriteLine("Running Day 14 - Part 2");

        platform = input.Select(line => line.Select(FromChar).ToList()).ToList();

        List<int> hashes = [];
        List<int> loads = [];
        Dictionary<int, List<int>> hashToCycles = [];

        var (loopStart, loopLength) = (0, 0);
        for (int i = 0; loopLength == 0; ++i)
        {
            Spin();
            var hash = GetPlatformHashCode();

            hashes.Add(hash);
            loads.Add(CalculateLoad());
            hashToCycles.TryAdd(hash, []);
            var hashCycles = hashToCycles[hash];
            hashCycles.Add(i);

            if (hashCycles.Count >= 3)
            {
                var r1 = hashes[hashCycles[^3]..hashCycles[^2]];
                var r2 = hashes[hashCycles[^2]..hashCycles[^1]];
                if (r1.SequenceEqual(r2))
                {
                    loopStart = hashCycles[^3];
                    loopLength = hashCycles[^2] - hashCycles[^3];
                }
            }
        }

        var load = loads[loopStart + ((999999999 - loopStart) % loopLength)];
        Console.WriteLine($"Total load after 1,000,000,000 Cycles = {load}");
    }

    int GetPlatformHashCode() =>
        string.Join("", platform!.Select(p => string.Join("", p.Select(ToChar)))).GetHashCode();

    void Spin()
    {
        RollNorth();
        RollWest();
        RollSouth();
        RollEast();
    }

    void RollNorth()
    {
        for (int x = 0; x < platform!.Count; ++x)
            for (int y = 0; y < platform[x].Count; ++y)
                if (platform[x][y] == Rock.Round)
                    for (int i = x; i >= 0; --i)
                        if (i <= 0 || platform[i - 1][y] != Rock.Empty)
                        {
                            platform[i][y] = Rock.Round;
                            if (i != x)
                                platform[x][y] = Rock.Empty;
                            break;
                        }
    }

    void RollSouth()
    {
        for (int x = platform!.Count - 1; x >= 0; --x)
            for (int y = 0; y < platform[x].Count; ++y)
                if (platform[x][y] == Rock.Round)
                    for (int i = x; i < platform.Count; ++i)
                        if (i >= platform.Count - 1 || platform[i + 1][y] != Rock.Empty)
                        {
                            platform[i][y] = Rock.Round;
                            if (i != x)
                                platform[x][y] = Rock.Empty;
                            break;
                        }
    }

    void RollWest()
    {
        for (int y = 0; y < platform![0].Count; ++y)
            for (int x = 0; x < platform.Count; ++x)
                if (platform[x][y] == Rock.Round)
                    for (int i = y; i >= 0; --i)
                        if (i <= 0 || platform[x][i - 1] != Rock.Empty)
                        {
                            platform[x][i] = Rock.Round;
                            if (i != y)
                                platform[x][y] = Rock.Empty;
                            break;
                        }
    }

    void RollEast()
    {
        for (int y = platform![0].Count - 1; y >= 0; --y)
            for (int x = 0; x < platform.Count; ++x)
                if (platform[x][y] == Rock.Round)
                    for (int i = y; i <= platform[0].Count; ++i)
                        if (i >= platform[0].Count - 1 || platform[x][i + 1] != Rock.Empty)
                        {
                            platform[x][i] = Rock.Round;
                            if (i != y)
                                platform[x][y] = Rock.Empty;
                            break;
                        }
    }

    int CalculateLoad()
    {
        int totalLoad = 0;
        for (int x = 0; x < platform!.Count; ++x)
            for (int y = 0; y < platform[x].Count; ++y)
                if (platform[x][y] == Rock.Round)
                    totalLoad += platform.Count - x;
        return totalLoad;
    }

    enum Rock
    {
        Round,
        Cubed,
        Empty,
    }

    static Rock FromChar(char c) =>
        c switch
        {
            'O' => Rock.Round,
            '#' => Rock.Cubed,
            _ => Rock.Empty
        };

    static char ToChar(Rock r) =>
        r switch
        {
            Rock.Round => '0',
            Rock.Cubed => '#',
            _ => '.'
        };

}
