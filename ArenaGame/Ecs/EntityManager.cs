using System;
using System.Linq;
using ArenaGame.Ecs.Components;

namespace ArenaGame.Ecs;

using System.Collections.Generic;

public class EntityManager
{
    private const int MAX_ENTITIES = 100;
    private int nextEntityId = 1;
    // List of all entities in the game
    public List<Entity> Entities { get; private set; }

    // Component array that stores all components in the game
    public ComponentArray ComponentArray { get; private set; }

    // List of archetypes for efficient component queries
    public List<Archetype> Archetypes { get; private set; }

    public EntityManager()
    {
        Entities = new List<Entity>();
        ComponentArray = new ComponentArray();
        Archetypes = new List<Archetype>();
    }

    // Create a new entity and add it to the entity list
    public Entity CreateEntity()
    {
        var entity = new Entity(nextEntityId++, this);
        Entities.Add(entity);
        if (nextEntityId > MAX_ENTITIES) {
            nextEntityId = 1;
        }
        return entity;
    }

    // Destroy an entity and remove it from the entity list
    public void DestroyEntity(Entity entity)
    {
        if (Entities.Contains(entity))
        Entities.Remove(entity);
        ComponentArray.RemoveEntity(entity);
    }

    // Add a component to an entity
    public void AddComponent(Entity entity, Component component)
    {
        ComponentArray.AddComponent(entity, component);
        UpdateArchetypes(entity, component);
    }

    // Remove a component from an entity
    public void RemoveComponent<T>(Entity entity) where T : Component
    {
        Type componentType = typeof(T);
        Component component = ComponentArray.ComponentArrays[componentType][entity];
        ComponentArray.RemoveComponent<T>(entity);
        UpdateArchetypes(entity, component);
    }

    // Get a component from an entity
    public T GetComponent<T>(Entity entity) where T : Component
    {
        return ComponentArray.GetComponent<T>(entity);
    }
    
    public bool HasComponent<T>(Entity entity) where T : Component
    {
        return ComponentArray.HasComponent<T>(entity);
    }

    // Update the archetypes after adding or removing a component
    private void UpdateArchetypes(Entity entity, Component component)
    {
        foreach (var archetype in Archetypes)
        {
            archetype.UpdateArchetype(entity, component);
        }
    }
}