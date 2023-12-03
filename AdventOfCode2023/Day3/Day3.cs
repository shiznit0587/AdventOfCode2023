class Day3
{
    public async Task Run()
    {
        var input = await File.ReadAllLinesAsync($"{GetType()}/input.txt");

        Console.WriteLine("Running Day 3 - Part 1");

        List<NumberData> numbers = new List<NumberData>();
        List<SymbolData> symbols = new List<SymbolData>();

        for (int j = 0; j < input.Length; ++j)
        {
            string line = input[j];

            NumberData? number = null;

            for (int i = 0; i < line.Length; ++i)
            {
                
                if (char.IsDigit(line[i]))
                {
                    int digit = int.Parse($"{line[i]}");
                    if (number == null)
                    {
                        number = new NumberData { Value = digit, Start = Tuple.Create(j, i) };
                        numbers.Add(number);
                    }
                    else
                    {
                        number.Value = number.Value * 10 + digit;
                    }
                }
                else 
                {
                    if (number != null)
                    {
                        number.End = Tuple.Create(j,i - 1);
                        number = null;
                    }
                    
                    if (line[i] != '.')
                    {
                        symbols.Add(new SymbolData { Symbol = line[i], Position = Tuple.Create(j, i) });
                    }
                }
            }

            if (number != null)
            {
                number.End = Tuple.Create(j, line.Length - 1);
            }
        }

        int sum = 0;
        foreach(var number in numbers)
        {
            
            if (symbols.Any(s => SymbolAdjacent(s, number)))
            {
                sum += number.Value;
            }
        }

        Console.WriteLine($"sum = {sum}");

        Console.WriteLine("Running Day 3 - Part 2");

        sum = 0;
        foreach (var symbol in symbols)
        {
            if (symbol.Symbol == '*')
            {
                var adjNumbers = numbers.Where(n => SymbolAdjacent(symbol, n)).ToList();
                if (adjNumbers.Count == 2)
                {
                    sum += adjNumbers[0].Value * adjNumbers[1].Value;
                }
            }
        }
        
        Console.WriteLine($"sum = {sum}");
    }

    private bool SymbolAdjacent(SymbolData symbol, NumberData number)
    {
        return WithinRange(symbol.Position.Item1, number.Start.Item1 - 1, number.End.Item1 + 1) &&
            WithinRange(symbol.Position.Item2, number.Start.Item2 - 1, number.End.Item2 + 1);
    }

    private bool WithinRange(int x, int a, int b)
    {
        return a <= x && x <= b;
    }

    class SymbolData
    {
        public char Symbol;
        public Tuple<int, int> Position;
    }

    class NumberData
    {
        public int Value;
        public Tuple<int, int> Start;
        public Tuple<int, int> End;
    }
}
