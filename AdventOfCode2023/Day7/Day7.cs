class Day7
{
    public async Task Run()
    {
        var input = await File.ReadAllLinesAsync($"{GetType()}/input.txt");

        Console.WriteLine("Running Day 7 - Part 1");

        var hands = input.Select(Hand.ParseHand).OrderBy(c => c.Strength).ToList();

        Console.WriteLine($"Total winnings = {hands.Select((h, i) => h.Bid * (i + 1)).Sum()}");

        Console.WriteLine("Running Day 7 - Part 2");
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
        readonly int[] LabelCounts = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
        public int Strength;

        HandType HandType;

        int[] LabelCountsSorted
        {
            get
            {
                if (_labelCountsSorted == null)
                {
                    _labelCountsSorted = (int[])LabelCounts.Clone();
                    Array.Sort(_labelCountsSorted);
                    Array.Reverse(_labelCountsSorted);
                }
                return _labelCountsSorted;
            }
        }
        private int[]? _labelCountsSorted;

        public static Hand ParseHand(string line)
        {
            var split = line.Split(" ");
            var hand = new Hand(split[0], int.Parse(split[1]));

            foreach (var c in hand.Cards)
            {
                var idx = char.IsDigit(c) ? c - '2' : 8 + "TJQKA".IndexOf(c);
                hand.LabelCounts[idx]++;
                hand.Strength = (hand.Strength << 4) | idx;
            }

            hand.HandType = hand.LabelCountsSorted switch
            {
                [5, ..] => HandType.FiveOfAKind,
                [4, ..] => HandType.FourOfAKind,
                [3, 2, ..] => HandType.FullHouse,
                [3, 1, 1, ..] => HandType.ThreeOfAKind,
                [2, 2, 1, ..] => HandType.TwoPair,
                [2, 1, 1, 1, ..] => HandType.OnePair,
                _ => HandType.HighCard
            };

            hand.Strength = (Convert.ToInt32(hand.HandType) << 20) | hand.Strength;

            return hand;
        }

        Hand(string cards, int bid)
        {
            Cards = cards;
            Bid = bid;
        }
    }
}
