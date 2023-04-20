using System.Drawing;
using System.Numerics;
using ArenaGame.Ecs;
using ArenaGame.Ecs.Archetypes;
using ArenaGame.Ecs.Components;
using ArenaGame.Ecs.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Color = Microsoft.Xna.Framework.Color;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace ArenaGame;

public class Game1 : Game
{
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private EntityManager entityManager;
    private ComponentManager componentManager;
    private Entity player;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
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
        MovementSystem movementSystem = new MovementSystem();
        movementSystem.Update(gameTime);

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