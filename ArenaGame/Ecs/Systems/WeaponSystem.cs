using System;
using ArenaGame.Ecs.Archetypes;
using ArenaGame.Ecs.Components;
using Microsoft.Xna.Framework;

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
            Player3DArchetype player3DArchetype = (Player3DArchetype)ArchetypeFactory.GetArchetype(EArchetype.Player3D);
            Entity player3D = EntityManager.Instance.GetEntitiesWithArchetype(player3DArchetype)[0]; // 0 is always the player...
            playerTransform = (TransformComponent)player3D.GetComponent<TransformComponent>();

            WeaponArchetype weaponArchetype = (WeaponArchetype)ArchetypeFactory.GetArchetype(EArchetype.Weapon);
            Entity sword = EntityManager.Instance.GetEntitiesWithArchetype(weaponArchetype)[1]; // ... and 1 is always the weapon???
            swordTransform = (TransformComponent)sword.GetComponent<TransformComponent>();
        }

        public void Update(GameTime gameTime)
        {
            // Rotate sword in the player's direction
            swordTransform.Position = playerTransform.Position + Vector3.Transform(new Vector3(0f, offset, -30f), playerTransform.Rotation);
            swordTransform.Rotation = playerTransform.Rotation;

            float time = (float)gameTime.TotalGameTime.TotalSeconds;

            // Adds sword movement from side to side
            /*
            Vector3 right = Vector3.Transform(Vector3.Right, swordTransform.Rotation);
            float sideOffset = offset * (float)Math.Sin(time * speed);
            swordTransform.Position += right * sideOffset;
            */

            // Rotate sword around it's own axis
            swordTransform.Rotation *= Quaternion.CreateFromYawPitchRoll(speed * time, 0f, 0f);
        }
    }
}
