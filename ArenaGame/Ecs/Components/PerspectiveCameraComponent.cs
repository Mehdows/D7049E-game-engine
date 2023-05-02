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
    public float FieldOfView { get; set; } = MathHelper.ToRadians(45f);
    public float AspectRatio { get; set; } = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.AspectRatio;
    public float NearClipPlane { get; set; } = 0.1f;
    public float FarClipPlane { get; set; } = 1000f;
    
    public Matrix ViewMatrix { get; set; }
    public Matrix ProjectionMatrix { get; set; }
    public TransformComponent Target { get; set; }
    public TransformComponent Transform { get; set; }

    public PerspectiveCameraComponent(TransformComponent transform)
    {
        Transform = transform;
        UpdateProjectionMatrix();
    }
    public PerspectiveCameraComponent() 
    {
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
        // Vector3 targetPosition = ((TransformComponent)Target.GetComponent<TransformComponent>()).Position;
        //ViewMatrix = Matrix.CreateLookAtRH(new Vector3(0,100,150), new Vector3(0,0,0), Transform.Up);
    }

    public void UpdateProjectionMatrix()
    {
        ProjectionMatrix = Matrix.CreatePerspectiveFieldOfViewRH(FieldOfView, AspectRatio, NearClipPlane, FarClipPlane);
    }
}