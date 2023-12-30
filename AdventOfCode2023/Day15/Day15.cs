class Day15
{
    public async Task Run()
    {
        var input = (await File.ReadAllLinesAsync($"{GetType()}/input.txt"))[0].Split(',');

        Console.WriteLine("Running Day 15 - Part 1");

        Console.WriteLine($"Hash sums = {input.Select(RunHash).Sum()}");

        Console.WriteLine("Running Day 15 - Part 2");
    }

    int RunHash(string str) =>
        str.Aggregate(0, (h, c) => (h + c) * 17 % 256);
}
