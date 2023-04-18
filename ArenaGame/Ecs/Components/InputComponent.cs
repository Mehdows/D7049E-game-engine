using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ArenaGame.Ecs.Components;

public class InputComponent : Component
{
    public Dictionary<Keys, Vector2> Bindings { get; set; }

    public InputComponent()
    {
        Bindings = new Dictionary<Keys, Vector2>();
    }
}