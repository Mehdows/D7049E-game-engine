using System;
using System.Collections.Generic;
using ArenaGame.Ecs;
using ArenaGame.Ecs.Archetypes;
using ArenaGame.Ecs.Components;
using ArenaGame.Ecs.Systems;
using BEPUphysics;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.NarrowPhaseSystems.Pairs;
using BEPUutilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Matrix = BEPUutilities.Matrix;
using Quaternion = BEPUutilities.Quaternion;
using Vector3 = BEPUutilities.Vector3;

namespace ArenaGame;

public class Game1 : Game
{
    private static Game1 instance;

    public static Game1 Instance
    {
        get { return instance; }
    }

    private GraphicsDeviceManager graphics;
    private EntityManager entityManager;
    private ComponentManager componentManager;
    
    private List<SoundEffect> _soundEffects;

    
    private PlayerControllerSystem playerControllerSystem;

    // 3D rendering
    private Entity player;
    private Entity enemy;
    private Entity sword;
    private Entity camera;
    
    private TransformComponent playerTransform;
    
    private RenderingSystem renderingSystem;
    private InputSystem inputSystem;
    private PhysicsSystem physicsSystem;
    private FollowCameraSystem followCameraSystem;
    private AISystem aiSystem;
    private WeaponSystem weaponSystem;
    private SpawnerSystem spawnerSystem;
    
    private Model model;
    public Model CubeModel;
    internal Space GameSpace { get; set; }
    private BEPUphysics.Entities.Entity entity;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        graphics.PreferredBackBufferWidth = (int)Math.Round(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.8f, 0);
        graphics.PreferredBackBufferHeight = (int)Math.Round(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.8f, 0);
        graphics.ApplyChanges();
        
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        // Create entity and component managers
        entityManager = EntityManager.Instance;
        componentManager = ComponentManager.Instance;
    }

    protected override void Initialize()
    {
        GameSpace = new Space();
        GameSpace.ForceUpdater.Gravity = new Vector3(0, -9.81f, 0);
        // 3D
        
        EntityBuilder builder = new EntityBuilder()
            .AddTransformComponent()
            .AddMeshComponent("Models/sword", new Vector3(0,0,0))
            .AddCollisionComponent(new Vector3(30, 30, 0), new BoxShape(3, 2, 20), new Vector3(5, 5, 50), "Weapon", 0);
        sword = builder.Build();
        
        builder = new EntityBuilder()
            .AddTransformComponent()
            .AddMeshComponent("Models/player_character", new Vector3(0,-3.5f,0))
            .AddCollisionComponent(new Vector3(30,30,0),new CapsuleShape(10f, 5f), new Vector3(20, 50, 20), "Player", -15f)
            .AddWeaponComponent(sword, 20f, 3.5f)
            .AddInputComponent();
        player = builder.Build();

        builder = new EntityBuilder()
            .AddTransformComponent()
            .AddMeshComponent("Models/enemy_character", new Vector3(0,-3.5f, 0))
            .AddCollisionComponent(new Vector3(10, 30, 0), new CapsuleShape(10f, 5f), new Vector3(20, 50, 20), "Enemy", -15f)
            .AddAIControllerComponent(EnemyType.Basic);
        enemy = builder.Build();

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
        // spawnerSystem = new SpawnerSystem();
        
        // Physics
        base.Initialize();
    }

    protected override void LoadContent()
    {
        
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
        // spawnerSystem.Update(gameTime);
        // Allows the game to exit
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
        
        // Update the Space object in your game's update loop
        GameSpace.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        renderingSystem.Draw(gameTime);

        base.Draw(gameTime);
    }
}