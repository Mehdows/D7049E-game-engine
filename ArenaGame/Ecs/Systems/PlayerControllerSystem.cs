using Microsoft.Xna.Framework;

namespace ArenaGame.Ecs.Systems;

public class PlayerControllerSystem: ISystem
{
    public void Update(GameTime gameTime)
    {
        var entityManager = EntityManager.Instance;
        var componentManager = ComponentManager.Instance;

        
    }
}