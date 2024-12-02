class Day17
{
    public async Task Run()
    {
        var input = await File.ReadAllLinesAsync($"{GetType()}/input.txt");

        Console.WriteLine("Running Day 17 - Part 1");

        // This is a weighted graph traversal, with some edges becoming invalid based on the previous path.

        // Weighted grapgh traversal is best with a DFS, so once a full path is found a path that's worse than it at any point can be immediately discarded.

        //

        Console.WriteLine("Running Day 17 - Part 2");
    }
}