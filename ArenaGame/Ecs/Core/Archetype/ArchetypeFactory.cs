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
            case EArchetype.Player:
                return new PlayerArchetype(new[]
                {
                    ComponentManager.Instance.GetComponentArray(typeof(PositionComponent)).GetComponentType(),
                    ComponentManager.Instance.GetComponentArray(typeof(VelocityComponent)).GetComponentType(),
                    ComponentManager.Instance.GetComponentArray(typeof(InputComponent)).GetComponentType()
                });
        
            case EArchetype.Enemy:
                return new EnemyArchetype(new[]
                {
                    ComponentManager.Instance.GetComponentArray(typeof(PositionComponent)).GetComponentType(),
                    ComponentManager.Instance.GetComponentArray(typeof(VelocityComponent)).GetComponentType()
                });
            // TODO: Lägg till flera archetyper här


            default:
                throw new ArgumentException("Invalid archetype type.");
        }
    }
}