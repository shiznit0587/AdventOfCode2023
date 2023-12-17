class Day12
{
    public async Task Run()
    {
        var input = await File.ReadAllLinesAsync($"{GetType()}/input.txt");

        Console.WriteLine("Running Day 12 - Part 1");

        int validArrangementSum = 0;
        foreach (var line in input)
        {
            var parts = line.Split(' ');
            var counts = parts[1].Split(',');
            var damagedRegions = counts.Select(int.Parse).ToList();
            var totalDamaged = damagedRegions.Sum();
            var knownDamaged = parts[0].Count(c => c == '#');
            var unknownDamaged = totalDamaged - knownDamaged;
            var unknownIndexes = parts[0].Select((c, i) => c == '?' ? i : -1).Where(i => i != -1).ToList();
            var unknowns = unknownIndexes.Count;

            var permutations = NChooseK(unknownIndexes, unknownDamaged).ToList();

            int validArrangements = 0;
            foreach (var permutation in permutations)
            {
                var testLine = string.Join("", line.Select((c, i) => 
                {
                    return c switch
                    {
                        '#' => '#',
                        '.' => '.',
                        _ when permutation.Contains(i) => '#',
                        _ => '.'
                    };
                }));

                if (testLine.Split('.', StringSplitOptions.RemoveEmptyEntries).Select(c => c.Length).SequenceEqual(damagedRegions))
                {
                    ++validArrangements;
                }
            }

            validArrangementSum += validArrangements;
        }

        Console.WriteLine($"Valid Arrangements = {validArrangementSum}");
        // I input answer 7715 and it was too low...

        Console.WriteLine("Running Day 12 - Part 2");
    }

    private static IEnumerable<HashSet<int>> NChooseK(List<int> n, int k, HashSet<int>? choices = null)
    {
        choices ??= [];

        for (int i = 0; i < n.Count; ++i)
        {
            var option = n[i];
            var remainingN = n[(i + 1)..];

            var choicesWithOption = choices.ToHashSet();
            choicesWithOption.Add(option);

            if (k == 1)
                yield return choicesWithOption;
            else
                foreach (var response in NChooseK(remainingN, k - 1, choicesWithOption))
                    yield return response;
        }
    }
}
