using System;
using System.Collections.Generic;
using ArenaGame.Ecs.Components;
using NUnit.Framework;

namespace ArenaGame.Ecs.Tests;

[TestFixture]
[TestFixture]
public class EntityManagerTests
{
    [SetUp]
    public void SetUp()
    {
        // Ensure EntityManager is reset before each test
        EntityManager.Instance.Reset();
        ComponentManager.Instance.Reset();
    }

    [Test]
    public void CreateEntity_NewEntityHasCorrectId()
    {
        // Arrange
        EntityManager entityManager = EntityManager.Instance;

        // Act
        Entity entity = entityManager.CreateEntity();

        // Assert
        Assert.AreEqual(0, entity.Id);
    }

    [Test]
    public void CreateEntity_CreatesMultipleEntitiesWithUniqueIds()
    {
        // Arrange
        EntityManager entityManager = EntityManager.Instance;

        // Act
        Entity entity1 = entityManager.CreateEntity();
        Entity entity2 = entityManager.CreateEntity();
        Entity entity3 = entityManager.CreateEntity();

        // Assert
        Assert.AreEqual(0, entity1.Id);
        Assert.AreEqual(1, entity2.Id);
        Assert.AreEqual(2, entity3.Id);
    }

    [Test]
    public void DestroyEntity_RemovesEntityFromEntityManager()
    {
        // Arrange
        EntityManager entityManager = EntityManager.Instance;
        Entity entity = entityManager.CreateEntity();

        // Act
        entityManager.DestroyEntity(entity.Id);

        // Assert
        Assert.IsFalse(entityManager.HasEntity(entity.Id));
    }

    [Test]
    public void GetEntity_ReturnsEntityWithMatchingId()
    {
        // Arrange
        EntityManager entityManager = EntityManager.Instance;
        Entity entity1 = entityManager.CreateEntity();
        Entity entity2 = entityManager.CreateEntity();

        // Act
        Entity retrievedEntity = entityManager.GetEntity(entity1.Id);

        // Assert
        Assert.AreEqual(entity1, retrievedEntity);
    }

    [Test]
    public void GetEntity_ReturnsNullForNonexistentEntity()
    {
        // Arrange
        EntityManager entityManager = EntityManager.Instance;

        // Act
        Entity retrievedEntity = entityManager.GetEntity(123);

        // Assert
        Assert.IsNull(retrievedEntity);
    }
    
    [Test]
    public void CreateEntity_CreatesNewEntity() {
        Entity entity = EntityManager.Instance.CreateEntity();
        Assert.IsNotNull(entity);
    }

    [Test]
    public void DestroyEntity_RemovesEntity() {
        Entity entity = EntityManager.Instance.CreateEntity();
        int entityId = entity.Id;

        EntityManager.Instance.DestroyEntity(entityId);
        Assert.IsFalse(EntityManager.Instance.HasEntity(entityId));
    }
    
    [Test]
    public void AddComponent_AddsComponentToEntity() {
        // Create entity
        Entity entity = EntityManager.Instance.CreateEntity();

        // Register component type
        ComponentManager.Instance.RegisterComponent(typeof(PositionComponent));

        // Add component to entity
        PositionComponent position = new PositionComponent(0, 0);
        entity.AddComponent(position);

        // Retrieve component array and check if component was added
        ComponentArray componentArray = ComponentManager.Instance.GetComponentArray(typeof(PositionComponent));
        Assert.IsTrue(componentArray.HasComponent(entity.Id));
    }
    
    [Test]
    public void RemoveComponent_RemovesComponentFromEntity() {
        // Create entity
        Entity entity = EntityManager.Instance.CreateEntity();

        // Register component type
        ComponentManager.Instance.RegisterComponent(typeof(PositionComponent));

        // Add component to entity
        entity.AddComponent(new PositionComponent(0,0));

        // Remove component from entity
        entity.RemoveComponent<PositionComponent>();

        // Retrieve component array and check if component was removed
        ComponentArray componentArray = ComponentManager.Instance.GetComponentArray(typeof(PositionComponent));
        Assert.IsFalse(componentArray.HasComponent(entity.Id));
    }
    
    [Test]
    public void GetComponent_ReturnsCorrectComponent() {
        // Create entity
        Entity entity = EntityManager.Instance.CreateEntity();

        // Register component type
        ComponentManager.Instance.RegisterComponent(typeof(PositionComponent));

        // Add component to entity
        PositionComponent position = new PositionComponent(0, 0);
        entity.AddComponent(position);

        // Retrieve component from entity and check if it is the correct one
        PositionComponent retrievedPosition = (PositionComponent)entity.GetComponent<PositionComponent>();
        Assert.AreEqual(position, retrievedPosition);
    }
    
    [Test]
    public void GetEntitiesWithComponents_ReturnsEntitiesWithComponents() {
        // Create entities
        Entity entity1 = EntityManager.Instance.CreateEntity();
        Entity entity2 = EntityManager.Instance.CreateEntity();
        Entity entity3 = EntityManager.Instance.CreateEntity();

        // Register component types
        ComponentManager.Instance.RegisterComponent(typeof(PositionComponent));
        ComponentManager.Instance.RegisterComponent(typeof(VelocityComponent));

        // Add components to entities
        entity1.AddComponent(new PositionComponent(0,0));
        entity2.AddComponent(new PositionComponent(0,0));
        entity2.AddComponent(new VelocityComponent(1, 1));
        entity3.AddComponent(new PositionComponent(0,0));
        entity3.AddComponent(new VelocityComponent(2, 2));

        // Retrieve entities with position and velocity components and check if they are correct
        Archetype archetype = new Archetype(new Type[] { typeof(PositionComponent),  typeof(VelocityComponent) });
        List<Entity> entitiesWithComponents = EntityManager.Instance.GetEntitiesWithArchetype(archetype);
        Assert.AreEqual(2, entitiesWithComponents.Count);
        Assert.Contains(entity2, entitiesWithComponents);
        Assert.Contains(entity3, entitiesWithComponents);
    }
    
}