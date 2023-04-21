using System.Collections.Generic;
using ArenaGame.Ecs.Components;
using Microsoft.Xna.Framework;

namespace ArenaGame.Ecs.Systems;

public interface ISystem
{
    public  void Update(GameTime gameTime);

}