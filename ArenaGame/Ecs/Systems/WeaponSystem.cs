using System;
using ArenaGame.Ecs.Archetypes;
using ArenaGame.Ecs.Components;
using Microsoft.Xna.Framework;
using Matrix = BEPUutilities.Matrix;
using Vector3 = BEPUutilities.Vector3;
using Quaternion = BEPUutilities.Quaternion;

namespace ArenaGame.Ecs.Systems
{
    public class WeaponSystem : ISystem
    {
        private CollisionComponent playerCollision;

        private WeaponComponent sword;
        private TransformComponent swordTransform;
        private CollisionComponent swordCollision;
        private MeshComponent swordMesh;

        public WeaponSystem()
        {
            var playerID = ComponentManager.Instance.GetComponentArray(typeof(InputComponent)).GetEntityComponents()[0].Item1;
            playerCollision = (CollisionComponent) EntityManager.Instance.GetEntity(playerID).GetComponent<CollisionComponent>();
            
            sword = (WeaponComponent) EntityManager.Instance.GetEntity(playerID).GetComponent<WeaponComponent>();
            swordTransform =(TransformComponent) sword.WeaponEntity.GetComponent<TransformComponent>();
            swordCollision =(CollisionComponent) sword.WeaponEntity.GetComponent<CollisionComponent>();
            swordMesh =(MeshComponent) sword.WeaponEntity.GetComponent<MeshComponent>();
        }

        public void Update(GameTime gameTime)
        {
            
            float swordOffset = -10f;
            float time = (float)gameTime.TotalGameTime.TotalSeconds;

            float angle = sword.Speed * time;

            float xPos = playerCollision.CollisionEntity.Position.X + (float)Math.Cos(angle) * sword.Radius;
            float zPos = playerCollision.CollisionEntity.Position.Z + (float)Math.Sin(angle) * sword.Radius;
            Vector3 newPosition = new Vector3(xPos, playerCollision.CollisionEntity.Position.Y, zPos);

            Vector3 bottomCenterOffset = new Vector3(0f, -1f, 0f);
            Vector3 adjustedPosition = newPosition - bottomCenterOffset;
            Vector3 forward = -Vector3.Normalize(playerCollision.CollisionEntity.Position - newPosition);
            Matrix rotationMatrix = Matrix.CreateWorldRH(newPosition, forward, Vector3.Up);
            Quaternion orientation = Quaternion.CreateFromRotationMatrix(rotationMatrix);

            swordCollision.CollisionEntity.Position = adjustedPosition;
            swordCollision.CollisionEntity.Orientation = orientation;
            swordTransform.WorldTransform = swordCollision.CollisionEntity.WorldTransform;
            swordMesh.localOffset = new Vector3((float)Math.Cos(angle) * swordOffset, 0, (float)Math.Sin(angle) * swordOffset);
        }
    }
}