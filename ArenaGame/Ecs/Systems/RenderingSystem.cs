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
    public Entity ActiveCamera { get; set; }

    public RenderingSystem(Entity activeCamera)
    {
        ActiveCamera = activeCamera;
        if (ActiveCamera != null)
        {
            Vector3 cameraPos = ((TransformComponent)ActiveCamera.GetComponent<TransformComponent>()).Position;
            Console.Out.WriteLine($"Camera position: {cameraPos}");
            // Print out the cameras rotation
            Quaternion cameraRot = ((TransformComponent)ActiveCamera.GetComponent<TransformComponent>()).Rotation;
            Console.Out.WriteLine($"Camera rotation: {cameraRot}");
        
        }
    }

    public void Draw()
    {
        // TODO: Ask if this is the best way to do this
        var meshComponentArray = ComponentManager.Instance.GetComponentArray(typeof(MeshComponent));
        foreach (var (entityID, meshComponent) in meshComponentArray.GetEntityComponents())
        {
            var entity = EntityManager.Instance.GetEntity(entityID);
            MeshComponent mesh = (MeshComponent)meshComponent;
            TransformComponent transform = (TransformComponent)entity.GetComponent<TransformComponent>();
            if (transform != null && mesh != null)
            {
                Matrix worldMatrix = Matrix.CreateScale(transform.Scale) *
                                     Matrix.CreateFromQuaternion(transform.Rotation) *
                                     Matrix.CreateTranslation(transform.Position);
                DrawModel(mesh.Model, worldMatrix);
            }
        }
    }

    private void DrawModel(Model model, Matrix worldMatrix)
    {
        PerspectiveCameraComponent activePerspectiveCameraComponent = (PerspectiveCameraComponent)ActiveCamera.GetComponent<PerspectiveCameraComponent>();
        Matrix viewMatrix =  activePerspectiveCameraComponent.ViewMatrix;
        Matrix projectionMatrix = activePerspectiveCameraComponent.ProjectionMatrix;

        foreach (ModelMesh mesh in model.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.World = MathConverter.Convert(worldMatrix);
                effect.View = MathConverter.Convert(viewMatrix);
                effect.Projection = MathConverter.Convert(projectionMatrix);
                effect.EnableDefaultLighting();
            }

            mesh.Draw();
        }
    }

    public void Update(GameTime gameTime)
    {
            ((TransformComponent)ActiveCamera.GetComponent<TransformComponent>()).Position -= new Vector3(0.01f,0.01f,0f);
            Vector3 cameraPos = ((TransformComponent)ActiveCamera.GetComponent<TransformComponent>()).Position;
            // Console.Out.WriteLine($"Camera position: {cameraPos}");
            // Rotate the camera slowly
            Quaternion cameraRot = ((TransformComponent)ActiveCamera.GetComponent<TransformComponent>()).Rotation;
            ((TransformComponent)ActiveCamera.GetComponent<TransformComponent>()).Rotation = Quaternion.CreateFromYawPitchRoll(0.01f,0.0f,0f) * cameraRot;
            //Print out the cameras rotation
            Console.Out.WriteLine($"Camera rotation: {cameraRot}");

    }
}