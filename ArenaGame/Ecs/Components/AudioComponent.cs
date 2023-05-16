using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

namespace ArenaGame.Ecs.Components;


public class AudioComponent : IComponent
{
    public Song Song { get; set; }

    public AudioComponent(Song song)
    {
        Song = song;
    }

    public AudioComponent()
    {
        throw new ContentLoadException("AudioComponent must be initialized with a song.");
    }


}