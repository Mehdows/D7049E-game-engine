using System;
using ArenaGame.Ecs.Archetypes;
using ArenaGame.Ecs.Components;
using Microsoft.Xna.Framework;
using Quaternion = BEPUutilities.Quaternion;
using Vector3 = BEPUutilities.Vector3;

namespace ArenaGame.Ecs.Systems
{
    public class WeaponSystem : ISystem
    {
        private float offset = 15f;
        private float speed = 2f;

        private TransformComponent playerTransform;
        private TransformComponent swordTransform;

        public WeaponSystem()
        {
            PlayerArchetype playerArchetype = (PlayerArchetype)ArchetypeFactory.GetArchetype(EArchetype.Player);
            Entity player = EntityManager.Instance.GetEntitiesWithArchetype(playerArchetype)[0]; // 0 is always the player...
            playerTransform = (TransformComponent)player.GetComponent<TransformComponent>();

            WeaponArchetype weaponArchetype = (WeaponArchetype)ArchetypeFactory.GetArchetype(EArchetype.Weapon);
            Entity sword = EntityManager.Instance.GetEntitiesWithArchetype(weaponArchetype)[1]; // ... and 1 is always the weapon???
            swordTransform = (TransformComponent)sword.GetComponent<TransformComponent>();
        }

        public void Update(GameTime gameTime)
        {
            // Rotate sword in the player's direction
            swordTransform.Position = playerTransform.Position + new Vector3(0f, offset, -30f);
            swordTransform.orientation = playerTransform.orientation;

            float time = (float)gameTime.TotalGameTime.TotalSeconds;

            // Adds sword movement from side to side
            /*
            Vector3 right = Vector3.Transform(Vector3.Right, swordTransform.Rotation);
            float sideOffset = offset * (float)Math.Sin(time * speed);
            swordTransform.Position += right * sideOffset;
            */

            // Rotate sword around it's own axis
            swordTransform.orientation *= Quaternion.CreateFromYawPitchRoll(speed * time, 0f, 0f);
        }
    }
}
