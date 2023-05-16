using System;
using System.Collections.Generic;
using ArenaGame.Ecs.Components;

namespace ArenaGame.Ecs.Archetypes;

public class ArchetypeFactory
{
    private static readonly Dictionary<EArchetype, Archetype> archetypeInstances = new Dictionary<EArchetype, Archetype>();

    private ArchetypeFactory()
    {
    }

    public static Archetype GetArchetype(EArchetype archetypeType)
    {
        if (!archetypeInstances.ContainsKey(archetypeType))
        {
            archetypeInstances.Add(archetypeType, CreateArchetype(archetypeType));
        }

        return archetypeInstances[archetypeType];
    }

    private static Archetype CreateArchetype(EArchetype archetype)
    {
        switch (archetype)
        {
            case EArchetype.Enemy:
                return new EnemyArchetype(new[]
                {
                    ComponentManager.Instance.GetComponentArray(typeof(PositionComponent)).GetComponentType(),
                    ComponentManager.Instance.GetComponentArray(typeof(VelocityComponent)).GetComponentType()
                });
            case EArchetype.Player:
                return new PlayerArchetype(new[]
                {
                    ComponentManager.Instance.GetComponentArray(typeof(InputComponent)).GetComponentType(),
                    ComponentManager.Instance.GetComponentArray(typeof(TransformComponent)).GetComponentType(), // Can't define MeshComponent here since it can't have empty constructor
                });
            case EArchetype.Weapon:
                return new WeaponArchetype(new[]
                {
                    ComponentManager.Instance.GetComponentArray(typeof(TransformComponent)).GetComponentType()
                });
            case EArchetype.Spawner:
                return new SpawnerArchetype(new[]
                {
                    ComponentManager.Instance.GetComponentArray(typeof(TransformComponent)).GetComponentType(),
                    ComponentManager.Instance.GetComponentArray(typeof(SpawnerComponent)).GetComponentType()
                });


            default:
                throw new ArgumentException("Invalid archetype type.");
        }
    }
}