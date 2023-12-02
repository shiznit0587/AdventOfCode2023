class Day1
{
    public async Task Run()
    {
        var input = await File.ReadAllLinesAsync($"{GetType()}/input.txt");

        Console.WriteLine("Running Day 1 - Part 1");

        int sum = input.Aggregate(0, (sum, line) => sum + ExtractCalibrationValue1(line));
        
        Console.WriteLine($"Sum = {sum}");
        Console.WriteLine("Running Day 1 - Part 2");

        sum = input.Aggregate(0, (sum, line) => sum + ExtractCalibrationValue2(line));

        Console.WriteLine($"Sum = {sum}");
    }

    public int ExtractCalibrationValue1(string line)
    {
        int first = -1;
        int last = -1;

        for (int i = 0; i < line.Length; ++i)
        {
            if (char.IsDigit(line[i]))
            {
                int digit = int.Parse($"{line[i]}");
                if (first < 0)
                    first = digit;
                last = digit;
            }
        }

        return first * 10 + last;
    }

    public int ExtractCalibrationValue2(string line)
    {
        int first = -1;
        int last = -1;

        for (int i = 0; i < line.Length; ++i)
        {
            int digit = -1;

            if (line.IndexOf("one", i) == i)
                digit = 1;
            else if (line.IndexOf("two", i) == i)
                digit = 2;
            else if (line.IndexOf("three", i) == i)
                digit = 3;
            else if (line.IndexOf("four", i) == i)
                digit = 4;
            else if (line.IndexOf("five", i) == i)
                digit = 5;
            else if (line.IndexOf("six", i) == i)
                digit = 6;
            else if (line.IndexOf("seven", i) == i)
                digit = 7;
            else if (line.IndexOf("eight", i) == i)
                digit = 8;
            else if (line.IndexOf("nine", i) == i)
                digit = 9;
            else if (char.IsDigit(line[i]))
                digit = int.Parse($"{line[i]}");

            if (first < 0)
                first = digit;
            if (digit >= 0)
                last = digit;
        }

        return first * 10 + last;    
    }
}
