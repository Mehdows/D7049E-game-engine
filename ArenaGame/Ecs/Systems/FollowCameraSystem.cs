using System;
using ArenaGame.Ecs;
using ArenaGame.Ecs.Components;
using ArenaGame.Ecs.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector3 = BEPUutilities.Vector3;

namespace ArenaGame;

public class FollowCameraSystem: ISystem
{
    private Entity followTargetEntity;
    private Entity camera;
    private Vector3 offset = new (0f,400f,-100f);
    private float lagTime = 0.2f;
    
    public FollowCameraSystem(Entity followTargetEntity, Entity camera)
    {
        this.followTargetEntity = followTargetEntity;
        this.camera = camera;
    }
    
    public void Update(GameTime gameTime)
    {
        if (followTargetEntity == null) return;
        
        if (followTargetEntity.GetComponent<TransformComponent>() == null ) return;
        Vector3 targetCurrentPosition = ((TransformComponent)followTargetEntity.GetComponent<TransformComponent>()).Position;

        // Calculate the camera position
        Vector3 cameraTarget = targetCurrentPosition + offset;
        Vector3 cameraPosition = ((PerspectiveCameraComponent)camera.GetComponent<PerspectiveCameraComponent>()).Transform.Position;

        // Move the camera slightly towards the target (with lag)
        cameraPosition += (cameraTarget - cameraPosition) * lagTime;

        // Print out the camera position
        // Console.WriteLine($"Camera position: {cameraPosition}");

        // Update the camera position
        ((PerspectiveCameraComponent)camera.GetComponent<PerspectiveCameraComponent>()).Transform.Position = cameraPosition;
        ((PerspectiveCameraComponent)camera.GetComponent<PerspectiveCameraComponent>()).UpdateViewMatrix();
    }
}