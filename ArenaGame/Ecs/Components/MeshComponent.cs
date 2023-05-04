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
    public Model Model { get; set; }

    public MeshComponent(String modelPath) 
    {
        this.modelPath = modelPath;
    }

    // TODO Ändra så att Capsule inte är hårdkodad
    public new void LoadContent(ContentManager contentManager)
    {
        Model = contentManager.Load<Model>(modelPath);
        
    }
}