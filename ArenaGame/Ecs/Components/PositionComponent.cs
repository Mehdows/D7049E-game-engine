using Microsoft.Xna.Framework;

namespace ArenaGame.Ecs.Components;

public class PositionComponent
{
    public Vector2 Position { get; set; }

    public float X
    {
        get { return Position.X; }
        set { Position = new Vector2(value, Position.Y); }
    }

    public float Y
    {
        get { return Position.Y; }
        set { Position = new Vector2(Position.X, value); }
    }

    public PositionComponent(float x, float y)
    {
        Position = new Vector2(x, y);
    }
}