class Day10
{
    public async Task Run()
    {
        var input = (await File.ReadAllLinesAsync($"{GetType()}/input.txt")).ToList();

        Console.WriteLine("Running Day 10 - Part 1");

        Direction[,] pipes =  new Direction[input[0].Length, input.Count];
        for (int y = 0; y < input.Count; ++y)
        {
            for (int x = 0; x < input[0].Length; ++x)
            {
                pipes[x, y] = FromChar(input[y][x]);
            }
        }

        int sY = input.IndexOf(input.First(l => l.Contains('S')));
        int sX = input[sY].IndexOf('S');

        var currPos = (x: sX, y: sY);
        var usedDir = Direction.None;
        if (pipes[sX, sY - 1].HasFlag(Direction.Down))
        {
            currPos = (sX, sY - 1);
            usedDir = Direction.Down;
        }
        else if (pipes[sX, sY + 1].HasFlag(Direction.Up))
        {
            currPos = (sX, sY + 1);
            usedDir = Direction.Up;
        }
        else if (pipes[sX - 1, sY].HasFlag(Direction.Right))
        {
            currPos = (sX - 1, sY);
            usedDir = Direction.Right;
        }

        int pipeLength = 1;
        while (currPos != (sX, sY))
        {
            var nextDir = pipes[currPos.x, currPos.y] & ~usedDir;
            ++pipeLength;

            currPos = nextDir switch
            {
                Direction.Up    => (currPos.x, currPos.y - 1),
                Direction.Down  => (currPos.x, currPos.y + 1),
                Direction.Left  => (currPos.x - 1, currPos.y),
                Direction.Right => (currPos.x + 1, currPos.y),
                _ => currPos
            };

            usedDir = nextDir switch
            {
                Direction.Up    => Direction.Down,
                Direction.Down  => Direction.Up,
                Direction.Left  => Direction.Right,
                Direction.Right => Direction.Left,
                _ => nextDir
            };
        }

        Console.WriteLine($"Furthest pipe from start = {pipeLength / 2}");
        
        Console.WriteLine("Running Day 10 - Part 2");
    }

    private static Direction FromChar(char c)
    {
        return c switch
        {
            '|' => Direction.Up   | Direction.Down,
            '-' => Direction.Left | Direction.Right,
            'L' => Direction.Up   | Direction.Right,
            'J' => Direction.Up   | Direction.Left,
            '7' => Direction.Down | Direction.Left,
            'F' => Direction.Down | Direction.Right,
            _   => Direction.None
        };
    }

    [Flags]
    enum Direction
    {
        None    = 0,
        Up      = 1,
        Down    = 2,
        Left    = 4,
        Right   = 8
    }
}
