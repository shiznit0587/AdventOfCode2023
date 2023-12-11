class Day9
{
    public async Task Run()
    {
        var input = await File.ReadAllLinesAsync($"{GetType()}/input.txt");

        Console.WriteLine("Running Day 9 - Part 1");

        var sum = input.Select(l => l.Split(' ').Select(int.Parse).ToList()).Select(Extrapolate).Sum();

        Console.WriteLine($"Sum of extrapolated values = {sum}");

        Console.WriteLine("Running Day 9 - Part 2");
    }

    public int Extrapolate(List<int> sequence)
    {
        int extrapolation = 0;
        while (sequence.Any(v => v != 0))
        {
            extrapolation += sequence.Last();
            sequence = Reduce(sequence);
        }
        return extrapolation;
    }

    public List<int> Reduce(List<int> sequence)
    {
        List<int> reduction = [];
        for (int i = 0; i < sequence.Count - 1; ++i)
        {
            reduction.Add(sequence[i+1] - sequence[i]);
        }
        return reduction;
    }
}
