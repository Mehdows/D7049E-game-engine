using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ArenaGame.Ecs.Components;

public enum InputKey {
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Up,
    Down,
    Left,
    Right,
    Space
}

public class InputComponent : IComponent {
    private bool[] keysDown = new bool[Enum.GetNames(typeof(InputKey)).Length];

    public void SetKeyDown(InputKey key) {
        keysDown[(int)key] = true;
    }

    public void SetKeyUp(InputKey key) {
        keysDown[(int)key] = false;
    }

    public bool IsKeyDown(InputKey key) {
        return keysDown[(int)key];
    }

    public void Reset() {
        keysDown = new bool[Enum.GetNames(typeof(InputKey)).Length];
    }
}