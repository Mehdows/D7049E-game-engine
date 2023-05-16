using System;
using ArenaGame.Ecs.Components;
using BEPUphysics.CollisionShapes.ConvexShapes;
using Microsoft.Xna.Framework;
using Vector3 = BEPUutilities.Vector3;

namespace ArenaGame.Ecs.Systems
{
    public class SpawnerSystem : ISystem
    {
        private Vector3 spawnPositionTopLeft = new(250f, 30f, 250f);
        private Vector3 spawnPositionTopRight = new(-250f, 30f, 250f);
        private Vector3 spawnPositionBottomLeft = new(250f, 30f, -250f);
        private Vector3 spawnPositionBottomRight = new(-250f, 30f, -250f);
        
        private AISystem aiSystem;
        private EntityBuilder builder;
        private float elapsedTime = 0f;

        public SpawnerSystem(AISystem aiSystem)
        {
            this.aiSystem = aiSystem;

            // TODO: Want to spawn every 10 seconds but it doesn't work since we can't load content in Update and don't know how to save model for reuse
            builder = new EntityBuilder()
                .AddTransformComponent()
                .AddMeshComponent("Models/enemy_character", new BEPUutilities.Vector3(0, -3.5f, 0))
                .AddCollisionComponent(spawnPositionTopLeft, new CapsuleShape(10f, 5f), new BEPUutilities.Vector3(20, 50, 20), "Enemy", -15f)
                .AddAIControllerComponent(EnemyType.Basic);
            this.aiSystem.AddEnemy(builder.Build());

            builder = new EntityBuilder()
                .AddTransformComponent()
                .AddMeshComponent("Models/enemy_character", new BEPUutilities.Vector3(0, -3.5f, 0))
                .AddCollisionComponent(spawnPositionTopRight, new CapsuleShape(10f, 5f), new BEPUutilities.Vector3(20, 50, 20), "Enemy", -15f)
                .AddAIControllerComponent(EnemyType.Basic);
            this.aiSystem.AddEnemy(builder.Build());

            builder = new EntityBuilder()
                .AddTransformComponent()
                .AddMeshComponent("Models/enemy_character", new BEPUutilities.Vector3(0, -3.5f, 0))
                .AddCollisionComponent(spawnPositionBottomLeft, new CapsuleShape(10f, 5f), new BEPUutilities.Vector3(20, 50, 20), "Enemy", -15f)
                .AddAIControllerComponent(EnemyType.Basic);
            this.aiSystem.AddEnemy(builder.Build());

            builder = new EntityBuilder()
                .AddTransformComponent()
                .AddMeshComponent("Models/enemy_character", new BEPUutilities.Vector3(0, -3.5f, 0))
                .AddCollisionComponent(spawnPositionBottomRight, new CapsuleShape(10f, 5f), new BEPUutilities.Vector3(20, 50, 20), "Enemy", -15f)
                .AddAIControllerComponent(EnemyType.Basic);
            this.aiSystem.AddEnemy(builder.Build());
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            elapsedTime += deltaTime;

            // Entered every 10 seconds
            if (elapsedTime >= 10f)
            {
                elapsedTime = 0f;

                // TODO: Actually want to spawn enemies here (can pick a random spawner after every time interval)
            }
        }
    }
}
