using System.Text.RegularExpressions;

partial class Day8
{
    string _instructions;
    Dictionary<string, Node> _nodeMap;

    public async Task Run()
    {
        var input = await File.ReadAllLinesAsync($"{GetType()}/input.txt");

        Console.WriteLine("Running Day 8 - Part 1");

        _instructions = input[0];
        _nodeMap = input.Skip(2).Select(Node.ParseNode).ToDictionary(n => n.Id);

        var steps = CountStepsToEnd(_nodeMap["AAA"], n => n.Id == "ZZZ");
        Console.WriteLine($"Steps to 'ZZZ' = {steps}");
        
        Console.WriteLine("Running Day 8 - Part 2");

        // Through experimentation, I learned that:
        // - There are six start nodes and six end nodes.
        //   - starts: AAA, GQA, GVA, HBA, NVA, XCA
        //   - ends:   ZZZ, TKZ, KJZ, JLZ, HVZ, LLZ
        // - Following the instructions from each start node ends in a loop with the same end node.
        //   - AAA->ZZZ, GQA->TKZ, GVA->KJZ, HBA->JLZ, NVA->HVZ, and XCA->LLZ
        // - The time to reach the end the first time is the same as the rest. (WOW!)
        //   - 18727, 22411, 16271, 14429, 20569, 24253
        // Therefore:
        // - This is a Least Common Multiple problem.

        var startNodes = _nodeMap.Values.Where(n => n.Id[2] == 'A').ToList();
        var stepCounts = startNodes.Select(n => CountStepsToEnd(n, c => c.Id[2] == 'Z')).ToList();

        var ghostSteps = stepCounts.Aggregate(LeastCommonMultiple);
        
        Console.WriteLine($"Steps to concurrent nodes ending with 'Z' = {ghostSteps}");
    }

    private long CountStepsToEnd(Node start, Predicate<Node> endCheck)
    {
        long steps = 0;
        int cursor = 0;
        var curr = start;
        while (!endCheck(curr))
        {
            curr = _instructions[cursor] switch 
            {
                'L' => _nodeMap[curr.Left],
                'R' => _nodeMap[curr.Right],
                _ => curr
            };
            ++steps;
            cursor = (cursor + 1) % _instructions.Length;
        }
        return steps;
    }

    private long GreatestCommonFactor(long a, long b)
    {
        while (b != 0)
        {
            long tmp = b;
            b = a % b;
            a = tmp;
        }
        return a;
    }

    private long LeastCommonMultiple(long a, long b)
    {
        return a / GreatestCommonFactor(a, b) * b;
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
