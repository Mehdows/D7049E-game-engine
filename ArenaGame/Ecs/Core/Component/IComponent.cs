using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ArenaGame.Ecs.Components;

public abstract class IComponent : GameComponent
{

    public virtual void Update(GameTime gameTime)
    {
        
    }

    public void LoadContent(ContentManager contentManager)
    {
    }

    public virtual void Draw(GameTime gameTime)
    {
    }

    protected IComponent() : base(Game1.Instance)
    {
    }
}