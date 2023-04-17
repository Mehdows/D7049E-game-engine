using Microsoft.Xna.Framework;

namespace ArenaGame.Ecs.Components;

public class VelocityComponent
{
    
    public Vector2 Velocity { get; set; }

    public float X
    {
        get { return Velocity.X; }
        set { Velocity = new Vector2(value, Velocity.Y); }
    }

    public float Y
    {
        get { return Velocity.Y; }
        set { Velocity = new Vector2(Velocity.X, value); }
    }

    public VelocityComponent(float x, float y)
    {
        Velocity = new Vector2(x, y);
    }
}