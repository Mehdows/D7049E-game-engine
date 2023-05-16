using System;
using ArenaGame.Ecs;
using ArenaGame.Ecs.Components;

namespace ArenaGame;

public class Entity
{
    public int Id { get; set; }
    
    public Entity(int id = -1)
    {
        Id = id;
    }

    public IComponent AddComponent(IComponent component = null) 
    {
        var newComponent = component;
        Type componentType = newComponent.GetType();
        var createdComponent = ComponentManager.Instance.GetComponentArray(componentType)
            .AddComponent(Id, newComponent);
        return createdComponent;
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