using System;
using ArenaGame.Ecs.Archetypes;
using ArenaGame.Ecs.Components;
using Microsoft.Xna.Framework;
using MathHelper = BEPUutilities.MathHelper;
using Quaternion = BEPUutilities.Quaternion;
using Vector3 = BEPUutilities.Vector3;

namespace ArenaGame.Ecs.Systems;

// For 3D, see PlayerControllerSystem for 2D
public class InputSystem: ISystem
{
    float speed = 10;
    float maxSpeed = 600;
    float brakeSpeed = 0.05f;

    private Vector3 movementDirection;
    
    public void Update(GameTime gameTime)
    {
        PlayerArchetype playerArchetype = (PlayerArchetype)ArchetypeFactory.GetArchetype(EArchetype.Player);
        Entity player3D = EntityManager.Instance.GetEntitiesWithArchetype(playerArchetype)[0];
        TransformComponent transform = (TransformComponent)player3D.GetComponent<TransformComponent>();
        InputComponent input = (InputComponent)player3D.GetComponent<InputComponent>();
        var collision = (CollisionComponent)player3D.GetComponent<CollisionComponent>();

        input.Update(gameTime);
        
        // Reset the movement direction
        movementDirection = Vector3.Zero;

        if (input.IsKeyHeld(InputKey.Up))
        {
            if (input.IsKeyHeld(InputKey.Left))
            {
                movementDirection = new Vector3(1, 0, 1);
                movementDirection.Normalize();
            }
            else if (input.IsKeyHeld(InputKey.Right))
            {
                movementDirection = new Vector3(-1, 0, 1);
                movementDirection.Normalize();
            }
            else
            {
                movementDirection = new Vector3(0, 0, 1);
            }
        }
        else if (input.IsKeyHeld(InputKey.Down))
        {
            if (input.IsKeyHeld(InputKey.Left))
            {
                movementDirection = new Vector3(1, 0, -1);
                movementDirection.Normalize();
            }
            else if (input.IsKeyHeld(InputKey.Right))
            {
                movementDirection = new Vector3(-1, 0, -1);
                movementDirection.Normalize();
            }
            else
            {
                movementDirection = new Vector3(0, 0, -1);
            }
        }
        else if (input.IsKeyHeld(InputKey.Left))
        {
            movementDirection = new Vector3(1, 0, 0);
        }
        else if (input.IsKeyHeld(InputKey.Right))
        {
            movementDirection = new Vector3(-1, 0, 0);
        }
        else
        {
            movementDirection = Vector3.Zero;
        }
        
        // If no keys are pressed, stop the player
        if (!input.IsKeyHeld(InputKey.Left) && !input.IsKeyHeld(InputKey.Right) && !input.IsKeyHeld(InputKey.Up) && !input.IsKeyHeld(InputKey.Down))
        {
            // Lerp the linear velocity to zero
            if (collision.CollisionEntity.LinearVelocity.LengthSquared() < 0.1f)
            {
                collision.CollisionEntity.LinearVelocity = new Vector3(0, collision.CollisionEntity.LinearVelocity.Y, 0);
            }
            else
                collision.CollisionEntity.LinearVelocity = Vector3.Lerp(collision.CollisionEntity.LinearVelocity, new Vector3(0, collision.CollisionEntity.LinearVelocity.Y, 0), 0.1f);
            
        }
        
        transform.WorldTransform = collision.CollisionEntity.WorldTransform;
        Vector3 newVelocity = Accelerate(movementDirection, speed);
        collision.CollisionEntity.LinearVelocity = new Vector3(newVelocity.X, collision.CollisionEntity.LinearVelocity.Y, newVelocity.Z);
        transform.WorldTransform = collision.CollisionEntity.WorldTransform;
        transform.Orientation = Rotate(transform, movementDirection);
    }
    
    private Vector3 Accelerate(Vector3 direction, float speed)
    {
        PlayerArchetype playerArchetype = (PlayerArchetype)ArchetypeFactory.GetArchetype(EArchetype.Player);
        Entity player = EntityManager.Instance.GetEntitiesWithArchetype(playerArchetype)[0];
        // Get the current velocity
        Vector3 velocity = ((CollisionComponent)player.GetComponent<CollisionComponent>()).CollisionEntity.LinearVelocity;
        
        // Calculate the new velocity
        Vector3 newVelocity = velocity + (direction * speed); 
        
        if (newVelocity.LengthSquared() > maxSpeed * maxSpeed)
        {
            newVelocity.Normalize();
            newVelocity *= maxSpeed;
        }
        
        // Check if the player is running in the opposite direction and apply a braking force
        if (Vector3.Dot(direction, velocity) < 0 && Math.Abs(velocity.Length()) > 0.1f)
        {
            Vector3 brakingForce = velocity * brakeSpeed;
            Console.Out.WriteLine($"Braking force {brakingForce} ");
            newVelocity -= brakingForce;
        }
        
        // Set the new velocity
        return newVelocity;
    }

    private Quaternion Rotate(TransformComponent transform, Vector3 movementDirection)
    {
        float angle = (float)Math.Atan2(movementDirection.X, movementDirection.Z);
        angle = MathHelper.ToDegrees(angle);
        
        // Round the angle to the nearest 45 degrees
        angle = (float)(Math.Round(angle / 45) * 45);
    
        // Convert the angle to a quaternion and set the player's rotation
        return Quaternion.CreateFromAxisAngle(transform.Up, MathHelper.ToRadians(angle));
    }
}