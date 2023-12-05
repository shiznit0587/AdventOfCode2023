using System.Text.RegularExpressions;

partial class Day4
{
    public async Task Run()
    {
        var input = await File.ReadAllLinesAsync($"{GetType()}/input.txt");

        Console.WriteLine("Running Day 4 - Part 1");

        List<Card> cards = input
            .Select(line => CardRegex().Match(line))
            .Select(m => new Card()
            {
                Id = int.Parse(m.Groups[1].ValueSpan),
                WinningNumbers = SplitToIntList(m.Groups[2].Value),
                CardNumbers = SplitToIntList(m.Groups[3].Value)
            })
            .ToList();

        var totalPoints = cards
            .Select(c => c.MatchingNumberCount)
            .Where(w => w > 0)
            .Select(w => Math.Pow(2, w - 1))
            .Sum();

        Console.WriteLine($"Total points = {totalPoints}");

        Console.WriteLine("Running Day 4 - Part 2");

        foreach (var card in cards)
        {
            for (int i = 0; i < card.MatchingNumberCount; ++i)
            {
                cards[card.Id + i].Count += card.Count;
            }
        }

        Console.WriteLine($"Total scratchcard count = {cards.Select(c => c.Count).Sum()}");        
    }

    class Card
    {
        public required int Id;
        public required List<int> WinningNumbers;
        public required List<int> CardNumbers;

        public int MatchingNumberCount
        {
            get 
            {
                _matchingNumberCount ??= CardNumbers.Intersect(WinningNumbers).Count();
                return _matchingNumberCount.Value;
            }
        }
        private int? _matchingNumberCount;

        public int Count = 1;
    }

    private static List<int> SplitToIntList(string s)
    {
        return s.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse)
                    .ToList();
    }

    [GeneratedRegex("Card\\s+(\\d+):(.*)\\|(.*)", RegexOptions.Compiled)]
    private static partial Regex CardRegex();
}
