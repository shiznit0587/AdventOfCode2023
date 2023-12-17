class Day12
{
    public async Task Run()
    {
        var input = await File.ReadAllLinesAsync($"{GetType()}/input.txt");

        Console.WriteLine("Running Day 12 - Part 1");

        int matchCountSum = 0;
        foreach (var line in input)
        {
            var parts = line.Split(' ');
            List<char> row = [..parts[0]];
            var groups = parts[1].Split(',').Select(int.Parse).ToList();

            int matches = CountMatches(row, groups);
            matchCountSum += matches;
        }

        Console.WriteLine($"Valid Arrangements = {matchCountSum}");

        Console.WriteLine("Running Day 12 - Part 2");
    }

    private static int CountMatches(List<char> row, List<int> groups, int i = 0, int currentGroupSize = 0)
    {
        if (i == row.Count)
        {
            if (groups.Count == 1 && currentGroupSize == groups[0])
            {
                return 1;
            }
            else if (groups.Count == 0 && currentGroupSize == 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        if (row[i] == '.')
        {
            if (groups.Count == 0 && currentGroupSize == 0)
            {
                // valid so far, keep going
                return CountMatches(row, groups, i + 1, currentGroupSize);
            }
            else
            {
                if (currentGroupSize == 0)
                {
                    // keep moving
                    return CountMatches(row, groups, i + 1, currentGroupSize);
                }
                // See if the previous group matches the size of the first unmatched group
                if (groups[0] == currentGroupSize)
                {
                    return CountMatches(row, groups[1..], i + 1, 0);
                }
                else
                {
                    // This means groups[0] was unsatisfied.
                    return 0;
                }
            }
        }
        else if (row[i] == '#')
        {
            if (groups.Count == 0)
            {
                // no groups left but extra damaged spring.
                return 0;
            }
            // See if the current group's size is <= the first unmatched group

            else if (currentGroupSize < groups[0])
            {
                // Then we can add one and still be a match. Try it.
                return CountMatches(row, groups, i + 1, currentGroupSize + 1);
            }
            else
            {
                // we make too big a group to match.
                return 0;
            }
        }
        else // if (row[i] == '?')
        {
            // Try a #
            List<char> testDamaged = [..row[0..i] , '#', ..row[(i + 1)..]];
            // Try a .
            List<char> testOperational = [..row[0..i], '.', ..row[(i + 1)..]];

            return CountMatches(testDamaged, groups, i, currentGroupSize) + CountMatches(testOperational, groups, i, currentGroupSize);
        }
    }
}
