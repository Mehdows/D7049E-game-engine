using System;
using System.Collections.Generic;
using ArenaGame.Ecs.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ArenaGame.Ecs.Systems;

public class MovementSystem : System
{
    public override void AddEntity(Entity entity) 
    {
        if (entity.HasComponent<PositionComponent>() && entity.HasComponent<VelocityComponent>()) 
        {
            entities.Add(entity);
        }
    }
    
    public override void Update(GameTime gameTime) 
    {
        foreach (Entity entity in entities) 
        {
            EntityTypeComponent entityType = entity.GetComponent<EntityTypeComponent>();
            if (entityType.type == EntityType.Player)
            {
                PositionComponent playerPosition = entity.GetComponent<PositionComponent>();
                VelocityComponent playerVelocity = entity.GetComponent<VelocityComponent>();


                InputComponent playerInput = entity.GetComponent<InputComponent>();
                playerVelocity.X = 0;
                playerVelocity.Y = 0;
                foreach (Keys key in playerInput.Bindings.Keys)
                {
                    if (Keyboard.GetState().IsKeyDown(key))
                    {
                        Vector2 direction = playerInput.Bindings[key];
                        playerVelocity.X += direction.X * 100;
                        playerVelocity.Y += direction.Y * 100;
                    }
                }

                // Update the player's position based on its velocity
                playerPosition.X += (int)(playerVelocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds);
                playerPosition.Y += (int)(playerVelocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
        }
    } 
}