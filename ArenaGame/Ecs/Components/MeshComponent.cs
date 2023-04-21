using Microsoft.Xna.Framework.Graphics;

namespace ArenaGame.Ecs.Components;

public class MeshComponent : IComponent
{
    public Model Model { get; set; }

    public MeshComponent(Model model)
    {
        Model = model;
    }
}