using System;
using BEPUphysics.BroadPhaseEntries;
using BEPUutilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Matrix = BEPUutilities.Matrix;
using Vector3 = BEPUutilities.Vector3;

namespace ArenaGame.Ecs.Components;

public class MeshComponent : IComponent, IDrawable 
{
    public int DrawOrder { get; }
    public bool Visible { get; }
    public event EventHandler<EventArgs> DrawOrderChanged;
    public event EventHandler<EventArgs> VisibleChanged;
    
    public Model Model { get; set; }
    public Matrix Transform;
    Microsoft.Xna.Framework.Matrix[] boneTransforms;

    public MeshComponent(Model model) 
    {
        Vector3[] vertices;
        int[] indices;
        ModelDataExtractor.GetVerticesAndIndicesFromModel(model, out vertices, out indices);
        var mesh  = new StaticMesh(vertices, indices, new AffineTransform(new Vector3(0, 0, 0)));
        Transform = mesh.WorldTransform.Matrix;
        Model = model;
    }

    public MeshComponent() 
    {
        throw new ContentLoadException("MeshComponent must be initialized with a model.");
    }
}