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

    public void AddComponent<T>() where T: IComponent, new()
    {
        Type componentType = typeof(T);
        ComponentManager.Instance.GetComponentArray(componentType)
            .AddComponent(Id, new T());
        
    }
    
}