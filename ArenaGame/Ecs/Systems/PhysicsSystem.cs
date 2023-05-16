using System;
using ArenaGame.Ecs.Components;
using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using Microsoft.Xna.Framework;

namespace ArenaGame.Ecs.Systems;

public class PhysicsSystem: ISystem
{
    Space Space {get; set;}
    Game Game {get; set;}
    
    public PhysicsSystem(Space space, Game game)
    {
        this.Space = space;
        this.Game = game;
    }
    public void Update(GameTime gameTime)
    {
        var entities = ComponentManager.Instance.GetComponentArray(typeof(CollisionComponent)).GetEntityComponents();
        foreach (var ( _, component) in entities)
        {
            var collsiionComponent = (CollisionComponent)component;
            collsiionComponent.CollisionEntity.CollisionInformation.Events.InitialCollisionDetected += HandleCollision;
        }
    }

    void HandleCollision(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
    {

        if (other is EntityCollidable otherEntity )
        {
            if (sender.Entity.Tag is "Weapon")
            {
                // Console.WriteLine($"{sender.Entity}");
                if (otherEntity.Entity.Tag is string tag && tag == "Enemy")
                {
               
                    // Console.WriteLine($"{otherEntity.Entity}");
                    // Console.WriteLine($"ENEMY HIT");
                
                }
                
            }
            
            
        }
    }
}