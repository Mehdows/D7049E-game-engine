
using ArenaGame.Ecs.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Matrix = BEPUutilities.Matrix;

namespace ArenaGame.Ecs;

/// <summary>
/// Component that draws a model following the position and orientation of a BEPUphysics entity.
/// </summary>
public class EntityModel : DrawableGameComponent
{
    /// <summary>
    /// Entity that this model follows.
    /// </summary>
    BEPUphysics.Entities.Entity entity;
    Model model;
    /// <summary>
    /// Base transformation to apply to the model.
    /// </summary>
    public Matrix Transform;
    Matrix[] boneTransforms;


    /// <summary>
    /// Creates a new EntityModel.
    /// </summary>
    /// <param name="entity">Entity to attach the graphical representation to.</param>
    /// <param name="model">Graphical representation to use for the entity.</param>
    /// <param name="transform">Base transformation to apply to the model before moving to the entity.</param>
    /// <param name="game">Game to which this component will belong.</param>
    public EntityModel(BEPUphysics.Entities.Entity entity, Model model, Matrix transform, Game game)
        : base(game)
    {
        this.entity = entity;
        this.model = model;
        this.Transform = transform;

        //Collect any bone transformations in the model itself.
        //The default cube model doesn't have any, but this allows the EntityModel to work with more complicated shapes.
        boneTransforms = new Matrix[model.Bones.Count];
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
        Matrix worldMatrix = Transform * entity.WorldTransform;


        Microsoft.Xna.Framework.Matrix[] convertedBoneTransforms = new Microsoft.Xna.Framework.Matrix[model.Bones.Count];
        model.CopyAbsoluteBoneTransformsTo(convertedBoneTransforms);
        foreach (ModelMesh mesh in model.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.World = convertedBoneTransforms[mesh.ParentBone.Index] * MathConverter.Convert(worldMatrix);
                effect.View = MathConverter.Convert(cameraComponent.ViewMatrix);
                effect.Projection = MathConverter.Convert(cameraComponent.ProjectionMatrix);
            }
            mesh.Draw();
        }
        base.Draw(gameTime);
    }
}
