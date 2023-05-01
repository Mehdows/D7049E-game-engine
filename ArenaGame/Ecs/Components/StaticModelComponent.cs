using System;
using ArenaGame;
using ArenaGame.Ecs;
using ArenaGame.Ecs.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Matrix = BEPUutilities.Matrix;

namespace ArenaGame;

/// <summary>
/// Component that draws a model.
/// </summary>
public class StaticModelComponent : IComponent, IDrawable 
{
    Model model;
    public Matrix Transform;
    Microsoft.Xna.Framework.Matrix[] boneTransforms;


    /// <summary>
    /// Creates a new StaticModel.
    /// </summary>
    /// <param name="model">Graphical representation to use for the entity.</param>
    /// <param name="transform">Base transformation to apply to the model before moving to the entity.</param>
    public StaticModelComponent(Model model, Matrix transform)
    {
        this.model = model;
        Transform = transform;

        //Collect any bone transformations in the model itself.
        //The default cube model doesn't have any, but this allows the StaticModel to work with more complicated shapes.
        boneTransforms = new Microsoft.Xna.Framework.Matrix[model.Bones.Count];
        foreach (ModelMesh mesh in model.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.EnableDefaultLighting();
            }
        }
    }

    public override void Draw(GameTime gameTime)
    {
        PerspectiveCameraComponent cameraComponent =
            (PerspectiveCameraComponent)ComponentManager.Instance.GetComponentArray(typeof(PerspectiveCameraComponent)).GetEntityComponents()[0].Item2;
            
        model.CopyAbsoluteBoneTransformsTo(boneTransforms);
        foreach (ModelMesh mesh in model.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.World = boneTransforms[mesh.ParentBone.Index] * MathConverter.Convert(Transform);
                effect.View = MathConverter.Convert(cameraComponent.ViewMatrix);
                effect.Projection = MathConverter.Convert(cameraComponent.ProjectionMatrix);
            }
            mesh.Draw();
        }
    }

    public int DrawOrder { get; }
    public bool Visible { get; }
    public event EventHandler<EventArgs> DrawOrderChanged;
    public event EventHandler<EventArgs> VisibleChanged;
}