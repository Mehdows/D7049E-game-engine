using System;
using ArenaGame.Ecs.Components;

namespace ArenaGame.Ecs;

public class Archetype {
    public Type[] ComponentTypes { get; }

    public Archetype(Type[] componentTypes) {
        this.ComponentTypes = componentTypes;
    }

    public bool Matches(int entityId) {
        foreach (Type componentType in ComponentTypes) {
            ComponentArray componentArray = ComponentManager.Instance.GetComponentArray(componentType);
            if (!componentArray.HasComponent(entityId)) {
                return false;
            }
        }
        return true;
    }

    public ComponentArray GetComponentArray(Type componentType) {
        // Return the ComponentArray of the specified ComponentType, if present
        foreach (Type type in ComponentTypes) {
            if (type == componentType) {
                return ComponentManager.Instance.GetComponentArray(componentType);
            }
        }

        // If the specified ComponentType is not present in this Archetype, return null
        return null;
    }
    
}