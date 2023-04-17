using System;
using System.Collections.Generic;

namespace ArenaGame.Ecs;

public class Entity
{
    private Dictionary<Type, object> components = new Dictionary<Type, object>();
    
    public void AddComponent<T>(T component) where T : class 
    {
        components[typeof(T)] = component;
    }
    
    public T GetComponent<T>() where T : class 
    {
        if (components.TryGetValue(typeof(T), out object component)) 
        {
            return component as T;
        }
        return null;
    }
    
    public bool HasComponent<T>() 
    {
        return components.ContainsKey(typeof(T));
    } 
}