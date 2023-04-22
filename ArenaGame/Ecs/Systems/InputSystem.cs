using ArenaGame.Ecs.Archetypes;
using ArenaGame.Ecs.Components;
using Microsoft.Xna.Framework;

namespace ArenaGame.Ecs.Systems;

// For 3D, see PlayerControllerSystem for 2D
public class InputSystem: ISystem
{
    public void Update(GameTime gameTime)
    {
        Player3DArchetype player3DArchetype = (Player3DArchetype)ArchetypeFactory.GetArchetype(EArchetype.Player3D);
        Entity player3D = EntityManager.Instance.GetEntitiesWithArchetype(player3DArchetype)[0];
        TransformComponent transform = (TransformComponent)player3D.GetComponent<TransformComponent>();
        InputComponent input = (InputComponent)player3D.GetComponent<InputComponent>();

        input.Update(gameTime);

        if (input.IsKeyHeld(InputKey.Left))
        {
            transform.Position += new Vector3(5f, 0f, 0f);
            transform.Rotation = Quaternion.CreateFromAxisAngle(transform.Forward, MathHelper.ToRadians(90));
        }
        if (input.IsKeyHeld(InputKey.Right))
        {
            transform.Position += new Vector3(-5f, 0f, 0f);
            transform.Rotation = Quaternion.CreateFromAxisAngle(transform.Forward, MathHelper.ToRadians(-90));
        }
        if (input.IsKeyHeld(InputKey.Up))
        {
            transform.Position += new Vector3(0f, 5f, 0f);
            transform.Rotation = Quaternion.CreateFromAxisAngle(transform.Forward, MathHelper.ToRadians(0));
        }
        if (input.IsKeyHeld(InputKey.Down))
        {
            transform.Position += new Vector3(0f, -5f, 0f);
            transform.Rotation = Quaternion.CreateFromAxisAngle(transform.Forward, MathHelper.ToRadians(180));
        }
    }
}