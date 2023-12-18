class Day12
{
    public async Task Run()
    {
        var input = await File.ReadAllLinesAsync($"{GetType()}/input.txt");

        Console.WriteLine("Running Day 12 - Part 1");

        long matchCountSum = 0;
        foreach (var line in input)
        {
            var parts = line.Split(' ');
            List<char> row = [..parts[0]];
            var groups = parts[1].Split(',').Select(int.Parse).ToList();

            matchCountSum += CountMatches(row, groups);
        }

        Console.WriteLine($"Valid Arrangements = {matchCountSum}");

        Console.WriteLine("Running Day 12 - Part 2");

        matchCountSum = 0;
        Parallel.For(0, input.Length, i =>
        {
            string line = input[i];
            var parts = line.Split(' ');
            List<char> row = [..parts[0], '?', ..parts[0], '?', ..parts[0], '?', ..parts[0], '?', ..parts[0]];
            var groups = parts[1].Split(',').Select(int.Parse).ToList();
            groups = [..groups, ..groups, ..groups, ..groups, ..groups];

            long matches = CountMatches(row, groups);
            Interlocked.Add(ref matchCountSum, matches);
        });

        Console.WriteLine($"Valid Arrangements = {matchCountSum}");
    }

    private static long CountMatches(List<char> row, List<int> groups, int i = 0, int currentGroupSize = 0)
    {
        if (i == row.Count)
            return (groups.Count, currentGroupSize) switch
            {
                (0, 0) => 1,
                (1, _) when currentGroupSize == groups[0] => 1,
                _ => 0
            };

        return (row[i], groups.Count, currentGroupSize) switch
        {
            ('.', _, 0) => CountMatches(row, groups, i + 1, currentGroupSize),
            ('.', > 0, _) when groups[0] == currentGroupSize => CountMatches(row, groups[1..], i + 1, 0),
            ('.', _, _) => 0,
            ('#', 0, _) => 0,
            ('#', _, _) when currentGroupSize < groups[0] => CountMatches(row, groups, i + 1, currentGroupSize + 1),
            ('#', _, _) => 0,
            (_, _, _) => 
                CountMatches([..row[0..i] , '#', ..row[(i + 1)..]], groups, i, currentGroupSize) + 
                CountMatches([..row[0..i], '.', ..row[(i + 1)..]], groups, i, currentGroupSize)
        };
    }
}
