using System;
using ArenaGame.Ecs;
using ArenaGame.Ecs.Components;
using ArenaGame.Ecs.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Matrix = BEPUutilities.Matrix;
using Quaternion = BEPUutilities.Quaternion; 
using Vector3 = BEPUutilities.Vector3;

namespace ArenaGame;

public class RenderingSystem2 : ISystem
{
    private PerspectiveCameraComponent cameraComponent;
    public Entity ActiveCamera { get; set; }
    public RenderingSystem2(Entity activeCamera)  
    {
        ActiveCamera = activeCamera;
        cameraComponent = (PerspectiveCameraComponent)activeCamera.GetComponent<PerspectiveCameraComponent>();
        
    }

    public void Draw(GameTime gameTime)
    {
        var meshComponentArray = ComponentManager.Instance.GetComponentArray(typeof(MeshComponent));
        foreach (var (entityID, component) in meshComponentArray.GetEntityComponents())
        {
            var entity = EntityManager.Instance.GetEntity(entityID);
            var collision = (CollisionComponent)entity.GetComponent<CollisionComponent>();
            MeshComponent meshComponent = (MeshComponent)component;
            
            TransformComponent transform = (TransformComponent)entity.GetComponent<TransformComponent>();
            
            Matrix worldMatrix;
            if(collision != null)
                 worldMatrix = collision.Transform *
                                     Matrix.CreateFromQuaternion(transform.Orientation) *
                                     Matrix.CreateTranslation(collision.CollisionEntity.WorldTransform.Translation);
            else
            {
                 worldMatrix = Matrix.CreateScale(transform.Scale) *
                                     Matrix.CreateFromQuaternion(transform.Orientation) *
                                     Matrix.CreateTranslation(transform.WorldTransform.Translation);
            }
            foreach (ModelMesh mesh in meshComponent.Model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = MathConverter.Convert(worldMatrix);
                    effect.View = MathConverter.Convert(cameraComponent.ViewMatrix);
                    effect.Projection = MathConverter.Convert(cameraComponent.ProjectionMatrix);
                }
                mesh.Draw();
            }
        }
    }

    public void Update(GameTime gameTime)
    {
        
    }
}