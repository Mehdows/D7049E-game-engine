using System;

namespace ArenaGame.Ecs.Components;

public class ComponentType
{
    private readonly Type type;

    public ComponentType(Type componentType)
    {
        type = componentType;
    }

    public Type GetType()
    {
        return type;
    }
}