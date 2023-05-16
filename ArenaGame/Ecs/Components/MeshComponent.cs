using System;
using BEPUutilities;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ArenaGame.Ecs.Components;

public class MeshComponent : IComponent
{
    private readonly String modelPath;
    public Model Model { get; set; }

    public Vector3 localOffset { get; set; }
    
    public MeshComponent(String modelPath, Vector3 localOffset) 
    {
        this.modelPath = modelPath;
        this.localOffset = localOffset;
    }
    
    public MeshComponent(Model model, Vector3 localOffset) 
    {
        Model = model;
        this.localOffset = localOffset;
    }
    public MeshComponent(String modelPath) 
    {
        this.modelPath = modelPath;
        this.localOffset = Vector3.Zero;
    }

    public new void LoadContent(ContentManager contentManager)
    {
        if (Model == null)
        {
            Model = contentManager.Load<Model>(modelPath);
        }
        
    }
}