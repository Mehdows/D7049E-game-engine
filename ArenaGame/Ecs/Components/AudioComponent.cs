using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace ArenaGame.Ecs.Components;


public class AudioComponent
{


    public float volume;
    public Song song;
   // public SoundEffect soundEffect;
    private bool songIsPlaying;
    private float Volume;
    private bool shouldPlaySong;


    public bool ShouldPlaySong
    {
        get { return shouldPlaySong; }
        set { shouldPlaySong = value; }
    }


    public AudioComponent(Song background_song, bool ShouldPlaySong)
    {
        // soundEffect = soundEffect
        song = background_song;
        Volume = MediaPlayer.Volume;
        shouldPlaySong = ShouldPlaySong;
    }




    /*
    public void PlaySoundEffect()
    {
        if (effectisPlaying) return;
        soundEffectInstance = _soundEffect.CreateInstance();
        soundEffectInstance.Play();
        isPlaying = true;
    }

    public void StopSoundEffect()
        {
            if (!effectisPlaying) return;
            soundEffectInstance.Stop();
            isPlaying = false;
        }

    */


    public void PlaySong()
    {
        if (songIsPlaying) return;
        MediaPlayer.Volume -= 0.95f;
        MediaPlayer.Play(song);
        songIsPlaying = true;
    }

    public void StopSong()
    {
        if (!songIsPlaying) return;
        MediaPlayer.Stop();
        songIsPlaying = false;
    }

}