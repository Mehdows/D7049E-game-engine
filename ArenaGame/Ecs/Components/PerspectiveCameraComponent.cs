using System;
using ArenaGame.Ecs.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MathHelper = BEPUutilities.MathHelper;
using Matrix = BEPUutilities.Matrix;
using Vector3 = BEPUutilities.Vector3;

namespace ArenaGame;

public class PerspectiveCameraComponent : IComponent 
{
    public float FieldOfView { get; set; }
    public float AspectRatio { get; set; }
    public float NearClipPlane { get; set; }
    public float FarClipPlane { get; set; }
    
    public Matrix ViewMatrix { get; set; }
    public Matrix ProjectionMatrix { get; set; }
    public TransformComponent Target { get; set; }
    public TransformComponent Transform { get; set; }

    public PerspectiveCameraComponent(float fov, float aspectRatio, float nearClipPlane, float farClipPlane )
    {
        FieldOfView = MathHelper.ToRadians(fov);
        AspectRatio = aspectRatio;
        NearClipPlane = nearClipPlane;
        FarClipPlane = farClipPlane;
        UpdateProjectionMatrix();
    }

    public void LookAt(TransformComponent target = null)
    {
        if (target == null)
        {
            throw new Exception("Target cannot be null");
        }
        Target = target;    
        UpdateViewMatrix();
    }
    
    public override void Update(GameTime gameTime)
    {
        UpdateViewMatrix();
        UpdateProjectionMatrix();
    }

    public void UpdateViewMatrix()
    {
        ViewMatrix = Matrix.CreateLookAtRH(Transform.Position, Target.Position, Vector3.Up);
    }

    public void UpdateProjectionMatrix()
    {
        ProjectionMatrix = Matrix.CreatePerspectiveFieldOfViewRH(FieldOfView, AspectRatio, NearClipPlane, FarClipPlane);
    }
}