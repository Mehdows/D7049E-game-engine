using ArenaGame.Ecs.Archetypes;
using ArenaGame.Ecs.Components;
using Microsoft.Xna.Framework;

namespace ArenaGame.Ecs.Systems;

// For 2D, see InputSystem for 3D
public class PlayerControllerSystem: ISystem
{
    private int speed = 10;

    public void Update(GameTime gameTime)
    {
        PlayerArchetype playerArchetype = (PlayerArchetype)ArchetypeFactory.GetArchetype(EArchetype.Player);
        Entity player = EntityManager.Instance.GetEntitiesWithArchetype(playerArchetype)[0];
        InputComponent input = (InputComponent)player.GetComponent<InputComponent>();
        PositionComponent position = (PositionComponent)player.GetComponent<PositionComponent>();
        
        input.Update(gameTime);

        if (input.IsKeyHeld(InputKey.Up))
        {
            position.Y -= 1 * speed;
        }
        
        if(input.IsKeyHeld(InputKey.Down))
        {
            position.Y += 1 * speed;    
        }
        
        if(input.IsKeyHeld(InputKey.Left))
        {
            position.X -= 1 * speed;
        }
        
        if(input.IsKeyHeld(InputKey.Right))
        {
            position.X += 1 * speed;
        }
        
        
    }
}