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
        return (ComponentArray)componentArrays[componentType];
    }

    public void Reset()
    {
        componentArrays.Clear();
    }
}