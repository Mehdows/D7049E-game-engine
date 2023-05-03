using System;
using BEPUphysics.BroadPhaseEntries;
using BEPUphysics.Entities.Prefabs;
using BEPUutilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Matrix = BEPUutilities.Matrix;
using Vector3 = BEPUutilities.Vector3;

namespace ArenaGame.Ecs.Components;

public class MeshComponent : IComponent
{
    private readonly String modelPath;
    public Matrix Transform;
    public Capsule Capsule { get; set; }
    public Model Model { get; set; }

    public MeshComponent(String modelPath) 
    {
        this.modelPath = modelPath;
    }

    public new void LoadContent(ContentManager contentManager)
    {
        Capsule = new Capsule(new Vector3(0, 40, 0), 20, 5);
        Transform =  Matrix.CreateScale(Capsule.Radius/30, Capsule.Length/110, Capsule.Radius/30);
        Model = contentManager.Load<Model>(modelPath);
        
    }
}