using System.Text.RegularExpressions;

partial class Day8
{
    public async Task Run()
    {
        var input = await File.ReadAllLinesAsync($"{GetType()}/input.txt");

        Console.WriteLine("Running Day 8 - Part 1");

        var instructions = input[0];

        var nodes = input.Skip(2).Select(Node.ParseNode).ToDictionary(n => n.Id);

        var curr = nodes["AAA"];
        int steps = 0;
        int cursor = 0;
        while (curr.Id != "ZZZ")
        {
            curr = instructions[cursor] switch 
            {
                'L' => nodes[curr.Left],
                'R' => nodes[curr.Right],
                _ => curr
            };
            ++steps;
            cursor = (cursor + 1) % instructions.Length;
        }

        Console.WriteLine($"Steps to 'ZZZ' = {steps}");

        Console.WriteLine("Running Day 8 - Part 2");
    }

    partial record Node
    {
        public required string Id;
        public required string Left;
        public required string Right;

        public static Node ParseNode(string line)
        {
            var m = NodeRegex().Match(line);

            return new Node
            {
                Id = m.Groups[1].Value,
                Left = m.Groups[2].Value,
                Right = m.Groups[3].Value
            };
        }

        [GeneratedRegex("(\\w+) = \\((\\w+), (\\w+)\\)", RegexOptions.Compiled)]
        private static partial Regex NodeRegex();
    }
}
