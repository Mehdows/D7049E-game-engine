using System;
using System.Collections.Generic;
using System.Linq;
using ArenaGame.Ecs;
using ArenaGame.Ecs.Components;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUutilities;

namespace ArenaGame;

public class EntityBuilder
{
    private Entity entity;
    private List<Type> componentTypes = new List<Type>();
    
    public EntityBuilder()
    {
        entity = new Entity();
    }
    
    public EntityBuilder AddTransformComponent(float x, float y, float z) {
        TransformComponent transformComponent = new TransformComponent(x, y, z);
        entity.AddComponent(transformComponent);
        componentTypes.Add(typeof(TransformComponent));
        return this;
    }
    public EntityBuilder AddTransformComponent() {
        TransformComponent transformComponent = new TransformComponent();
        entity.AddComponent(transformComponent);
        componentTypes.Add(typeof(TransformComponent));
        return this;
    }
    
    public EntityBuilder AddMeshComponent(string meshName) {
        MeshComponent meshComponent = new MeshComponent(meshName);
        entity.AddComponent(meshComponent);
        componentTypes.Add(typeof(MeshComponent));
        return this;
    }
    
    public EntityBuilder AddCollisionComponent(Vector3 startPosition,ConvexShape shape, Vector3 transformScale, string tag) {
        CollisionComponent collisionComponent = new CollisionComponent(startPosition, shape, transformScale, tag);
        entity.AddComponent(collisionComponent);
        componentTypes.Add(typeof(CollisionComponent));
        return this;
    }
    
    public EntityBuilder AddInputComponent() {
        InputComponent inputComponent = new InputComponent();
        entity.AddComponent(inputComponent);
        componentTypes.Add(typeof(InputComponent));
        return this;
    }
    
    public EntityBuilder AddPerspectiveCameraComponent(float fov, float aspect, float near, float far) {
        PerspectiveCameraComponent perspectiveCameraComponent = new PerspectiveCameraComponent(fov, aspect, near, far);
        entity.AddComponent(perspectiveCameraComponent);
        componentTypes.Add(typeof(PerspectiveCameraComponent));
        return this;
    }
    
    public EntityBuilder AddAIControllerComponent(EnemyType enemyType) {
        AIControllerComponent aiControllerComponent = new AIControllerComponent(enemyType);
        entity.AddComponent(aiControllerComponent);
        componentTypes.Add(typeof(AIControllerComponent));
        return this;
    }

    public EntityBuilder AddComponents(IEnumerable<IComponent> components) {
        foreach (var component in components) {
            entity.AddComponent(component);
        }
        return this;
    }

    public Entity Build() {
        // Add the entity to the EntityManager
        Entity createdEntity = EntityManager.Instance.AddEntity(entity);
        // For each type of components go trough the component manager and modify the id to match the created entity
        foreach (var componentType in componentTypes) {
            ComponentManager.Instance.GetComponentArray(componentType).ModifyComponentId(-1, createdEntity.Id);
        }
        
        return createdEntity;
    }
}