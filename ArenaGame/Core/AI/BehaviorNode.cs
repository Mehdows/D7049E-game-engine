using Microsoft.Xna.Framework;

namespace ArenaGame.Core.AI;

public abstract class BehaviorNode
{
    public abstract NodeStatus Execute(GameTime gameTime);
}