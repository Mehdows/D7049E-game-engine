using System.Collections.Generic;
using ArenaGame.Ecs.Components;
using Microsoft.Xna.Framework;

namespace ArenaGame.Ecs.Systems;

public abstract class System
{


    public abstract void Update(EntityManager entityManager, GameTime gameTime);

}