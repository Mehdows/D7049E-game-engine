using System;
using System.Collections.Generic;
using ArenaGame.Ecs.Components;

namespace ArenaGame.Ecs;

public class Entity
{
    public int Id { get; }
    private readonly EntityManager _entityManager;
    
    public Entity(int id, EntityManager entityManager)
    {
        Id = id;
        _entityManager = entityManager;
    }

    public void AddComponent(Component component)
    {
        _entityManager.AddComponent(this, component);
    }

    public void RemoveComponent<T>() where T : Component
    {
        _entityManager.RemoveComponent<T>(this);
    }

    public T GetComponent<T>() where T : Component
    {
        return _entityManager.GetComponent<T>(this);
    }
}