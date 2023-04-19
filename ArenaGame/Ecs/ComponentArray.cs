using System.Linq;

namespace ArenaGame.Ecs;
using Components;
using System;
using System.Collections.Generic;

public class ComponentArray
{
    public Dictionary<Type, Dictionary<Entity, Component>> ComponentArrays { get; }

    
    public ComponentArray()
    {
        ComponentArrays = new Dictionary<Type, Dictionary<Entity, Component>>();
    }

    public void AddComponent(Entity entity, Component component)
    {
        Type componentType = component.GetType();

        if (!ComponentArrays.ContainsKey(componentType))
        {
            ComponentArrays[componentType] = new Dictionary<Entity, Component>();
        }

        ComponentArrays[componentType][entity] = component;
    }

    public void RemoveComponent<T>(Entity entity) where T : Component
    {
        Type componentType = typeof(T);

        if (!ComponentArrays.ContainsKey(componentType) || !ComponentArrays[componentType].ContainsKey(entity))
        {
            return;
        }

        ComponentArrays[componentType].Remove(entity);
    }

    public T GetComponent<T>(Entity entity) where T : Component
    {
        var componentType = typeof(T);

        if (!ComponentArrays.ContainsKey(componentType)) return null;
        if (ComponentArrays[componentType].ContainsKey(entity))
        {
            return (T)ComponentArrays[componentType][entity];
        }

        return null;
    }
    

    public bool HasComponent<T>(Entity entity) where T : Component
    {
        Type componentType = typeof(T);
        if (!ComponentArrays.TryGetValue(componentType, out var componentArrayForType))
        {
            return false;
        }

        return componentArrayForType.ContainsKey(entity);
    }
    
    public void RemoveEntity(Entity entity)
    {
        foreach (var componentType in ComponentArrays.Keys)
        {
            if (ComponentArrays[componentType].ContainsKey(entity))
            {
                ComponentArrays[componentType].Remove(entity);
            }
        }
    }
}