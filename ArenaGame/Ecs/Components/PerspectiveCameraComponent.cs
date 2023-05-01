using System;
using System.Collections.Specialized;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MathHelper = BEPUutilities.MathHelper;
using Matrix = BEPUutilities.Matrix;
using Vector3 = BEPUutilities.Vector3;

namespace ArenaGame.Ecs.Components;

public class PerspectiveCameraComponent : IComponent 
{
    public float FieldOfView { get; set; } = MathHelper.ToRadians(90f);
    public float AspectRatio { get; set; } = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.AspectRatio;
    public float NearClipPlane { get; set; } = 0.1f;
    public float FarClipPlane { get; set; } = 1000f;
    
    public Matrix ViewMatrix { get; set; }
    public Matrix ProjectionMatrix { get; set; }
    public Entity Target { get; set; }
    public TransformComponent Transform { get; set; }

    public PerspectiveCameraComponent(TransformComponent transform)
    {
        Transform = transform;
        UpdateProjectionMatrix();
    }
    public PerspectiveCameraComponent() 
    {
    }

    public void LookAt(Entity target = null)
    {
        if (target == null)
        {
            throw new Exception("Target cannot be null");
        }
        Target = target;    
        UpdateViewMatrix();
    }

    private void UpdateViewMatrix()
    {
        ViewMatrix = Matrix.CreateLookAtRH(Transform.Position, Transform.Position + Transform.Forward, -Transform.Up);
        Vector3 targetPosition = ((TransformComponent)Target.GetComponent<TransformComponent>()).Position;
        ViewMatrix = Matrix.CreateLookAtRH(Transform.Position, targetPosition, Vector3.UnitY);
    }

    public void UpdateProjectionMatrix()
    {
        ProjectionMatrix = Matrix.CreatePerspectiveFieldOfViewRH(FieldOfView, AspectRatio, NearClipPlane, FarClipPlane);
    }
}