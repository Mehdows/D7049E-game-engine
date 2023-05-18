using System;
using System.Collections.Generic;
using System.ComponentModel;
using ArenaGame.Core.AI.BehaviorTrees;
using ArenaGame.Ecs.Archetypes;
using ArenaGame.Ecs.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArenaGame.Ecs.Systems;

public class AISystem: ISystem
{
    public Action<Entity> OnEnemyKilled { get; set; }

    private List<Entity> enemyEntities = new List<Entity>();
    private Entity player;

    public AISystem()
    {
        
        PlayerArchetype playerArchetype = (PlayerArchetype)ArchetypeFactory.GetArchetype(EArchetype.Player);
        player = EntityManager.Instance.GetEntitiesWithArchetype(playerArchetype)[0];
        var enemies = ComponentManager.Instance.GetComponentArray(typeof(AIControllerComponent)).GetEntityComponents();
        foreach (var (entityID, component) in enemies)
        {
            AIControllerComponent aiControllerComponent = (AIControllerComponent) component;
            Entity entity = EntityManager.Instance.GetEntity(entityID);
            enemyEntities.Add(entity);
            TransformComponent transformComponent = (TransformComponent) entity.GetComponent<TransformComponent>();
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
        List<Entity> deadEnemies = new List<Entity>();
        foreach (var enemy in enemyEntities)
        {
            AIControllerComponent aiControllerComponent =(AIControllerComponent) enemy.GetComponent<AIControllerComponent>();
            TransformComponent transform = (TransformComponent) enemy.GetComponent<TransformComponent>();

            // Remove enemies if they fall outside map
            if (transform.position.Y < 0f)
            {
                deadEnemies.Add(enemy);
                continue;
            }

            if (aiControllerComponent != null)
            {
                // Print to see if the enemy is moving
                aiControllerComponent.BehaviorTree.Update(gameTime);
            }
            
        }

        foreach (var enemy in deadEnemies)
        {
            enemyEntities.Remove(enemy);
            OnEnemyKilled?.Invoke(enemy);
        }
    }

    public void AddEnemy(Entity enemy)
    {
        AIControllerComponent aiControllerComponent = (AIControllerComponent)enemy.GetComponent<AIControllerComponent>();
        enemyEntities.Add(enemy);
        
        switch (aiControllerComponent.EnemyType)
        {
            case EnemyType.Basic:
                aiControllerComponent.BehaviorTree = new BasicEnemyBehaviorTree(enemy, player, 25f);
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