
using System;
using ArenaGame.Ecs.Components;
using Microsoft.Xna.Framework;

namespace ArenaGame.Ecs.Systems;

public class AudioSystem : ISystem
{


    public void AddEntity(Entity entity)
    {
        /*if (entity.HasComponent<AudioComponent>()) 
        { 
            entities.Add(entity);
        }*/
    }


    public void Update(GameTime gameTime)
    {
        /*
        foreach (Entity entity in entities)
        {

            var audio = entity.GetComponent<AudioComponent>();

            
            if (audio.ShouldPlaySoundEffect)
            {
                audio.PlaySoundEffect();
            }

            if (audio.ShouldStopSoundEffect)
            {
                audio.StopSoundEffect();
            }

            

        if (audio.ShouldPlaySong)
            { 
                audio.PlaySong();
            }
            else
            {
                audio.StopSong();
            }

        }
        */
    }
}

