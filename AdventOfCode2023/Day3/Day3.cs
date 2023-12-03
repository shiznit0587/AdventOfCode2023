class Day3
{
    public async Task Run()
    {
        var input = await File.ReadAllLinesAsync($"{GetType()}/input.txt");

        Console.WriteLine("Running Day 3 - Part 1");

        List<NumberData> numbers = [];
        List<SymbolData> symbols = [];

        for (int j = 0; j < input.Length; ++j)
        {
            string line = input[j];
            int? start = null;

            for (int i = 0; i < line.Length; ++i)
            {
                if (char.IsDigit(line[i]))
                {
                    start ??= i;
                }
                else 
                {
                    if (start is int st)
                    {
                        numbers.Add(new NumberData 
                        { 
                            Value = int.Parse(line[st..i]),
                            Start = Tuple.Create(j, st),
                            End = Tuple.Create(j, i - 1) 
                        });
                        start = null;
                    }
                    
                    if (line[i] != '.')
                    {
                        symbols.Add(new SymbolData { Symbol = line[i], Position = Tuple.Create(j, i) });
                    }
                }
            }

            if (start is int s)
            {
                numbers.Add(new NumberData 
                { 
                    Value = int.Parse(line[s..]),
                    Start = Tuple.Create(j, s),
                    End = Tuple.Create(j, line.Length - 1) 
                });
            }
        }

        int sum = numbers
                .Where(n => symbols.Any(s => SymbolAdjacent(s, n)))
                .Select(n => n.Value).Sum();

        Console.WriteLine($"sum = {sum}");

        Console.WriteLine("Running Day 3 - Part 2");

        sum = 0;
        foreach (var symbol in symbols.Where(s => s.Symbol == '*'))
        {
            var adjNumbers = numbers.Where(n => SymbolAdjacent(symbol, n)).ToList();
            if (adjNumbers.Count == 2)
            {
                sum += adjNumbers[0].Value * adjNumbers[1].Value;
            }
        }
        
        Console.WriteLine($"sum = {sum}");
    }

    private bool SymbolAdjacent(SymbolData symbol, NumberData number)
    {
        return WithinRange(symbol.Position.Item1, number.Start.Item1 - 1, number.End.Item1 + 1) &&
            WithinRange(symbol.Position.Item2, number.Start.Item2 - 1, number.End.Item2 + 1);
    }

    private bool WithinRange(int x, int a, int b) => a <= x && x <= b;

    class SymbolData
    {
        public required char Symbol;
        public required Tuple<int, int> Position;
    }

    class NumberData
    {
        public required int Value;
        public required Tuple<int, int> Start;
        public required Tuple<int, int> End;
    }
}
