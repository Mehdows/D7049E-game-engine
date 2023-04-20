using System;
using System.Linq;
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

    public void DestroyEntity(int entityId) {
        entities.Remove(entityId);
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