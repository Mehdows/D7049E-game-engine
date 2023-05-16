using System;
using ArenaGame.Ecs.Components;
using BEPUphysics;
using BEPUphysics.CollisionShapes.ConvexShapes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector3 = BEPUutilities.Vector3;

namespace ArenaGame.Ecs.Systems
{
    public class SpawnerSystem : ISystem
    {
        private Vector3 spawnPositionTopLeft = new(250f, 30f, 250f);
        private Vector3 spawnPositionTopRight = new(-250f, 30f, 250f);
        private Vector3 spawnPositionBottomLeft = new(250f, 30f, -250f);
        private Vector3 spawnPositionBottomRight = new(-250f, 30f, -250f);
        
        private Space gameSpace;
        private AISystem aiSystem;
        private EntityBuilder builder;
        private Model model;
        private float elapsedTime = 0f;
        
        private static Random random = new Random();
        private const float MinSpawnInterval = 5f;
        private const float MaxSpawnInterval = 15f;
        private float timeUntilNextSpawn = GetRandomSpawnInterval();




        public SpawnerSystem(Space gameSpace, AISystem aiSystem, Model model)
        {
            this.gameSpace = gameSpace;
            this.model = model;
            this.aiSystem = aiSystem;

            // TODO: Want to spawn every 10 seconds but it doesn't work since we can't load content in Update and don't know how to save model for reuse
            builder = new EntityBuilder()
                .AddTransformComponent()
                .AddMeshComponent(this.model, new Vector3(0, -3.5f, 0))
                .AddCollisionComponent(spawnPositionTopLeft, new CapsuleShape(10f, 5f), new BEPUutilities.Vector3(20, 50, 20), "Enemy", -15f)
                .AddAIControllerComponent(EnemyType.Basic);
            this.aiSystem.AddEnemy(builder.Build());

            builder = new EntityBuilder()
                .AddTransformComponent()
                .AddMeshComponent(this.model, new BEPUutilities.Vector3(0, -3.5f, 0))
                .AddCollisionComponent(spawnPositionTopRight, new CapsuleShape(10f, 5f), new BEPUutilities.Vector3(20, 50, 20), "Enemy", -15f)
                .AddAIControllerComponent(EnemyType.Basic);
            this.aiSystem.AddEnemy(builder.Build());

            builder = new EntityBuilder()
                .AddTransformComponent()
                .AddMeshComponent(this.model, new BEPUutilities.Vector3(0, -3.5f, 0))
                .AddCollisionComponent(spawnPositionBottomLeft, new CapsuleShape(10f, 5f), new BEPUutilities.Vector3(20, 50, 20), "Enemy", -15f)
                .AddAIControllerComponent(EnemyType.Basic);
            this.aiSystem.AddEnemy(builder.Build());

            builder = new EntityBuilder()
                .AddTransformComponent()
                .AddMeshComponent(this.model, new BEPUutilities.Vector3(0, -3.5f, 0))
                .AddCollisionComponent(spawnPositionBottomRight, new CapsuleShape(10f, 5f), new BEPUutilities.Vector3(20, 50, 20), "Enemy", -15f)
                .AddAIControllerComponent(EnemyType.Basic);
            this.aiSystem.AddEnemy(builder.Build());
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            elapsedTime += deltaTime;

            timeUntilNextSpawn -= deltaTime;
            if (timeUntilNextSpawn <= 0f)
            {
                // Spawn the AI entity
                builder = new EntityBuilder()
                    .AddTransformComponent()
                    .AddMeshComponent(this.model, new Vector3(0, -3.5f, 0))
                    .AddCollisionComponent(spawnPositionBottomRight, new CapsuleShape(10f, 5f), new Vector3(20, 50, 20), "Enemy", -15f)
                    .AddAIControllerComponent(EnemyType.Basic);
                var newEnemy = builder.Build();
                this.aiSystem.AddEnemy(newEnemy);
                // Dont forget to add to space for the collision -> movement to work
                gameSpace.Add(((CollisionComponent)newEnemy.GetComponent<CollisionComponent>()).CollisionEntity);
                
                
                builder = new EntityBuilder()
                    .AddTransformComponent()
                    .AddMeshComponent(this.model, new Vector3(0, -3.5f, 0))
                    .AddCollisionComponent(spawnPositionBottomLeft, new CapsuleShape(10f, 5f), new Vector3(20, 50, 20), "Enemy", -15f)
                    .AddAIControllerComponent(EnemyType.Basic);
                newEnemy = builder.Build();
                this.aiSystem.AddEnemy(newEnemy);
                gameSpace.Add(((CollisionComponent)newEnemy.GetComponent<CollisionComponent>()).CollisionEntity);
                
                builder = new EntityBuilder()
                    .AddTransformComponent()
                    .AddMeshComponent(this.model, new Vector3(0, -3.5f, 0))
                    .AddCollisionComponent(spawnPositionTopLeft, new CapsuleShape(10f, 5f), new Vector3(20, 50, 20), "Enemy", -15f)
                    .AddAIControllerComponent(EnemyType.Basic);
                newEnemy = builder.Build();
                this.aiSystem.AddEnemy(newEnemy);
                gameSpace.Add(((CollisionComponent)newEnemy.GetComponent<CollisionComponent>()).CollisionEntity);
                
                
                builder = new EntityBuilder()
                    .AddTransformComponent()
                    .AddMeshComponent(this.model, new Vector3(0, -3.5f, 0))
                    .AddCollisionComponent(spawnPositionTopRight, new CapsuleShape(10f, 5f), new Vector3(20, 50, 20), "Enemy", -15f)
                    .AddAIControllerComponent(EnemyType.Basic);
                newEnemy = builder.Build();
                this.aiSystem.AddEnemy(newEnemy);
                
                 
                
                gameSpace.Add(((CollisionComponent)newEnemy.GetComponent<CollisionComponent>()).CollisionEntity);

                // Reset the time until the next spawn
                timeUntilNextSpawn = GetRandomSpawnInterval();
            }
        }

        private static float GetRandomSpawnInterval()
        {
            return (float)random.NextDouble() * (MaxSpawnInterval - MinSpawnInterval) + MinSpawnInterval;
        }
    }
    
}