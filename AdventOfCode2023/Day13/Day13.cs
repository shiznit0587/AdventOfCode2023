class Day13
{
    public async Task Run()
    {
        var input = await File.ReadAllLinesAsync($"{GetType()}/input.txt");

        Console.WriteLine("Running Day 13 - Part 1");

        List<Pattern> patterns = [];

        int patternStart = 0;
        for (int i = 0; i < input.Length; ++i)
            if (string.IsNullOrEmpty(input[i]))
            {
                patterns.Add(new([.. input[patternStart..i]]));
                patternStart = i + 1;
            }
        patterns.Add(new([.. input[patternStart..]]));

        var (cols, rows) = (0, 0);
        foreach (var pattern in patterns)
        {
            (bool split, int line) = FindSplit(pattern.Rows);
            if (split)
            {
                rows += line;
                continue;
            }

            (split, line) = FindSplit(pattern.Cols);
            cols += line;
        }

        Console.WriteLine($"Summary of notes = {cols + 100 * rows}");

        Console.WriteLine("Running Day 13 - Part 2");
    }

    private static (bool, int) FindSplit(List<string> lines)
    {
        (bool reflectionLine, double i) = (true, 0.5);

        for (; i < lines.Count - 1; ++i)
        {
            reflectionLine = true;
            var (left, right) = ((int)Math.Floor(i), (int)Math.Ceiling(i));

            while (left >= 0 && right < lines.Count)
            {
                if (lines[left] != lines[right])
                {
                    reflectionLine = false;
                    break;
                }
                --left;
                ++right;
            }

            if (reflectionLine)
            {
                break;
            }
        }

        return (reflectionLine, (int)Math.Ceiling(i));
    }

    class Pattern
    {
        public readonly List<string> Rows;
        public readonly List<string> Cols;

        public Pattern(List<string> rows)
        {
            Rows = rows;
            Cols = [];

            for (int i = 0; i < Rows[0].Length; ++i)
                Cols.Add(string.Join("", Rows.Select(r => r[i])));
        }
    }
}
