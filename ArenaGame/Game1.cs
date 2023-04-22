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
    private SpriteBatch spriteBatch;
    private EntityManager entityManager;
    private ComponentManager componentManager;
    private Entity player; 
    private PlayerControllerSystem playerControllerSystem;

    private List<SoundEffect> _soundEffects;
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
        // TODO: Add your initialization logic here
        PlayerArchetype playerArchetype = ArchetypeFactory.GetArchetype(EArchetype.Player) as PlayerArchetype;
        player = entityManager.CreateEntityWithArchetype(playerArchetype);
        playerControllerSystem = new PlayerControllerSystem();
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        Texture2D playerTexture = Content.Load<Texture2D>("player");
        player.AddComponent<SpriteComponent>(new SpriteComponent(playerTexture));
        
        spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
       
        playerControllerSystem.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        
        spriteBatch.Begin();
        spriteBatch.Draw(
            ((SpriteComponent)player.GetComponent<SpriteComponent>()).Texture,
            ((PositionComponent)player.GetComponent<PositionComponent>()).Position, Color.White);
        spriteBatch.End();

        // TODO: Add your drawing code here


        base.Draw(gameTime);
    }
}