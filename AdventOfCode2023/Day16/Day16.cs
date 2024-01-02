class Day16
{
    public async Task Run()
    {
        var input = await File.ReadAllLinesAsync($"{GetType()}/input.txt");

        Console.WriteLine("Running Day 16 - Part 1");

        int energizedTiles = ShineBeam(input, new() { X = 0, Y = -1, Dir = Right });

        Console.WriteLine($"Energized tiles = {energizedTiles}");

        Console.WriteLine("Running Day 16 - Part 2");

        List<Beam> entryBeams = [];
        for (int x = 0; x < input.Length; ++x)
        {
            entryBeams.Add(new() { X = x, Y = -1, Dir = Right });
            entryBeams.Add(new() { X = x, Y = input[0].Length, Dir = Left });
        }
        for (int y = 0; y < input[0].Length; ++y)
        {
            entryBeams.Add(new() { X = -1, Y = y, Dir = Down });
            entryBeams.Add(new() { X = input.Length, Y = y, Dir = Up });
        }

        energizedTiles = entryBeams.Select(b => ShineBeam(input, b)).Max();

        Console.WriteLine($"Max energized tiles = {energizedTiles}");
    }

    private static int ShineBeam(string[] input, Beam start)
    {
        HashSet<(int, int)> energizedTiles = [];
        HashSet<(int, int, int)> seenBeams = [];

        List<Beam> beams = [start];
        while (beams.Count > 0)
        {
            List<Beam> newBeams = [];
            List<Beam> deadBeams = [];

            foreach (var beam in beams)
            {
                (beam.X, beam.Y) = beam.Dir switch
                {
                    Up    => (beam.X - 1, beam.Y),
                    Down  => (beam.X + 1, beam.Y),
                    Left  => (beam.X, beam.Y - 1),
                    Right => (beam.X, beam.Y + 1),
                    _ => (beam.X, beam.Y)
                };

                if (beam.X < 0 || input.Length <= beam.X || beam.Y < 0 || input[0].Length <= beam.Y)
                {
                    deadBeams.Add(beam);
                    continue;
                }

                if (seenBeams.Contains((beam.X, beam.Y, beam.Dir)))
                {
                    deadBeams.Add(beam);
                    continue;
                }

                seenBeams.Add((beam.X, beam.Y, beam.Dir));
                energizedTiles.Add((beam.X, beam.Y));

                char space = input[beam.X][beam.Y];
                beam.Dir = Reflect(space, beam.Dir);
                var splitBeams = Split(space, beam);
                if (splitBeams.Count > 0)
                {
                    newBeams.AddRange(splitBeams);
                    deadBeams.Add(beam);
                }
            }

            beams.AddRange(newBeams);
            deadBeams.ForEach(b => beams.Remove(b));
        }

        return energizedTiles.Count;
    }

    class Beam
    {
        public int Id;
        public int X;
        public int Y;
        public int Dir;
    }

    static int Reflect(char space, int inDir) =>
        (space, inDir) switch
        {
            ('/',  Up)    => Right,
            ('/',  Down)  => Left,
            ('/',  Left)  => Down,
            ('/',  Right) => Up,
            ('\\', Up)    => Left,
            ('\\', Down)  => Right,
            ('\\', Left)  => Up,
            ('\\', Right) => Down,
            _ => inDir
        };

    static List<Beam> Split(char space, Beam beam) =>
        (space, beam.Dir) switch
        {
            ('|', Left or Right) => [
                new Beam { X = beam.X, Y = beam.Y, Dir = Up },
                new Beam { X = beam.X, Y = beam.Y, Dir = Down } ],
            ('-', Up or Down) => [
                new Beam { X = beam.X, Y = beam.Y, Dir = Left },
                new Beam { X = beam.X, Y = beam.Y, Dir = Right } ],
            (_, _) => []
        };

    const int Up = 0;
    const int Down = 1;
    const int Left = 2;
    const int Right = 3;
}
