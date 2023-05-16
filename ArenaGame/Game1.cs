using System;
using System.Collections.Generic;
using ArenaGame.Ecs;
using ArenaGame.Ecs.Archetypes;
using ArenaGame.Ecs.Components;
using ArenaGame.Ecs.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
    private SpriteFont spriteFont;

    // 3D rendering
    private Entity player3D;
    private Entity sword;
    private CameraComponent cameraComponent;
    
    private RenderingSystem renderingSystem;
    private InputSystem inputSystem;
    private WeaponSystem weaponSystem;
    private SpawnerSystem spawnerSystem;

    // Diagnostics variables
    private int framesPerSecond;
    private double memoryUsage; // Approximation of the total amount of memory currently allocated by the .NET garbage collector (GC) for managed objects (should be a couple of MB)

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
        // 3D
        Player3DArchetype player3DArchetype = ArchetypeFactory.GetArchetype(EArchetype.Player3D) as Player3DArchetype;
        player3D = entityManager.CreateEntityWithArchetype(player3DArchetype);

        WeaponArchetype weaponArchetype = ArchetypeFactory.GetArchetype(EArchetype.Weapon) as WeaponArchetype;
        sword = entityManager.CreateEntityWithArchetype(weaponArchetype);

        TransformComponent cameraTransform = new TransformComponent();
        cameraTransform.Position = new Vector3(0f, 0f, -500f);
        cameraComponent = new CameraComponent(cameraTransform);
        cameraComponent.LookAt(Vector3.Zero);
        renderingSystem = new RenderingSystem(cameraComponent);

        inputSystem = new InputSystem(graphics);
        weaponSystem = new WeaponSystem();
        spawnerSystem = new SpawnerSystem();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        // 2D
        spriteBatch = new SpriteBatch(GraphicsDevice);
        spriteFont = Content.Load<SpriteFont>("Fonts/Arial");

        // 3D
        Model model = Content.Load<Model>("Models/player_character");
        player3D.AddComponent<MeshComponent>(new MeshComponent(model));

        Model swordMesh = Content.Load<Model>("Models/sword");
        sword.AddComponent<MeshComponent>(new MeshComponent(swordMesh));

        Model environment = Content.Load<Model>("Models/environment");

    }

    protected override void Update(GameTime gameTime)
    {
        
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // Diagnostics
        framesPerSecond = (int)Math.Round(1f / gameTime.ElapsedGameTime.TotalSeconds);
        memoryUsage = GC.GetTotalMemory(false) / 1048576.0; // Convert from Bytes to MB

        // 3D
        inputSystem.Update(gameTime);
        weaponSystem.Update(gameTime);
        spawnerSystem.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // 2D
        spriteBatch.Begin();
        spriteBatch.DrawString(spriteFont, "FPS: " + framesPerSecond, new Vector2(10f, 10f), Color.White);
        spriteBatch.DrawString(spriteFont, "Memory Usage: " + memoryUsage.ToString("0.00") + " MB", new Vector2(10f, 30f), Color.White);
        spriteBatch.End();

        // 3D
        renderingSystem.Draw();

        base.Draw(gameTime);
    }
}