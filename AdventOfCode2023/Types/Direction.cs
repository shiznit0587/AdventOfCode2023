namespace Types;

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

static class DirectionExtensions
{
    public static Direction Opposite(this Direction dir)
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

    public static (int, int) OffsetPos(this Direction dir, (int x, int y) pos)
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
}
