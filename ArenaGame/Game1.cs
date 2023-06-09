﻿using System;
using System.Collections.Generic;
using ArenaGame.Ecs;
using ArenaGame.Ecs.Components;
using ArenaGame.Ecs.Systems;
using BEPUphysics;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUphysics.Entities.Prefabs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Color = Microsoft.Xna.Framework.Color;
using Matrix = BEPUutilities.Matrix;
using Vector3 = BEPUutilities.Vector3;

namespace ArenaGame;

public class Game1 : Game
{
    private static Game1 instance;

    public static Game1 Instance => instance;

    private GraphicsDeviceManager graphics;

    // BENCHMARK VARIABLE
    private bool benchmarkMode = false;

    private List<SoundEffect> _soundEffects;

    // Score
    private int score = 0;

    // 2D
    private SpriteBatch spriteBatch;
    private SpriteFont spriteFont;

    // 3D rendering
    private Entity player;
    private Entity enemy;
    private Entity sword;
    private Entity camera;
    
    
    private RenderingSystem renderingSystem;
    private InputSystem inputSystem;
    private PhysicsSystem physicsSystem;
    private FollowCameraSystem followCameraSystem;
    private AISystem aiSystem;
    private WeaponSystem weaponSystem;
    private SpawnerSystem spawnerSystem;
    
    public Model CubeModel;
    internal Space GameSpace { get; set; }

