using System;
using System.Collections.Generic;
using ArenaGame.Ecs.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public class InputComponent : IComponent
{
    private readonly Dictionary<InputKey, bool> _keyStates;
    private readonly Dictionary<InputKey, bool> _previousKeyStates;

    public InputComponent()
    {
        _keyStates = new Dictionary<InputKey, bool>();
        _previousKeyStates = new Dictionary<InputKey, bool>();

        // Initialize all keys to false
        foreach (InputKey key in Enum.GetValues(typeof(InputKey)))
        {
            _keyStates[key] = false;
            _previousKeyStates[key] = false;
        }
    }

    public  void Update(GameTime gameTime)
    {
        // Copy current state to previous state
        foreach (InputKey key in _keyStates.Keys)
        {
            _previousKeyStates[key] = _keyStates[key];
        }

        // Check for key presses and holds
        KeyboardState keyboardState = Keyboard.GetState();
        foreach (InputKey key in _keyStates.Keys)
        {
            Keys monogameKey = (Keys)key;
            bool isPressed = keyboardState.IsKeyDown(monogameKey);
            _keyStates[key] = isPressed;
        }
    }

    public bool IsKeyPressed(InputKey key)
    {
        return _keyStates[key] && !_previousKeyStates[key];
    }

    public bool IsKeyHeld(InputKey key)
    {
        return _keyStates[key];
    }
}

public enum InputKey
{
    Up = Keys.W,
    Down = Keys.S,
    Left = Keys.A,
    Right = Keys.D,
    Jump = Keys.Space,
    Attack = Keys.Enter,
    FullScreen = Keys.F11
}