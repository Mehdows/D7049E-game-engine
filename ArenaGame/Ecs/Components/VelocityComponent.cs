using Microsoft.Xna.Framework;

namespace ArenaGame.Ecs.Components;

public class VelocityComponent: IComponent
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

    public VelocityComponent(float x = 0, float y = 0) 
    {
        Velocity = new Vector2(x, y);
    }

    public VelocityComponent() 
    {
        Velocity = new Vector2(0, 0);
    }

}