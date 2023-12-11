class Day9
{
    public async Task Run()
    {
        var input = await File.ReadAllLinesAsync($"{GetType()}/input.txt");

        Console.WriteLine("Running Day 9 - Part 1");

        var extrapolations = input.Select(l => l.Split(' ').Select(int.Parse).ToList()).Select(Extrapolate).ToList();

        Console.WriteLine($"Sum of extrapolated values = {extrapolations.Select(e => e.Item2).Sum()}");

        Console.WriteLine("Running Day 9 - Part 2");

        Console.WriteLine($"Sum of extrapolated values = {extrapolations.Select(e => e.Item1).Sum()}");
    }

    public (int, int) Extrapolate(List<int> sequence)
    {
        List<List<int>> histories = [sequence];
        while (sequence.Any(v => v != 0))
        {
            sequence = Reduce(sequence);
            histories.Add(sequence);
        }

        histories.Reverse();

        (int exB, int exF) = (0, 0);
        foreach (var history in histories)
        {
            (exB, exF) = (history.First() - exB, history.Last() + exF);
        }
        return (exB, exF);
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
