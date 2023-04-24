using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ArenaGame.Ecs.Components;

public class MeshComponent : IComponent
{
    public Model Model { get; set; }

    public MeshComponent(Model model)
    {
        Model = model;
    }

    public MeshComponent()
    {
        throw new ContentLoadException("MeshComponent must be initialized with a model.");
    }
}