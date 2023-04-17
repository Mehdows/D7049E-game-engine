using System.Drawing;
using System.Numerics;
using ArenaGame.Ecs;
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
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Entity _player;
    private MovementSystem _movementSystem;
    private float playerSpeed = 100f;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _player = new Entity();
        _movementSystem = new MovementSystem();
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _player.AddComponent(new EntityTypeComponent{ type = EntityType.Player});
        _player.AddComponent(new PositionComponent(x: 0, y: 0));
        _player.AddComponent(new VelocityComponent(x: 0, y: 0));
        
        InputComponent input = new InputComponent();
        input.Bindings.Add(Keys.W, new Vector2(0, -1)); // Move up
        input.Bindings.Add(Keys.A, new Vector2(-1, 0)); // Move left
        input.Bindings.Add(Keys.S, new Vector2(0, 1)); // Move down
        input.Bindings.Add(Keys.D, new Vector2(1, 0)); // Move right
        _player.AddComponent(input);
        
        _movementSystem.AddEntity(_player);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        Texture2D playerTexture = Content.Load<Texture2D>("player");
        SpriteComponent sprite = new SpriteComponent(playerTexture);
        _player.AddComponent(sprite);
    }

    protected override void Update(GameTime gameTime)
    {
        _movementSystem.Update(gameTime);
        
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        //_playerGetComponent<InputComponent>().Update(Keyboard.GetState());

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        
        _spriteBatch.Begin();
        _spriteBatch.Draw(_player.GetComponent<SpriteComponent>().playerTexture, _player.GetComponent<PositionComponent>().Position, Color.White);
        _spriteBatch.End();

        // TODO: Add your drawing code here


        base.Draw(gameTime);
    }
}