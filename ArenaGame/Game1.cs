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

        cam = new Camera(_gpu, Vector3.Down, inp);
        basic3D = new Basic3DObject(_gpu, cam.up, Content);


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

        basic3D.AddFloor(200,200, Vector3.Zero, Vector3.Zero, "ball", null);
        basic3D.AddCube(50,50,50, Vector3.Zero, Vector3.Zero, "ball", null);
        basic3D.objects[1].pos = new Vector3(30, -40, -30);

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

        cam.MoveCamera(new Vector3(inp.gp.ThumbSticks.Left.X, inp.gp.ThumbSticks.Right.Y, inp.gp.ThumbSticks.Left.Y));
        if(inp.KeyDown(Keys.Up)) basic3D.objects[1].pos.Z++;
        if(inp.KeyDown(Keys.Down)) basic3D.objects[1].pos.Z--;
        if(inp.KeyDown(Keys.Left)) basic3D.objects[1].pos.X--;
        if(inp.KeyDown(Keys.Right)) basic3D.objects[1].pos.X++;
        if(inp.KeyDown(Keys.A)) basic3D.objects[1].pos.Y++;
        if(inp.KeyDown(Keys.Z)) basic3D.objects[1].pos.Y--;
        basic3D.objects[1].rot.Y += 0.03f;
        basic3D.objects[1].UpdateTransform();

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

        basic3D.Draw(cam);


        _gpu.SetRenderTarget(null);
        _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone);
        _spriteBatch.Draw(_MainTarget, desktopRect, Color.White);
        _spriteBatch.End();


        base.Draw(gameTime);
    }
}
