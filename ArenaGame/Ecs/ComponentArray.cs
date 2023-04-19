using System.Linq;

namespace ArenaGame.Ecs;
using Components;
using System;
using System.Collections.Generic;

public class ComponentArray {
    private IComponent[] components;
    private int[] entityIds;
    private Type componentType;

    public ComponentArray(Type componentType) {
        components = Array.Empty<IComponent>();
        entityIds = Array.Empty<int>();
        this.componentType = componentType;
    }

    public void AddComponent(int entityId, IComponent component) {
        // Expand arrays
        int index = components.Length;
        Array.Resize(ref components, index + 1);
        Array.Resize(ref entityIds, index + 1);

        // Add new component and entity ID
        components[index] = component;
        entityIds[index] = entityId;
    }

    public void RemoveComponent(int entityId) {
        for (int i = 0; i < components.Length; i++) {
            if (entityIds[i] == entityId) {
                // Remove component and entity ID at index i
                components[i] = null;
                entityIds[i] = -1;
            }
        }
    }

    public object GetComponent(int entityId) {
        for (int i = 0; i < components.Length; i++) {
            if (entityIds[i] == entityId) {
                return components[i];
            }
        }
        return null;
    }
    
    public bool HasComponent(int entityId) {
        for (int i = 0; i < entityIds.Length; i++) {
            if (entityIds[i] == entityId) {
                return true;
            }
        }
        return false;
    }

    public Type GetComponentType() {
        return componentType;
    }
    

}







