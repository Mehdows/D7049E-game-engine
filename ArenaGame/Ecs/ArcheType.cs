using System.Collections.Generic;
using ArenaGame.Ecs.Components;

namespace ArenaGame.Ecs;
using System;
using System.Collections.Generic;

public class Archetype
{
    public List<Entity> Entities { get; private set; }
    private Dictionary<Type, Dictionary<Entity, Component>> componentArrays;

    public Archetype(ComponentArray componentArray)
    {
        Entities = new List<Entity>();
        componentArrays = new Dictionary<Type, Dictionary<Entity, Component>>();

        foreach (Type type in componentArray.ComponentArrays.Keys)
        {
            componentArrays.Add(type, new Dictionary<Entity, Component>());
        }
    }

    public bool Matches(ComponentArray componentArray)
    {
        foreach (Type type in componentArrays.Keys)
        {
            if (!componentArray.ComponentArrays.ContainsKey(type))
            {
                return false;
            }
        }
        return true;
    }

    public void Add(Entity entity, ComponentArray componentArray)
    {
        Entities.Add(entity);

        foreach (Type type in componentArrays.Keys)
        {
            Dictionary<Entity, Component> componentArrayForType = componentArrays[type];
            if (componentArray.HasComponent<Component>(entity))
            {
                Component component = componentArray.GetComponent<Component>(entity);
                componentArrayForType.Add(entity, component);
            }
        }
    }

    public void UpdateArchetype(Entity entity, ComponentArray componentArray)
    {
        // Check if the entity still matches this archetype
        if (!Matches(componentArray))
        {
            Entities.Remove(entity);
            foreach (Type type in componentArrays.Keys)
            {
                Dictionary<Entity, Component> componentArrayForType = componentArrays[type];
                if (componentArrayForType.ContainsKey(entity))
                {
                    componentArrayForType.Remove(entity);
                }
            }
            return;
        }

        // Add any new components for this entity
        foreach (Type type in componentArrays.Keys)
        {
            Dictionary<Entity, Component> componentArrayForType = componentArrays[type];
            if (!componentArrayForType.ContainsKey(entity) && componentArray.HasComponent<Component>(entity))
            {
                Component component = componentArray.GetComponent<Component>(entity);
                componentArrayForType.Add(entity, component);
            }
        }

        // Remove any components that were removed from this entity
        foreach (Type type in componentArrays.Keys)
        {
            Dictionary<Entity, Component> componentArrayForType = componentArrays[type];
            if (componentArrayForType.ContainsKey(entity) && !componentArray.HasComponent<Component>(entity))
            {
                componentArrayForType.Remove(entity);
            }
        }
    }
}