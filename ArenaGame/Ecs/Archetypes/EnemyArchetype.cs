using System;

namespace ArenaGame.Ecs.Archetypes;

public class EnemyArchetype: Archetype
{
    public EnemyArchetype(Type[] componentTypes) : base(componentTypes)
    {
    }
}