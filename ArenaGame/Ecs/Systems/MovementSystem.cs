using System;
using System.Collections.Generic;
using ArenaGame.Ecs.Archetypes;
using ArenaGame.Ecs.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ArenaGame.Ecs.Systems;

public class MovementSystem : ISystem
{
    /*
    public override void AddEntity(Entity entity) 
    {
        if (entity.HasComponent<PositionComponent>() && entity.HasComponent<VelocityComponent>()) 
        {
            entities.Add(entity);
        }
    }*/
    
    public void Update(GameTime gameTime) 
    {
        // TODO: Check entites that have archetype that has position and velocity components
        foreach (Entity entity in EntityManager.Instance.GetEntities()) 
        {
            if (entity.HasComponent<PositionComponent>() &&
                entity.HasComponent<VelocityComponent>())
            {
                if (entity.HasComponent<InputComponent>())
                {
                    InputComponent input = (InputComponent)entity.GetComponent<InputComponent>();

                    // Get the current position and velocity of the entity
                    PositionComponent position = (PositionComponent)entity.GetComponent<PositionComponent>();
                    VelocityComponent velocity = (VelocityComponent)entity.GetComponent<VelocityComponent>();

                    // Calculate the new position based on the velocity
                    Vector2 newPosition = position.Position + velocity.Velocity;

                    // Adjust the position based on input
                    if (input.IsKeyDown(InputKey.Up))
                    {
                        newPosition.Y -= 1;
                    }
                    else if (input.IsKeyDown(InputKey.Down))
                    {
                        newPosition.Y += 1;
                    }
                    if (input.IsKeyDown(InputKey.Left))
                    {
                        newPosition.X -= 1;
                    }
                    else if (input.IsKeyDown(InputKey.Right))
                    {
                        newPosition.X += 1;
                    }

                    // Update the position component with the new position
                    position.Position = newPosition; 
                }
                
            }
        }
    }
}