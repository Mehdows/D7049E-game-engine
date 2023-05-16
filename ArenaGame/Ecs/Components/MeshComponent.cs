using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ArenaGame.Ecs.Components;

public class MeshComponent : IComponent
{
    private readonly String modelPath;
    public Model Model { get; set; }

    public MeshComponent(String modelPath) 
    {
        this.modelPath = modelPath;
    }

    public new void LoadContent(ContentManager contentManager)
    {
        Model = contentManager.Load<Model>(modelPath);
        
    }
}