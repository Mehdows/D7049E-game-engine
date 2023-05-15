using System;
using Microsoft.Xna.Framework;

namespace ArenaGame.Ecs.Systems
{
    public class SpawnerSystem : ISystem
    {
        public void Update(GameTime gameTime)
        {
            // TODO: Get all spawners and spawn enemies at their transform location, if the time is right. Another solution is to let the SpawnerSystem randomly select a spawner after a random amount of time.
        }
    }
}
