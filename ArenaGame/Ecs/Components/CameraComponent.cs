using BEPUutilities;
using Microsoft.Xna.Framework.Graphics;

namespace ArenaGame.Ecs.Components;

public class CameraComponent : IComponent
{
    public Matrix ViewMatrix { get; private set; }
    public Matrix ProjectionMatrix { get; private set; }
    
    public Matrix WorldMatrix { get; private set; }

    public TransformComponent Transform { get; set; }
    public float FieldOfView { get; set; } = MathHelper.ToRadians(45f);
    public float AspectRatio { get; set; } = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.AspectRatio;
    public float NearClipPlane { get; set; } = 0.1f;
    public float FarClipPlane { get; set; } = 1000f;

    public CameraComponent(TransformComponent transform)
    {
        Transform = transform;
        UpdateProjectionMatrix();
    }

    public void LookAt(Vector3 target)
    {
        Transform.LookAt(target);
        UpdateViewMatrix();
    }

    private void UpdateViewMatrix()
    {
        
        
        ViewMatrix = Matrix.CreateLookAtRH(Transform.Position, Transform.Position + Transform.Forward, Transform.Up);
    }

    public void UpdateProjectionMatrix()
    {
        ProjectionMatrix = Matrix.CreatePerspectiveFieldOfViewRH(FieldOfView, AspectRatio, NearClipPlane, FarClipPlane);
    }
    
    public void UpdateWorldMatrix()
    {
        // Top down world matrix
        WorldMatrix = Matrix.CreateFromAxisAngle(Vector3.Right, MathHelper.ToRadians(90f)) * Matrix.CreateTranslation(Transform.Position);
    }
}