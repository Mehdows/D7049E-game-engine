using Microsoft.Xna.Framework.Graphics;

public class SpriteComponent {
    // public string spriteName;
    public Texture2D playerTexture;

    public SpriteComponent(Texture2D playerTexture)
    {
        this.playerTexture = playerTexture;
    }
}