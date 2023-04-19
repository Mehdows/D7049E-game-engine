using System.Linq;
using System.Reflection;
using ArenaGame.Ecs.Components;

namespace ArenaGame.Ecs;
using System;
using System.Collections.Generic;

public class Archetype
{
    private ComponentArray ComponentArray { get; }
    private readonly Type[] _componentTypes;

    public Archetype(ComponentArray componentArray, params Type[] componentTypes)
    {
        ComponentArray = componentArray;
        _componentTypes = componentTypes;
    }

    public void AddEntity(Entity entity, params Component[] components)
    {
        if (!components.Equals(_componentTypes)) return;
        foreach (var component in components)
        {
            ComponentArray.AddComponent(entity, component);
        }
    }

    public void RemoveEntity(Entity entity)
    {
        ComponentArray.RemoveEntity(entity);
    }

    public bool HasEntity(Entity entity)
    {
        if (_componentTypes.Length < 1) return false;
        foreach (var componentType in _componentTypes)
        {
            try
            {
                if (!ComponentArray.ComponentArrays[componentType].ContainsKey(entity))
                {
                    return false;
                }
            }
            catch (KeyNotFoundException e)
            {
                return false;
            }
        }
        return true;
    }

    public bool HasComponents(params Type[] componentTypes)
    {
        foreach (var componentType in componentTypes)
        {
            if (!ComponentArray.ComponentArrays.ContainsKey(componentType))
            {
                return false;
            }
        }

        return true;
    }

    public IEnumerable<Entity> GetEntities()
    {
        // Find the entities that have all the specified components
        IEnumerable<Entity> entities = null;
        foreach (var componentType in _componentTypes)
        {
            if (!ComponentArray.ComponentArrays.ContainsKey(componentType))
            {
                return Enumerable.Empty<Entity>();
            }

            if (entities == null)
            {
                entities = ComponentArray.ComponentArrays[componentType].Keys;
            }
            else
            {
                entities = entities.Intersect(ComponentArray.ComponentArrays[componentType].Keys);
            }
        }

        // Filter out entities that don't have all the specified components
        if (entities != null)
        {
            entities = entities.Where(entity =>
                _componentTypes.All(componentType => ComponentArray.ComponentArrays[componentType].ContainsKey(entity)));
        }

        return entities ?? Enumerable.Empty<Entity>();
    }

    public IEnumerable<Entity> GetEntitiesWithComponents(params Type[] componentTypes)
    {
        var entities = GetEntities();

        foreach (var componentType in componentTypes)
        {
            entities = entities.Where(entity => ComponentArray.ComponentArrays[componentType].ContainsKey(entity));
        }

        return entities;
    }

    public void UpdateArchetype(Entity entity, Component component)
    {
        var componentType = component.GetType();
        var entityHasComponent = _componentTypes.Contains(componentType) && 
                                 ComponentArray.ComponentArrays.ContainsKey(componentType) &&
                                 ComponentArray.ComponentArrays[componentType].ContainsKey(entity);

        if (!entityHasComponent)
        {
            return;
        }

        ComponentArray.AddComponent(entity, component);
    }
}