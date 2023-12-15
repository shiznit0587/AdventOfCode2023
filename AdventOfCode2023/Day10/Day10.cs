class Day10
{
    public async Task Run()
    {
        var input = (await File.ReadAllLinesAsync($"{GetType()}/input.txt")).ToList();

        Console.WriteLine("Running Day 10 - Part 1");

        (int width, int height) = (input[0].Length, input.Count);
        Direction[,] pipes =  new Direction[width, height];
        for (int y = 0; y < height; ++y)
            for (int x = 0; x < width; ++x)
                pipes[x, y] = FromChar(input[y][x]);

        int sY = input.IndexOf(input.First(l => l.Contains('S')));
        int sX = input[sY].IndexOf('S');

        var startPos = (x: sX, y: sY);
        var startDir = Direction.None;
        if (sY - 1 >= 0 && pipes[sX, sY - 1].HasFlag(Direction.Down))
        {
            startPos = (sX, sY - 1);
            startDir = Direction.Down;
        }
        else if (sY + 1 < height && pipes[sX, sY + 1].HasFlag(Direction.Up))
        {
            startPos = (sX, sY + 1);
            startDir = Direction.Up;
        }
        else if (sX - 1 >= 0 && pipes[sX - 1, sY].HasFlag(Direction.Right))
        {
            startPos = (sX - 1, sY);
            startDir = Direction.Right;
        }

        var currPos = startPos;
        var usedDir = startDir;

        int pipeLength = 1;
        while (currPos != (sX, sY))
        {
            var nextDir = pipes[currPos.x, currPos.y] & ~usedDir;
            ++pipeLength;

            currPos = OffsetByDir(currPos, nextDir);
            usedDir = OppositeDir(nextDir);
        }

        Console.WriteLine($"Furthest pipe from start = {pipeLength / 2}");
        
        Console.WriteLine("Running Day 10 - Part 2");

        // Double the grid size, adding rows and columns so we can fill all the gaps during our scanning.
        Tile[,] tiles = new Tile[width * 2, height * 2];

        for (int y = 0; y < height * 2; ++y)
            for (int x = 0; x < width * 2; ++x)
                tiles[x, y] = new Tile();

        for (int y = 0; y < height; ++y)
            for (int x = 0; x < width; ++x)
                tiles[x * 2, y * 2].Direction = pipes[x,y];

        (sX, sY) = (sX * 2, sY * 2);
        tiles[sX, sY].State = TileState.Pipe;
        currPos = (startPos.x * 2, startPos.y * 2);
        usedDir = startDir;

        // Fill the gap caused by the doubling of the grid size.
        currPos = startDir switch
        {
            Direction.Down => (sX, sY - 1),
            Direction.Up => (sX, sY + 1),
            Direction.Right => (sX - 1, sY),
            _ => (sX + 1, sY)
        };

        // Walk the pipe, filling in the pipe spaces in the new columns and rows on the way as well.
        while (currPos != (sX, sY))
        {
            var nextTile = tiles[currPos.x, currPos.y];
            if (nextTile.Direction == Direction.None)
            {
                nextTile.State = TileState.Pipe;
                nextTile.Direction = (Direction.Horizontal & usedDir) != Direction.None ? Direction.Horizontal : Direction.Vertical;
            }
            else
            {
                nextTile.State = TileState.Pipe;

                var nextDir = nextTile.Direction & ~usedDir;
                currPos = OffsetByDir(currPos, nextDir);
                usedDir = OppositeDir(nextDir);
            }
        }

        // Init our scanning queue with every tile along the perimeter.
        Queue<(int, int)> visitQueue = new();

        for (int y = 0; y < height * 2; ++y)
        {
            visitQueue.Enqueue((0, y));
            visitQueue.Enqueue((width * 2 - 1, y));
        }

        for (int x = 1; x < width * 2 - 1; ++x)
        {
            visitQueue.Enqueue((x, 0));
            visitQueue.Enqueue((x, height * 2 - 1));
        }

        List<(int x, int y)> offsets = [(-1, 0), (1, 0), (0, -1), (0, 1)];
        
        // Scan for all tiles reachable from outside the pipe.
        while (visitQueue.TryDequeue(out currPos))
        {
            var tile = tiles[currPos.x, currPos.y];
            if (tile.State == TileState.Unknown)
            {
                tile.State = TileState.Outside;
                foreach (var offset in offsets)
                {
                    (var x, var y) = (currPos.x + offset.x, currPos.y + offset.y);
                    if (0 <= x && x < width * 2 && 0 <= y && y < height * 2 && tiles[x, y].State == TileState.Unknown)
                        visitQueue.Enqueue((x, y));
                }
            }
        }

        // All unmarked tiles are inside the pipe.
        for (int y = 0; y < height * 2; ++y)
            for (int x = 0; x < width * 2; ++x)
            {
                var tile = tiles[x, y];
                if (tile.State == TileState.Unknown)
                    tile.State = TileState.Inside;
            }

        int insideCount = 0;
        for (int y = 0; y < height; ++y)
            for (int x = 0; x < width; ++x)
                if (tiles[x * 2, y * 2].State == TileState.Inside)
                    ++insideCount;

        Console.WriteLine($"Enclosed tiles = {insideCount}");
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
        Right   = 8,
        Horizontal = Left | Right,
        Vertical = Up | Down
    }

    private static Direction OppositeDir(Direction dir)
    {
        return dir switch
        {
            Direction.Up    => Direction.Down,
            Direction.Down  => Direction.Up,
            Direction.Left  => Direction.Right,
            Direction.Right => Direction.Left,
            _ => dir
        };
    }

    private static (int, int) OffsetByDir((int x, int y) pos, Direction dir)
    {
        return dir switch
        {
            Direction.Up    => (pos.x, pos.y - 1),
            Direction.Down  => (pos.x, pos.y + 1),
            Direction.Left  => (pos.x - 1, pos.y),
            Direction.Right => (pos.x + 1, pos.y),
            _ => pos
        };
    }

    enum TileState
    {
        Unknown,
        Pipe,
        Outside,
        Inside
    }

    class Tile
    {
        public Direction Direction = Direction.None;
        public TileState State = TileState.Unknown;
    }
}
