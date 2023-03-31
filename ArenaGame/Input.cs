using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArenaGame;

public class Input{

    public const ButtonState ButtonUp = ButtonState.Released;
    public const ButtonState ButtonDown = ButtonState.Pressed;

    // KEYBOARD - We have and kb and old kb states in order to check if a key has been pressed or held down
    public KeyboardState kb, okb;
    public bool shift_down, control_down, alt_down, shift_press, control_press, alt_press;
    public bool old_shift_down, old_control_down, old_alt_down;

    // MOUSE
    public MouseState ms, oms;
    public bool leftClick, midClick, rightClick, leftDown, midDown, rightDown;
    public int mosX, mosY;
    public Vector2 mouseVec;
    public Point mousePos;

    // GAMEPAD
    public GamePadState gp, ogp;
    public bool A_down, B_down, X_down, Y_down, RB_down, LB_down, start_down, back_down, leftStick_down, rightStick_down;
    public bool A_press, B_press, X_press, Y_press, RB_press, LB_press, start_press, back_press, leftStick_press, rightStick_press;

    float screenScaleX, screenScaleY; // Used to scale desktop resolution mouse coordinates to match position in MainTarget
    public Input(PresentationParameters pp, RenderTarget2D target2D){
        screenScaleX = 1.0f / ((float) pp.BackBufferWidth / (float) target2D.Width);
        screenScaleY = 1.0f / ((float) pp.BackBufferHeight / (float) target2D.Height);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool KeyPress(Keys k){if (kb.IsKeyDown(k) && okb.IsKeyUp(k)) return true; else return false;}

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool KeyDown(Keys k){if (kb.IsKeyDown(k)) return true; else return false;}

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool ButtonsPress(Buttons button){if (gp.IsButtonDown(button) && ogp.IsButtonUp(button)) return true; return false;}

    public void Update()
    {
        // Update all the old states
        old_alt_down = alt_down; 
        old_shift_down = shift_down;
        old_control_down = control_down;

        okb = kb;
        oms = ms;
        ogp = gp;

        kb = Keyboard.GetState();
        ms = Mouse.GetState();
        gp = GamePad.GetState(PlayerIndex.One);

        //KEYBOARD
        shift_down = shift_press = control_down = control_press = alt_down = alt_press = false;

        // Check for shift, control, and alt keys being held down
        if(kb.IsKeyDown(Keys.LeftShift) || kb.IsKeyDown(Keys.RightShift)) shift_down = true;
        if(kb.IsKeyDown(Keys.LeftControl) || kb.IsKeyDown(Keys.RightControl)) control_down = true;
        if(kb.IsKeyDown(Keys.LeftAlt) || kb.IsKeyDown(Keys.RightAlt)) alt_down = true;

        // Check for shift, control, and alt keys being pressed
        if((shift_down) && (!old_shift_down)) shift_press = true;
        if((control_down) && (!old_control_down)) control_press = true;
        if((alt_down) && (!old_alt_down)) alt_press = true;

        // MOUSE
        mouseVec = new Vector2((float)ms.Position.X * screenScaleX, (float)ms.Position.Y * screenScaleY);
        mosX = (int)mouseVec.X;
        mosY = (int)mouseVec.Y;
        mousePos = new Point(mosX, mosY);

        leftClick = midClick = rightClick = leftDown = midDown = rightDown = false;

        if (ms.LeftButton == ButtonDown) leftDown = true;
        if (ms.MiddleButton == ButtonDown) midDown = true;
        if (ms.RightButton == ButtonDown) rightDown = true;

        if((leftDown) && (oms.LeftButton == ButtonUp)) leftClick = true;
        if((midDown) && (oms.MiddleButton == ButtonUp)) midClick = true;
        if((rightDown) && (oms.RightButton == ButtonUp)) rightClick = true;

        // GAMEPAD
        A_down = B_down = X_down = Y_down = RB_down = LB_down = start_down = back_down = leftStick_down = rightStick_down = false;
        A_press = B_press = X_press = Y_press = RB_press = LB_press = start_press = back_press = leftStick_press = rightStick_press = false;
        if(gp.Buttons.A == ButtonState.Pressed) { A_down = true; if(gp.Buttons.A == ButtonState.Released) A_press = true; }
        if(gp.Buttons.B == ButtonState.Pressed) { B_down = true; if(gp.Buttons.B == ButtonState.Released) B_press = true; }
        if(gp.Buttons.X == ButtonState.Pressed) { X_down = true; if(gp.Buttons.X == ButtonState.Released) X_press = true; }
        if(gp.Buttons.Y == ButtonState.Pressed) { Y_down = true; if(gp.Buttons.Y == ButtonState.Released) Y_press = true; }
        if(gp.Buttons.RightShoulder == ButtonState.Pressed) { RB_down = true; if(gp.Buttons.RightShoulder == ButtonState.Released) RB_press = true; }
        if(gp.Buttons.LeftShoulder == ButtonState.Pressed) { LB_down = true; if(gp.Buttons.LeftShoulder == ButtonState.Released) LB_press = true; }
        if(gp.Buttons.Start == ButtonState.Pressed) { start_down = true; if(gp.Buttons.Start == ButtonState.Released) start_press = true; }
        if(gp.Buttons.Back == ButtonState.Pressed) { back_down = true; if(gp.Buttons.Back == ButtonState.Released) back_press = true; }
        if(gp.Buttons.LeftStick == ButtonState.Pressed) { leftStick_down = true; if(gp.Buttons.LeftStick == ButtonState.Released) leftStick_press = true; }
        if(gp.Buttons.RightStick == ButtonState.Pressed) { rightStick_down = true; if(gp.Buttons.RightStick == ButtonState.Released) rightStick_press = true; };
    }
}