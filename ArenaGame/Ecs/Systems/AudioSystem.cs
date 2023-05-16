
using ArenaGame.Ecs.Archetypes;
using ArenaGame.Ecs.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;


using System.Diagnostics;

namespace ArenaGame.Ecs.Systems;

public class AudioSystem : ISystem
{

    private bool isInitialized;
    private bool isMuted;


    public void Initialize()
    {
        Player3DArchetype player3DArchetype = (Player3DArchetype)ArchetypeFactory.GetArchetype(EArchetype.Player3D);
        Entity player = EntityManager.Instance.GetEntitiesWithArchetype(player3DArchetype)[0];
        AudioComponent audio = (AudioComponent)player.GetComponent<AudioComponent>();

        if (audio == null)
        {
            // Handle the case when the AudioComponent is not found
            throw new System.Exception("AudioComponent not found.");
        }

        PlaySong(audio);
        isInitialized = true;
    }

    public void Update(GameTime gameTime)
    {
        if (!isInitialized)
        {
            Initialize();
        }
    }

    // PLay song
    private void PlaySong(AudioComponent audio)
    {
        MediaPlayer.Volume = 0.05f;
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Play(audio.Song);
    }

    /*
    //Mute song
    public void ToggleMute()
    {
        isMuted = !isMuted;
        MediaPlayer.IsMuted = isMuted;
    }
    */
}

