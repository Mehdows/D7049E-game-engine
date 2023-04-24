using System.Collections.Generic;
using ArenaGame.Ecs;
using ArenaGame.Ecs.Archetypes;
using ArenaGame.Ecs.Components;
using ArenaGame.Ecs.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Color = Microsoft.Xna.Framework.Color;

namespace ArenaGame;

public class Game1 : Game
{
    private GraphicsDeviceManager graphics;
    private EntityManager entityManager;
    private ComponentManager componentManager;
    
    private List<SoundEffect> _soundEffects;

    // 2D
    private SpriteBatch spriteBatch;
    private Entity player; 
    
    private PlayerControllerSystem playerControllerSystem;

    // 3D rendering
    private Entity player3D;
    private CameraComponent cameraComponent;
    
    private RenderingSystem renderingSystem;
    private InputSystem inputSystem;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        graphics.PreferredBackBufferWidth = 1920;  
        graphics.PreferredBackBufferHeight = 1080; 
        graphics.ApplyChanges();
        
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
        // Create entity and component managers
        entityManager = EntityManager.Instance;
        componentManager = ComponentManager.Instance;
    }

    protected override void Initialize()
    {
        // 2D
        PlayerArchetype playerArchetype = ArchetypeFactory.GetArchetype(EArchetype.Player) as PlayerArchetype;
        player = entityManager.CreateEntityWithArchetype(playerArchetype);
        playerControllerSystem = new PlayerControllerSystem();

        // 3D
        Player3DArchetype player3DArchetype = ArchetypeFactory.GetArchetype(EArchetype.Player3D) as Player3DArchetype;
        player3D = entityManager.CreateEntityWithArchetype(player3DArchetype);

        TransformComponent cameraTransform = new TransformComponent();
        cameraTransform.Position = new Vector3(0f, 0f, -500f);
        cameraComponent = new CameraComponent(cameraTransform);
        cameraComponent.LookAt(Vector3.Zero);
        renderingSystem = new RenderingSystem(cameraComponent);

        inputSystem = new InputSystem();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        // 2D
        Texture2D playerTexture = Content.Load<Texture2D>("Sprites/player");
        player.AddComponent<SpriteComponent>(new SpriteComponent(playerTexture));

        // 3D
        Model model = Content.Load<Model>("Models/FreeMale");
        player3D.AddComponent<MeshComponent>(new MeshComponent(model));
    }

    protected override void Update(GameTime gameTime)
    {
        
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // 2D
        //playerControllerSystem.Update(gameTime);

        // 3D
        inputSystem.Update(gameTime);

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