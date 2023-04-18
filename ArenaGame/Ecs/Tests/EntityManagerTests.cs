using System.Collections.Generic;
using ArenaGame.Ecs.Components;
using NUnit.Framework;

namespace ArenaGame.Ecs.Tests;

[TestFixture]
public class EntityManagerTests
{
    private EntityManager entityManager;

    [SetUp]
    public void Setup()
    {
        entityManager = new EntityManager();
    }

    [Test]
    public void CreateEntity_ReturnsNewEntity()
    {
        Entity entity = entityManager.CreateEntity();

        Assert.IsNotNull(entity);
    }

    [Test]
    public void DestroyEntity_RemovesEntity()
    {
        Entity entity = entityManager.CreateEntity();
        entityManager.DestroyEntity(entity);

        Assert.IsFalse(entityManager.Entities.Contains(entity));
    }

    [Test]
    public void AddComponent_AddsComponentToEntity()
    {
        Entity entity = entityManager.CreateEntity();
        PositionComponent positionComponent = new PositionComponent();

        entityManager.AddComponent(entity, positionComponent);

        Assert.IsTrue(entityManager.HasComponent<PositionComponent>(entity));
    }

    [Test]
    public void RemoveComponent_RemovesComponentFromEntity()
    {
        Entity entity = entityManager.CreateEntity();
        PositionComponent positionComponent = new PositionComponent();
        entityManager.AddComponent(entity, positionComponent);

        entityManager.RemoveComponent<PositionComponent>(entity);

        Assert.IsFalse(entityManager.HasComponent<PositionComponent>(entity));
    }

    [Test]
    public void GetComponent_GetsComponentFromEntity()
    {
        Entity entity = entityManager.CreateEntity();
        PositionComponent expectedComponent = new PositionComponent();
        entityManager.AddComponent(entity, expectedComponent);

        PositionComponent actualComponent = entityManager.GetComponent<PositionComponent>(entity);

        Assert.AreEqual(expectedComponent, actualComponent);
    }

    [Test]
    public void HasComponent_ReturnsTrueIfEntityHasComponent()
    {
        Entity entity = entityManager.CreateEntity();
        PositionComponent positionComponent = new PositionComponent();
        entityManager.AddComponent(entity, positionComponent);

        bool hasComponent = entityManager.HasComponent<PositionComponent>(entity);

        Assert.IsTrue(hasComponent);
    }

    [Test]
    public void HasComponent_ReturnsFalseIfEntityDoesNotHaveComponent()
    {
        Entity entity = entityManager.CreateEntity();

        bool hasComponent = entityManager.HasComponent<PositionComponent>(entity);

        Assert.IsFalse(hasComponent);
    }

    [Test]
    public void GetComponent_ReturnsNullIfEntityDoesNotHaveComponent()
    {
        Entity entity = entityManager.CreateEntity();

        PositionComponent component = entityManager.GetComponent<PositionComponent>(entity);

        Assert.IsNull(component);
    }

    
    /*
    [Test]
    public void GetEntitiesWithComponents_ReturnsEntitiesWithAllComponents()
    {
        Entity entity1 = entityManager.CreateEntity();
        entityManager.AddComponent(entity1, new PositionComponent());
        entityManager.AddComponent(entity1, new VelocityComponent());

        Entity entity2 = entityManager.CreateEntity();
        entityManager.AddComponent(entity2, new PositionComponent());
        entityManager.AddComponent(entity2, new SpriteComponent());

        List<Entity> entitiesWithComponents = entityManager.GetEntitiesWithComponents(typeof(PositionComponent), typeof(VelocityComponent));

        Assert.AreEqual(1, entitiesWithComponents.Count);
        Assert.AreEqual(entity1, entitiesWithComponents[0]);
    }

    [Test]
    public void GetEntitiesWithComponents_ReturnsEmptyListIfNoEntitiesHaveAllComponents()
    {
        Entity entity1 = entityManager.CreateEntity();
        entityManager.AddComponent(entity1, new PositionComponent());
        entityManager.AddComponent(entity1, new VelocityComponent());

        Entity entity2 = entityManager.CreateEntity();
        entityManager.AddComponent(entity2, new PositionComponent());

        List<Entity> entitiesWithComponents = entityManager.GetEntitiesWithComponents(typeof(PositionComponent), typeof(VelocityComponent));

        Assert.AreEqual(0, entitiesWithComponents.Count);
    }*/
}