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
                
        }
    }
}