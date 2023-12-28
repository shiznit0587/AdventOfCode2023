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

        var (sum1, sum2) = (0, 0);
        foreach (var p in patterns)
        {
            var split = p.FindSplits().First();
            sum1 += SplitToNotesValue(split);
            sum2 += SplitToNotesValue(GetCoords(p.Rows.Count, p.Cols.Count)
                .Select(c => p.Smudged(c.x, c.y).FindSplits().Where(s => s != split).ToList())
                .Where(splits => splits.Count != 0)
                .First().First());
        }

        Console.WriteLine($"Summary of notes 1 = {sum1}");
        Console.WriteLine("Running Day 13 - Part 2");
        Console.WriteLine($"Summary of notes 2 = {sum2}");
    }

    private static int SplitToNotesValue((Orientation? orientation, int line) split) =>
        split switch
        {
            (Orientation.Horizontal, int l) => l * 100,
            (Orientation.Vertical, int l) => l,
            _ => 0
        };

    private static IEnumerable<(int x, int y)> GetCoords(int xMax, int yMax)
    {
        for (int x = 0; x < xMax; ++x)
            for (int y = 0; y < yMax; ++y)
                yield return (x, y);
    }

    enum Orientation
    {
        Horizontal,
        Vertical
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

        Pattern(List<string> rows, List<string> cols)
        {
            Rows = rows;
            Cols = cols;
        }

        public IEnumerable<(Orientation orientation, int line)> FindSplits()
        {
            foreach (int line in FindSplits(Rows))
                yield return (Orientation.Horizontal, line);

            foreach (int line in FindSplits(Cols))
                yield return (Orientation.Vertical, line);
        }

        private static IEnumerable<int> FindSplits(List<string> lines)
        {
            for (double i = 0.5; i < lines.Count - 1; ++i)
            {
                bool reflectionLine = true;
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
                    yield return (int)Math.Ceiling(i);
                }
            }
        }

        public Pattern Smudged(int x, int y)
        {
            Pattern p = new([..Rows], [..Cols]);

            p.Rows[x] = string.Join("", p.Rows[x].Select((c, i) => FixSmudge(c, i, y)));
            p.Cols[y] = string.Join("", p.Cols[y].Select((c, i) => FixSmudge(c, i, x)));

            return p;
        }

        private static char FixSmudge(char c, int idx, int smudgeIdx) =>
            idx == smudgeIdx ? (c == '#' ? '.' : '#') : c;
    }
}
