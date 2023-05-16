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

    // 2D
    //private SpriteBatch spriteBatch;
    //private Entity player; 
    
    private PlayerControllerSystem playerControllerSystem;

    // 3D rendering
    private Entity player;
    private Entity enemy;
    private TransformComponent playerTransform;
    private Entity sword;
    private CameraComponent cameraComponent;
    
    private Entity camera;
    // private RenderingSystem renderingSystem;
    private RenderingSystem2 renderingSystem;
    private InputSystem inputSystem;
    private WeaponSystem weaponSystem;
    private SpawnerSystem spawnerSystem;
    private PhysicsSystem physicsSystem;
    private FollowCameraSystem followCameraSystem;
    private AISystem aiSystem;
    private Model model;

    // Physics
    public Model CubeModel;
    public Model PlaygroundModel;
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
            .AddMeshComponent("Models/FreeMale")
            .AddCollisionComponent(new Vector3(30,30,0),new CapsuleShape(10f, 5f), new Vector3(30, 60, 30), "Player")
            .AddInputComponent();
        player = builder.Build();

        builder = new EntityBuilder()
            .AddTransformComponent()
            .AddMeshComponent("Models/FreeMale")
            .AddCollisionComponent(new Vector3(10, 30, 0), new CapsuleShape(10f, 5f), new Vector3(30, 60, 30), "Enemy")
            .AddAIControllerComponent(EnemyType.Basic);
        enemy = builder.Build();
        
        WeaponArchetype weaponArchetype = ArchetypeFactory.GetArchetype(EArchetype.Weapon) as WeaponArchetype;
        sword = entityManager.CreateEntityWithArchetype(weaponArchetype);


        // Create a new camera and add the PerspectiveCameraComponent to it with a transform
        builder = new EntityBuilder()
            .AddTransformComponent(0f, 400f, -100f)
            .AddPerspectiveCameraComponent(45, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.AspectRatio, 0.1f, 1000f);
        camera = builder.Build();
        TransformComponent cameraTransform = (TransformComponent)camera.GetComponent<TransformComponent>(); 
        ((PerspectiveCameraComponent)camera.GetComponent<PerspectiveCameraComponent>()).Transform = cameraTransform;
        ((PerspectiveCameraComponent)camera.GetComponent<PerspectiveCameraComponent>())
            .LookAt((TransformComponent)player.GetComponent<TransformComponent>());
        
        
        renderingSystem = new RenderingSystem2(camera);
        inputSystem = new InputSystem();
        followCameraSystem = new FollowCameraSystem(player, camera);
        physicsSystem = new PhysicsSystem(GameSpace, this);
        aiSystem = new AISystem();
        
        // inputSystem = new InputSystem(graphics);
        weaponSystem = new WeaponSystem();
        spawnerSystem = new SpawnerSystem();

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
        
        // 3D
        // Model playerModel = Content.Load<Model>("Models/FreeMale");
        // MeshComponent playerMesh =(MeshComponent) player.AddComponent<MeshComponent>(new MeshComponent(playerModel));

        // Physics
        PlaygroundModel = Content.Load<Model>("Models/playground");
        Box ground = new Box(Vector3.Zero, 500, 1, 500);
        GameSpace.Add(ground);
        
        // Box Enemy1 = new Box(new Vector3(0, 10, 0), 5, 5, 5, 1);
        // Box Enemy2 = new Box(new Vector3(0, 15, 0), 5, 5, 5, 1);
        // Box Enemy3 = new Box(new Vector3(0, 20, 0), 5, 5, 5, 1);
        // GameSpace.Add(Enemy1);
        // GameSpace.Add(Enemy2);
        // GameSpace.Add(Enemy3);
        // Enemy1.Tag = "Enemy";
        // Enemy2.Tag = "Enemy";
        // Enemy3.Tag = "Enemy";
        
        // //Now that we have something to fall on, make a few more boxes.
        // //These need to be dynamic, so give them a mass- in this case, 1 will be fine.
        // GameSpace.Add(new Box(new Vector3(0, 4, 0), 5, 5, 5, 1));
        // GameSpace.Add(new Box(new Vector3(0, 8, 0), 5, 5, 5, 1));
        // GameSpace.Add(new Box(new Vector3(0, 12, 0), 5, 5, 5, 1));

        //Create a physical environment from a triangle mesh.
        //First, collect the the mesh data from the model using a helper function.
        //This special kind of vertex inherits from the TriangleMeshVertex and optionally includes
        //friction/bounciness data.
        //The StaticTriangleGroup requires that this special vertex type is used in lieu of a normal TriangleMeshVertex array.
        Vector3[] vertices;
        int[] indices;
        ModelDataExtractor.GetVerticesAndIndicesFromModel(PlaygroundModel, out vertices, out indices);
        //Give the mesh information to a new StaticMesh.  
        //Give it a transformation which scoots it down below the kinematic box entity we created earlier.
        var mesh = new StaticMesh(vertices, indices, new AffineTransform(new Vector3(0, -10, 0)));

        //Add it to the space!
        GameSpace.Add(mesh);
        // Make it visible too
        Components.Add(new StaticModelComponent(PlaygroundModel, mesh.WorldTransform.Matrix));

        // //Hook an event handler to an entity to handle some game logic.
        // //Refer to the Entity Events documentation for more information.
        // Box deleterBox = new Box(new Vector3(5, 2, 0), 3, 3, 3);
        // GameSpace.Add(deleterBox);
        // deleterBox.CollisionInformation.Events.InitialCollisionDetected += HandleCollision;

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

    /// <summary>
    /// Used to handle a collision event triggered by an entity specified above.
    /// </summary>
    /// <param name="sender">Entity that had an event hooked.</param>
    /// <param name="other">Entity causing the event to be triggered.</param>
    /// <param name="pair">Collision pair between the two objects in the event.</param>
    void HandleCollision(EntityCollidable sender, Collidable other, CollidablePairHandler pair)
    {
        //This type of event can occur when an entity hits any other object which can be collided with.
        //They aren't always entities; for example, hitting a StaticMesh would trigger this.
        //Entities use EntityCollidables as collision proxies; see if the thing we hit is one.
        // var otherEntityInformation = other as EntityCollidable;
        // if (otherEntityInformation != null)
        // {
        //     //We hit an entity! remove it.
        //     GameSpace.Remove(otherEntityInformation.Entity);
        //     //Remove the graphics too.
        //     Components.Remove((EntityModel)otherEntityInformation.Entity.Tag);
        // }
        // 3D
        Model model = Content.Load<Model>("Models/player_character");
        player3D.AddComponent<MeshComponent>(new MeshComponent(model));

        Model swordMesh = Content.Load<Model>("Models/sword");
        sword.AddComponent<MeshComponent>(new MeshComponent(swordMesh));

        Model environment = Content.Load<Model>("Models/environment");

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
        
        // Update the Space object in your game's update loop
        GameSpace.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // 2D
        /*
        spriteBatch.Begin();
        spriteBatch.Draw(
            ((SpriteComponent)player.GetComponent<SpriteComponent>()).Texture,
            ((PositionComponent)player.GetComponent<PositionComponent>()).Position, Color.White);
        spriteBatch.End();
        */

        // 3D
        renderingSystem.Draw();

        base.Draw(gameTime);
    }
}