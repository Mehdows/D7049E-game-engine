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

public class RenderingSystem : ISystem
{
    private PerspectiveCameraComponent cameraComponent;
    public Entity ActiveCamera { get; set; }
    public RenderingSystem(Entity activeCamera)  
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
            
            Matrix transformMatrix;
            if(collision != null)
                 transformMatrix = collision.Transform *
                                     Matrix.CreateFromQuaternion(transform.Orientation) *
                                     Matrix.CreateTranslation(collision.CollisionEntity.WorldTransform.Translation);
            else
            {
                transformMatrix = Matrix.CreateScale(transform.Scale) *
                                     Matrix.CreateFromQuaternion(transform.Orientation) *
                                     Matrix.CreateTranslation(transform.WorldTransform.Translation);
            }

            Matrix localOffsetMatrix = Matrix.CreateTranslation(meshComponent.localOffset);
            
            Matrix worldMatrix = transformMatrix  * localOffsetMatrix;
            
            
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