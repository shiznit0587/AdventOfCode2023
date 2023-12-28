class Day14
{
    public async Task Run()
    {
        var input = await File.ReadAllLinesAsync($"{GetType()}/input.txt");

        Console.WriteLine("Running Day 14 - Part 1");

        var platform = input.Select(line => line.Select(FromChar).ToList()).ToList();

        for (int x = 0; x < platform.Count; ++x)
        {
            var row = platform[x];
            for (int y = 0; y < row.Count; ++y)
                if (row[y] == Rock.Round)
                    for (int i = x; i >= 0; --i)
                        // if the next one is a blocker, the rock will roll to here.
                        if (i <= 0 || platform[i - 1][y] != Rock.Empty)
                        {
                            platform[i][y] = Rock.Round;
                            // Don't clear it if it can't roll.
                            if (i != x)
                                platform[x][y] = Rock.Empty;
                            break;
                        }
        }

        // All the rocks have now rolled. Let's tally the total load now.
        int totalLoad = 0;
        for (int x = 0; x < platform.Count; ++x)
            for (int y = 0; y < platform[x].Count; ++y)
                if (platform[x][y] == Rock.Round)
                    totalLoad += platform.Count - x;

        Console.WriteLine($"Total load = {totalLoad}");

        Console.WriteLine("Running Day 14 - Part 2");
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
}
