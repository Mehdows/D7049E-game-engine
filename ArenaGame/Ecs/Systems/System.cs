using System.Collections.Generic;
using ArenaGame.Ecs.Components;
using Microsoft.Xna.Framework;

namespace ArenaGame.Ecs.Systems;

public abstract class System
{
    protected List<Entity> entities = new List<Entity>();

    public abstract void AddEntity(Entity entity);

    public abstract void Update(GameTime gameTime);

}