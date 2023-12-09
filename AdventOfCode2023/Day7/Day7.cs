class Day7
{
    public async Task Run()
    {
        var input = await File.ReadAllLinesAsync($"{GetType()}/input.txt");

        Console.WriteLine("Running Day 7 - Part 1");

        var hands = input.Select(Hand.ParseHand).OrderBy(c => c.Strength).ToList();

        Console.WriteLine($"Total winnings = {hands.Select((h, i) => h.Bid * (i + 1)).Sum()}");

        Console.WriteLine("Running Day 7 - Part 2");

        hands = hands.OrderBy(h => h.JokerStrength).ToList();

        Console.WriteLine($"Total winnings = {hands.Select((h, i) => h.Bid * (i + 1)).Sum()}");
    }

    enum HandType
    {
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind
    }

    class Hand
    {
        public readonly string Cards;
        public readonly int Bid;
        public readonly int Strength;
        public readonly int JokerStrength;

        readonly HandType HandType;
        readonly HandType JokerHandType;

        public static Hand ParseHand(string line)
        {
            var split = line.Split(" ");
            return new Hand(split[0], int.Parse(split[1]));
        }

        Hand(string cards, int bid)
        {
            Cards = cards;
            Bid = bid;

            int[] labelCounts = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];

            foreach (var c in Cards)
            {
                var idx = "23456789TJQKA".IndexOf(c);
                labelCounts[idx]++;
                Strength = (Strength << 4) | idx;
                idx = "J23456789TQKA".IndexOf(c);
                JokerStrength = (JokerStrength << 4) | idx;
            }
            
            int[] labelCountsSorted = (int[])labelCounts.Clone();
            Array.Sort(labelCountsSorted);
            Array.Reverse(labelCountsSorted);

            HandType = DetermineHandType(labelCountsSorted);
            Strength = (Convert.ToInt32(HandType) << 20) | Strength;

            var jokers = labelCounts[9];
            labelCounts[9] = 0;
            labelCountsSorted = (int[])labelCounts.Clone();

            Array.Sort(labelCountsSorted);
            Array.Reverse(labelCountsSorted);
            labelCountsSorted[0] += jokers;

            JokerHandType = DetermineHandType(labelCountsSorted);
            JokerStrength = (Convert.ToInt32(JokerHandType) << 20) | JokerStrength;
        }

        private static HandType DetermineHandType(int[] labelCounts)
        {
            return labelCounts switch
            {
                [5, ..] => HandType.FiveOfAKind,
                [4, ..] => HandType.FourOfAKind,
                [3, 2, ..] => HandType.FullHouse,
                [3, 1, 1, ..] => HandType.ThreeOfAKind,
                [2, 2, 1, ..] => HandType.TwoPair,
                [2, 1, 1, 1, ..] => HandType.OnePair,
                _ => HandType.HighCard
            };
        }
    }
}