    // Diagnostics variables
    //private int framesPerSecond;
    private int frameCount;
    private double elapsedTime;
    private double fps;
    private double memoryUsage; // Approximation of the total amount of memory currently allocated by the .NET garbage collector (GC) for managed objects (should be a couple of MB)

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        graphics.PreferredBackBufferWidth = (int)Math.Round(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.8f, 0);
        graphics.PreferredBackBufferHeight = (int)Math.Round(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.8f, 0);
        graphics.ApplyChanges();
        
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        instance = this;
    }

    protected override void Initialize()
    {
        GameSpace = new Space();
        GameSpace.ForceUpdater.Gravity = new Vector3(0, -9.81f, 0);
        
        // 3D
        EntityBuilder builder = new EntityBuilder()
            .AddTransformComponent()
            .AddMeshComponent("Models/Sword/Sword", new Vector3(0,0,0))
            .AddCollisionComponent(new Vector3(30, 30, 0), new BoxShape(3, 2, 20), new Vector3(5, 5, 50), "Weapon", 0);
        sword = builder.Build();
        
        builder = new EntityBuilder()
            .AddTransformComponent()
            .AddMeshComponent("Models/player_character", new Vector3(0,-3.5f,0))
            .AddCollisionComponent(new Vector3(30,30,0),new CapsuleShape(10f, 5f), new Vector3(20, 50, 20), "Player", -15f)
            .AddWeaponComponent(sword, 20f, 3.5f)
            .AddInputComponent();
        player = builder.Build();

        // Create a new camera and add the PerspectiveCameraComponent to it with a transform
        builder = new EntityBuilder()
            .AddTransformComponent(0f, 400f, -100f)
            .AddPerspectiveCameraComponent(45, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.AspectRatio, 0.1f, 1000f);
        camera = builder.Build();
        TransformComponent cameraTransform = (TransformComponent)camera.GetComponent<TransformComponent>(); 
        ((PerspectiveCameraComponent)camera.GetComponent<PerspectiveCameraComponent>()).Transform = cameraTransform;
        ((PerspectiveCameraComponent)camera.GetComponent<PerspectiveCameraComponent>())
            .LookAt((TransformComponent)player.GetComponent<TransformComponent>());
        
        
        renderingSystem = new RenderingSystem(camera);
        inputSystem = new InputSystem(graphics);
        followCameraSystem = new FollowCameraSystem(player, camera);
        physicsSystem = new PhysicsSystem(GameSpace, this);
        aiSystem = new AISystem();
        weaponSystem = new WeaponSystem();

        aiSystem.OnEnemyKilled += (entity) => { 
            score++;
            // EntityManager.Instance.DestroyEntity(entity.Id); 
        };
        
        // Physics
        base.Initialize();
    }

    protected override void LoadContent()
    {
        // 2D
        spriteBatch = new SpriteBatch(GraphicsDevice);
        spriteFont = Content.Load<SpriteFont>("Fonts/Arial");
        Model enemyModel = Content.Load<Model>("Models/enemy_character");
        spawnerSystem = new SpawnerSystem(GameSpace, aiSystem, enemyModel, benchmarkMode);
        
        // 3D
        CubeModel = Content.Load<Model>("Models/cube");
        // Call the load content for each MeshComponent in the component manager
        foreach (var (_, component) in ComponentManager.Instance.GetComponentArray(typeof(MeshComponent)).GetEntityComponents())
        {
            var meshComponent = (MeshComponent) component;
            meshComponent.LoadContent(Content);
        }

        foreach (var (_, component) in ComponentManager.Instance.GetComponentArray(typeof(CollisionComponent)).GetEntityComponents())
        {
            var collisionComponent = (CollisionComponent) component;
            GameSpace.Add(collisionComponent.CollisionEntity);

            if (!collisionComponent.isVisible) { continue; }

            Matrix scaling;
            if (collisionComponent.Shape is CapsuleShape cS)
            {
                scaling =  Matrix.CreateScale(cS.Radius , cS.Length , cS.Radius );
            }else if (collisionComponent.Shape is BoxShape bS)
            {
                scaling = Matrix.CreateScale(bS.Width, bS.Height, bS.Length);
            }
            else
            {
                scaling = Matrix.Identity;
            }
            Components.Add(new EntityModel(collisionComponent.CollisionEntity, CubeModel, scaling, this));
        }

        // Physics
        Box ground = new Box(Vector3.Zero, 500, 1, 500);
        GameSpace.Add(ground);

        //Go through the list of entities in the space and create a graphical representation for them.
        foreach (BEPUphysics.Entities.Entity e in GameSpace.Entities)
        {
            if (e as Box != null)
            {
                Box box = e as Box;
                Matrix
                    scaling = Matrix.CreateScale(box.Width, box.Height,
                        box.Length); //Since the cube model is 1x1x1, it needs to be scaled to match the size of each individual box.
                EntityModel model = new EntityModel(e, CubeModel, scaling, this);
                //Add the drawable game component for this entity to the game.
                Components.Add(model);
            }
        }
    }

    protected override void Update(GameTime gameTime)
    {
        physicsSystem.Update(gameTime);
        aiSystem.Update(gameTime);
        
        if (followCameraSystem != null)
        {
            followCameraSystem.Update(gameTime);
        }
        inputSystem.Update(gameTime);
        weaponSystem.Update(gameTime);
        spawnerSystem.Update(gameTime);
        // Allows the game to exit
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

        // Calculate elapsed time and FPS
        //framesPerSecond = (int)Math.Round(1f / gameTime.ElapsedGameTime.TotalSeconds);
        double deltaTime = gameTime.ElapsedGameTime.TotalSeconds;
        elapsedTime += deltaTime;
        frameCount++;

        if (elapsedTime >= 1.0)
        {
            fps = (int)Math.Round(frameCount / elapsedTime);

            // Reset counters
            frameCount = 0;
            elapsedTime = 0.0;
        }

        // Memory usage
        memoryUsage = GC.GetTotalMemory(false) / 1048576.0; // Convert from Bytes to MB
        
        
        // Update the Space object in your game's update loop
        GameSpace.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // 3D
        renderingSystem.Draw(gameTime);

        // Disable depth buffering
        GraphicsDevice.DepthStencilState = DepthStencilState.None;

        // 2D
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone);
        spriteBatch.DrawString(spriteFont, "Score: " + score, new Vector2(10f, 10f), Color.White);
        spriteBatch.DrawString(spriteFont, "FPS: " + fps, new Vector2(10f, 60f), Color.White);
        spriteBatch.DrawString(spriteFont, "Memory Usage: " + memoryUsage.ToString("0.00") + " MB", new Vector2(10f, 80f), Color.White);
        
        spriteBatch.End();

        // Re-enable depth buffering
        GraphicsDevice.DepthStencilState = DepthStencilState.Default;

        base.Draw(gameTime);
    }

    public void ExitGame()
    {
        Exit();
    }
}