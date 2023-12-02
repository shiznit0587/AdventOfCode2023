using System.Text.RegularExpressions;

class Day2
{
    public async Task Run()
    {
        var input = await File.ReadAllLinesAsync($"{GetType()}/input.txt");

        Console.WriteLine("Running Day 2 - Part 1");

        var games = input.Select(ParseGame).ToList();

        int reds = 12, greens = 13, blues = 14;
        int totalCubes = reds + greens + blues;

        int sum = 0;
        foreach (var game in games)
        {
            foreach (var draw in game.Draws)
            {
                if (draw.Reds <= reds &&
                    draw.Greens <= greens &&
                    draw.Blues <= blues &&
                    draw.Reds + draw.Greens + draw.Blues <= totalCubes)
                {
                    sum += game.Id;
                }
            }
        }

        Console.WriteLine($"sum = {sum}");

        Console.WriteLine("Running Day 2 - Part 2");

        sum = 0;
        foreach (var game in games)
        {
            reds = game.Draws.Select(d => d.Reds).Max();
            greens = game.Draws.Select(d => d.Greens).Max();
            blues = game.Draws.Select(d => d.Blues).Max();

            sum += reds * greens * blues;
        }

        Console.WriteLine($"sum = {sum}");
    }

    private Game ParseGame(string line)
    {
        var game = new Game();

        var match = Regex.Match(line, "Game (\\d+): (.*)");

        game.Id = int.Parse(match.Groups[1].Value);

        var drawsRaw = match.Groups[2].Value.Split(";", StringSplitOptions.TrimEntries);

        foreach (string drawRaw in drawsRaw)
        {
            var blocksRaw = drawRaw.Split(",", StringSplitOptions.TrimEntries);
            var draw = new Draw();

            foreach (string blockRaw in blocksRaw)
            {
                var blocksMatch = Regex.Match(blockRaw, "(\\d+) (\\w+)");
                switch (blocksMatch.Groups[2].Value)
                {
                    case "red":
                        draw.Reds = int.Parse(blocksMatch.Groups[1].Value);
                        break;
                    case "green":
                        draw.Greens = int.Parse(blocksMatch.Groups[1].Value);
                        break;
                    case "blue":
                        draw.Blues = int.Parse(blocksMatch.Groups[1].Value);
                        break;
                }
            }

            game.Draws.Add(draw);
        }

        return game;
    }

    class Game
    {
        public int Id;
        public List<Draw> Draws = new List<Draw>();
    }

    class Draw
    {
        public int Reds;
        public int Greens;
        public int Blues;
    }
}
