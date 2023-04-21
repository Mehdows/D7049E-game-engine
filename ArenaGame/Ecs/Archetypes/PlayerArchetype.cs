using System;

namespace ArenaGame.Ecs.Archetypes;

public class PlayerArchetype : Archetype
{
    public PlayerArchetype(Type[] componentTypes = null) : base(componentTypes)
    {
    }
}