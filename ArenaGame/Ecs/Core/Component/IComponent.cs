using Microsoft.Xna.Framework;

namespace ArenaGame.Ecs.Components;

public abstract class IComponent : GameComponent
{
    public virtual void Initialize()
    {
        
    }

    public virtual void Update(GameTime gameTime)
    {
        
    }

    public virtual void Draw(GameTime gameTime)
    {
        
    }
    protected IComponent() : base(Game1.Instance)
    {
    }
}