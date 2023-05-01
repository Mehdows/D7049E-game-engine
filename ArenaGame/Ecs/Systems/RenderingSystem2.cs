using System;
using ArenaGame.Ecs;
using ArenaGame.Ecs.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Matrix = BEPUutilities.Matrix;
using Quaternion = BEPUutilities.Quaternion; 
using Vector3 = BEPUutilities.Vector3;

namespace ArenaGame;

public class RenderingSystem2 : DrawableGameComponent
{
    private PerspectiveCameraComponent cameraComponent;
    public RenderingSystem2() : base(Game1.Instance)
    {
         cameraComponent = (PerspectiveCameraComponent)ComponentManager.Instance.GetComponentArray(typeof(PerspectiveCameraComponent)).GetEntityComponents()[0].Item2;
    }

    public override void Draw(GameTime gameTime)
    {
        var meshComponentArray = ComponentManager.Instance.GetComponentArray(typeof(MeshComponent));
        foreach (var (entityID, component) in meshComponentArray.GetEntityComponents())
        {
            MeshComponent meshComponent = (MeshComponent)component;
            Microsoft.Xna.Framework.Matrix[] boneTransforms = new Microsoft.Xna.Framework.Matrix[meshComponent.Model.Bones.Count];
            meshComponent.Model.CopyAbsoluteBoneTransformsTo(boneTransforms);
            foreach (ModelMesh mesh in meshComponent.Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = boneTransforms[mesh.ParentBone.Index] *
                                   MathConverter.Convert(meshComponent.Transform);
                    effect.View = MathConverter.Convert(cameraComponent.ViewMatrix);
                    effect.Projection = MathConverter.Convert(cameraComponent.ProjectionMatrix);
                }
                mesh.Draw();
            }
            base.Draw(gameTime);
        }
    }
}