using System.Net.Mime;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace ArenaGame.Ecs.Components;

public class SpriteComponent : Component {
    // public string spriteName;
    public Texture2D playerTexture;

    public SpriteComponent(Texture2D playerTexture = null) 
    {
        if (playerTexture == null)
        {
        }
        this.playerTexture = playerTexture;
    }
}