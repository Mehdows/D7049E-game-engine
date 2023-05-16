using System;
using System.Linq;
using System.Reflection;
using ArenaGame.Ecs.Components;

namespace ArenaGame.Ecs;

using System.Collections.Generic;

public class EntityManager {
    private static EntityManager instance = null;
    private Dictionary<int, Entity> entities;
    private int nextEntityId;

    private EntityManager() {
        entities = new Dictionary<int, Entity>();
        nextEntityId = 0;
    }

    public static EntityManager Instance {
        get {
            if (instance == null) {
                instance = new EntityManager();
            }
            return instance;
        }
    }
    
    public Entity CreateEntity() {
        int entityId = nextEntityId++;
        Entity entity = new Entity(entityId);
        entities.Add(entityId, entity);
        return entity;
    }

    public Entity AddEntity(Entity entity)
    {
        int entityId = nextEntityId++;
        entity.Id = entityId;
        entities.Add(entityId, entity);
        return entity;
    }
    
    
    public Entity CreateEntityWithArchetype(Archetype archetype) {
        Entity entity = CreateEntity();
        foreach (Type componentType in archetype.ComponentTypes)
        {
            // Get the AddComponent<T>() method via reflection
            MethodInfo addComponentMethod = typeof(Entity).GetMethod("AddComponent").MakeGenericMethod(componentType);

            // Invoke the method on the entity with the new component instance as an argument
            addComponentMethod.Invoke(entity, new object[] { Activator.CreateInstance(componentType) });
        }
        return entity;
    }

    public void DestroyEntity(int entityId) {
        entities.Remove(entityId);
        ComponentManager.Instance.DestroyEntity(entityId);
    }

    public bool HasEntity(int entityId)
    {
        return entities.ContainsKey(entityId);
    }

    public Entity GetEntity(int entityId) {
        Entity entity;
        entities.TryGetValue(entityId, out entity);
        return entity;
    }
    public  List<Entity> GetEntities() {
        return entities.Values.ToList();
    }
    
    public List<Entity> GetEntitiesWithArchetype(Archetype archetype) {
        List<Entity> entitiesWithArchetype = new List<Entity>();
        foreach (var entity in entities.Values) {
            if (archetype.Matches(entity.Id)) {
                entitiesWithArchetype.Add(entity);
            }
        }
        return entitiesWithArchetype;
    }
    
    public void Reset() {
        entities.Clear();
        nextEntityId = 0;
    }
}