using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArenaGame;

public class Game1 : Game
{
    
    private const int SCREEN_WIDTH = 1024, SCREEN_HEIGHT = 768; 
    private GraphicsDevice _gpu;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteFont _font;
    static public int screenW, screenH;
    Camera cam;

    Input inp;

    Rectangle desktopRect;
    Rectangle screenRect;

    private RenderTarget2D _MainTarget;
    Texture2D test_tex;

    //3D
    Basic3DObject basic3D;

    
    public Game1 ()
    {
        int desktopWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - 10;
        int desktopHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 10;

        _graphics = new GraphicsDeviceManager(this){
            PreferredBackBufferWidth = desktopWidth,
            PreferredBackBufferHeight = desktopHeight,
            IsFullScreen = false,
            PreferredDepthStencilFormat = DepthFormat.None,
            GraphicsProfile = GraphicsProfile.HiDef // Important to allow 4Megs of indices at once
        };
        Window.IsBorderless = true;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _gpu = GraphicsDevice;
        PresentationParameters pp = _gpu.PresentationParameters;
        _spriteBatch = new SpriteBatch(_gpu);
        _MainTarget = new RenderTarget2D(_gpu, SCREEN_WIDTH, SCREEN_HEIGHT, false, pp.BackBufferFormat, DepthFormat.Depth24);
        screenW = _MainTarget.Width;
        screenH = _MainTarget.Height;
        desktopRect = new Rectangle(0, 0, pp.BackBufferWidth, pp.BackBufferHeight);
        screenRect = new Rectangle(0,0, screenW, screenH);

        inp = new Input(pp, _MainTarget);

        // // TODO: Add your initialization logic here
        // ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
        // ballSpeed = 100f;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        // _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        // ballTexture = Content.Load<Texture2D>("ball");
        _font = Content.Load<SpriteFont>("Font");
        test_tex = Content.Load<Texture2D>("ball");

    }

    protected override void UnloadContent()
    {

    }

    protected override void Update(GameTime gameTime)
    {
        // The new input system equivalent of the standard input system
        // if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        //     Exit();
        inp.Update();
        if(inp.back_down || inp.KeyDown(Keys.Escape)) Exit();

        base.Update(gameTime);
    }

    void Set3DStates() {
        _gpu.BlendState = BlendState.NonPremultiplied;
        _gpu.DepthStencilState = DepthStencilState.Default;
        if(_gpu.RasterizerState.CullMode == CullMode.None){
            RasterizerState rs = new RasterizerState { CullMode = CullMode.CullCounterClockwiseFace };
            _gpu.RasterizerState = rs;
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        _gpu.SetRenderTarget(_MainTarget);

        Set3DStates();
        _gpu.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Transparent, 1.0f, 0);

        _gpu.SetRenderTarget(null);
        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone);
        _spriteBatch.Draw(_MainTarget, desktopRect, Color.White);
        _spriteBatch.End();


        base.Draw(gameTime);
    }
}
