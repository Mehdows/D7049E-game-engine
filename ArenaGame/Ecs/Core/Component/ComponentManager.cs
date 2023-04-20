using System;
using System.Collections.Generic;
using System.ComponentModel;
using ArenaGame.Ecs.Components;

namespace ArenaGame.Ecs;

public class ComponentManager {
    private Dictionary<Type, ComponentArray> componentArrays = new Dictionary<Type, ComponentArray>();

    private static ComponentManager instance;
    public static ComponentManager Instance {
        get {
            if (instance == null) {
                instance = new ComponentManager();
            }
            return instance;
        }
    }

    public void RegisterComponent(Type componentType) {
        ComponentArray componentArray = new ComponentArray(componentType);
        componentArrays.Add(componentType, componentArray);
    }

    public ComponentArray GetComponentArray(Type componentType) {
        if (!componentArrays.ContainsKey(componentType)) {
            RegisterComponent(componentType);
        }

        return componentArrays[componentType];
    }
    
    public void DestroyEntity(int entityId)
    {
        foreach (ComponentArray componentArray in componentArrays.Values)
        {
            componentArray.RemoveComponent(entityId);
        }
    }

    public void Reset()
    {
        componentArrays.Clear();
    }
}