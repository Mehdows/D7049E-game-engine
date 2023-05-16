using System;
using System.Collections.Generic;
using ArenaGame.Core.AI.BehaviorTrees;
using ArenaGame.Ecs.Archetypes;
using ArenaGame.Ecs.Components;
using Microsoft.Xna.Framework;

namespace ArenaGame.Ecs.Systems;

public class AISystem: ISystem
{
    private List<Entity> enemyEntities = new List<Entity>();

    public AISystem()
    {
        
        PlayerArchetype playerArchetype = (PlayerArchetype)ArchetypeFactory.GetArchetype(EArchetype.Player);
        Entity player = EntityManager.Instance.GetEntitiesWithArchetype(playerArchetype)[0];
        var enemies = ComponentManager.Instance.GetComponentArray(typeof(AIControllerComponent)).GetEntityComponents();
        foreach (var (entityID, component) in enemies)
        {
            AIControllerComponent aiControllerComponent = (AIControllerComponent) component;
            Entity entity = EntityManager.Instance.GetEntity(entityID);
            enemyEntities.Add(entity);
            switch (aiControllerComponent.EnemyType)
            {
                case EnemyType.Basic:
                    aiControllerComponent.BehaviorTree = new BasicEnemyBehaviorTree(entity, player, 25f );
                    break;
                case EnemyType.Fast:
                    // aiControllerComponent.BehaviorTree = new FastEnemyBehaviorTree();
                    break;
                case EnemyType.Heavy:
                    // aiControllerComponent.BehaviorTree = new HeavyEnemyBehaviorTree();
                    break;
                case EnemyType.Boss:
                    // aiControllerComponent.BehaviorTree = new BossEnemyBehaviorTree();
                    break;
            }
            
        }
    }
    
    public void Update(GameTime gameTime)
    {
        foreach (var enemy in enemyEntities)
        {
            AIControllerComponent aiControllerComponent =(AIControllerComponent) enemy.GetComponent<AIControllerComponent>();

            if (aiControllerComponent != null)
            {
                // Print to see if the enemy is moving
                aiControllerComponent.BehaviorTree.Update(gameTime);
            }
            
        }
    }
}