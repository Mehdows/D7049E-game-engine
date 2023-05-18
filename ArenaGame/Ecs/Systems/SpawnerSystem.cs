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

        private static Random random = new Random();
        private const float MinSpawnInterval = 3f;
        private const float MaxSpawnInterval = 10f;
        private float timeUntilNextSpawn = GetRandomSpawnInterval();

        private bool benchmarkMode;
        private float spawnTime = 0f;
        private float totalElapsedTime = 0f;

        public SpawnerSystem(Space gameSpace, AISystem aiSystem, Model model, bool benchmarkMode)
        {
            this.gameSpace = gameSpace;
            this.model = model;
            this.aiSystem = aiSystem;
            this.benchmarkMode = benchmarkMode;
        }

        public void Update(GameTime gameTime)
        {
            if (benchmarkMode) { Benchmark(gameTime);}
            else { SpawnEnemies(gameTime); }
        }

        private void SpawnEnemies(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            timeUntilNextSpawn -= deltaTime;
            if (timeUntilNextSpawn <= 0f)
            {
                int randNext = random.Next(1, 4);
                Vector3 spawnPosition = GetNextSpawnPosition(randNext);

                // Spawn the AI entity
                builder = new EntityBuilder()
                    .AddTransformComponent()
                    .AddMeshComponent(model, new Vector3(0, -3.5f, 0))
                    .AddCollisionComponent(spawnPosition, new CapsuleShape(10f, 5f), new Vector3(20, 50, 20), "Enemy", -15f)
                    .AddAIControllerComponent(EnemyType.Basic);
                var newEnemy = builder.Build();
                aiSystem.AddEnemy(newEnemy);
                // Dont forget to add to space for the collision -> movement to work
                gameSpace.Add(((CollisionComponent)newEnemy.GetComponent<CollisionComponent>()).CollisionEntity);

                // Reset the time until the next spawn
                timeUntilNextSpawn = GetRandomSpawnInterval();
            }
        }

        private void Benchmark(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            spawnTime += deltaTime;
            totalElapsedTime += deltaTime;

            if (totalElapsedTime >= 30f)
            {
                Game1.Instance.ExitGame();
            }

            if (spawnTime >= 0.2f)
            {
                for (int i = 1; i <= 4; i++)
                {
                    Vector3 spawnPosition = GetNextSpawnPosition(i);

                    builder = new EntityBuilder()
                        .AddTransformComponent()
                        .AddMeshComponent(model, new Vector3(0, -3.5f, 0))
                        .AddCollisionComponent(spawnPosition, new CapsuleShape(10f, 5f), new Vector3(20, 50, 20), "Enemy", -15f)
                        .AddAIControllerComponent(EnemyType.Basic);
                    var newEnemy = builder.Build();
                    aiSystem.AddEnemy(newEnemy);
                    gameSpace.Add(((CollisionComponent)newEnemy.GetComponent<CollisionComponent>()).CollisionEntity);
                }

                spawnTime = 0f;
            }
        }

        private Vector3 GetNextSpawnPosition(int index)
        {
            Vector3 spawnPosition;
            switch (index)
            {
                case 1:
                    spawnPosition = spawnPositionTopLeft;
                    break;
                case 2:
                    spawnPosition = spawnPositionTopRight;
                    break;
                case 3:
                    spawnPosition = spawnPositionBottomLeft;
                    break;
                case 4:
                    spawnPosition = spawnPositionBottomRight;
                    break;
                default:
                    spawnPosition = spawnPositionTopLeft;
                    break;
            }
            return spawnPosition;
        }

        private static float GetRandomSpawnInterval()
        {
            return (float)random.NextDouble() * (MaxSpawnInterval - MinSpawnInterval) + MinSpawnInterval;
        }
    }
    
}