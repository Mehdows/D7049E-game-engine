using System;
using System.Collections.Generic;
using System.ComponentModel;
using IComponent = ArenaGame.Ecs.Components.IComponent;

namespace ArenaGame.Ecs;

public class Entity
{
    public int Id { get; }
    
    public Entity(int id )
    {
        Id = id;
    }

    public void AddComponent<T>(IComponent component = null) where T: IComponent, new()
    {
        var newComponent = component ?? new T();
        Type componentType = typeof(T);
        ComponentManager.Instance.GetComponentArray(componentType)
            .AddComponent(Id, newComponent);
    }
        
    public void RemoveComponent<T>() where T: IComponent
    {
        Type componentType = typeof(T);
        ComponentManager.Instance.GetComponentArray(componentType)
            .RemoveComponent(Id);
    }
    
    public IComponent GetComponent<T>() where T: IComponent
    {
        Type componentType = typeof(T);
        return ComponentManager.Instance.GetComponentArray(componentType)
            .GetComponent(Id);
    }
    
    public bool HasComponent<T>() where T: IComponent
    {
        Type componentType = typeof(T);
        return ComponentManager.Instance.GetComponentArray(componentType)
            .HasComponent(Id);
    }
    
}