using System.Net.Mime;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ArenaGame.Ecs.Components;

public class SpriteComponent : IComponent 
{
    // public string spriteName;
    public Texture2D Texture { get; }

    public SpriteComponent(Texture2D texture) 
    {
        Texture = texture;
    }

    public SpriteComponent()
    {
        throw new ContentLoadException("SpriteComponent must be initialized with a texture.");
    }
}