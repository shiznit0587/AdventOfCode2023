class Day15
{
    public async Task Run()
    {
        var input = (await File.ReadAllLinesAsync($"{GetType()}/input.txt"))[0].Split(',');

        Console.WriteLine("Running Day 15 - Part 1");

        Console.WriteLine($"Hash sums = {input.Select(RunHash).Sum()}");

        Console.WriteLine("Running Day 15 - Part 2");

        var boxes = Enumerable.Range(0, 256).Select(i => new List<Lens>()).ToList();

        foreach (var instr in input.Select(InitStep.Parse))
        {
            var box = boxes[instr.Box];
            var lens = box.Find(l => l.Label == instr.Label);

            switch (instr.Removal, lens)
            {
                case (true, not null):
                    box.Remove(lens);
                    break;
                case (false, not null):
                    lens.FocalLength = instr.FocalLength;
                    break;
                case (false, null):
                    box.Add(new Lens { Label = instr.Label, FocalLength = instr.FocalLength });
                    break;
            }
        }

        int totalFocusingPower = 0;
        for (int i = 0; i < boxes.Count; ++i)
            for (int j = 0; j < boxes[i].Count; ++j)
                totalFocusingPower += (i + 1) * (j + 1) * boxes[i][j].FocalLength;

        Console.WriteLine($"Total focusing power = {totalFocusingPower}");
    }

    static int RunHash(string str) =>
        str.Aggregate(0, (h, c) => (h + c) * 17 % 256);

    class Lens
    {
        public required string Label;
        public int FocalLength;
    }

    class InitStep
    {
        public string Label;
        public int Box;
        public int FocalLength;
        public bool Removal;

        public InitStep(string label)
        {
            Label = label;
            Box = RunHash(Label);
        }

        public InitStep(string label, int focalLength)
            : this(label)
        {
            FocalLength = focalLength;
        }

        public static InitStep Parse(string instr)
        {
            if (instr[^1] == '-')
                return new InitStep(instr[0..^1]) { Removal = true };

            var parts = instr.Split('=');
            return new InitStep(parts[0], int.Parse(parts[1]));
        }
    }
}
