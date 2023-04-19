using System.Linq;
using ArenaGame.Ecs.Components;
using NUnit.Framework;

namespace ArenaGame.Ecs.Tests;

[TestFixture]
public class ArchetypeTests
{
    private ComponentArray _componentArray;
    private Archetype _archetype;
    private EntityManager _entityManager;

    [SetUp]
    public void Setup()
    {
        _componentArray = new ComponentArray();
        _entityManager = new EntityManager();
        _archetype = new Archetype(_componentArray, typeof(VelocityComponent), typeof(PositionComponent));
    }

    [Test]
    public void AddEntity_AddsEntityWithAllComponents()
    {
        // Arrange
        var entity = new Entity(1, _entityManager);
        var velocity = new VelocityComponent(1f, 2f);
        var position = new PositionComponent(3f, 4f);

        // Act
        _archetype.AddEntity(entity, velocity, position);

        // Assert
        Assert.IsTrue(_archetype.HasEntity(entity));
        Assert.IsTrue(_archetype.GetEntities().Contains(entity));
    }

    [Test]
    public void RemoveEntity_RemovesEntityWithAllComponents()
    {
        // Arrange
        var entity = new Entity(1, _entityManager);
        var velocity = new VelocityComponent(1f, 2f);
        var position = new PositionComponent(3f, 4f);
        _archetype.AddEntity(entity, velocity, position);

        // Act
        _archetype.RemoveEntity(entity);

        // Assert
        Assert.IsFalse(_archetype.HasEntity(entity));
        Assert.IsFalse(_archetype.GetEntities().Contains(entity));
    }

    [Test]
    public void HasEntity_ReturnsTrueForEntityWithAllComponents()
    {
        // Arrange
        var entity = new Entity(1, _entityManager);
        var velocity = new VelocityComponent(1f, 2f);
        var position = new PositionComponent(3f, 4f);
        _archetype.AddEntity(entity, velocity, position);

        // Act
        var result = _archetype.HasEntity(entity);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void HasEntity_ReturnsFalseForEntityWithoutAllComponents()
    {
        // Arrange
        var entity = new Entity(1, _entityManager);
        var velocity = new VelocityComponent(1f, 2f);
        _archetype.AddEntity(entity, velocity);

        // Act
        var result = _archetype.HasEntity(entity);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void GetEntities_ReturnsEntitiesWithAllComponents()
    {
        // Arrange
        var entity1 = new Entity(1, _entityManager);
        var velocity1 = new VelocityComponent(1f, 2f);
        var position1 = new PositionComponent(3f, 4f);
        _archetype.AddEntity(entity1, velocity1, position1);

        var entity2 = new Entity(2, _entityManager);
        var velocity2 = new VelocityComponent(5f, 6f);
        var position2 = new PositionComponent(7f, 8f);
        _archetype.AddEntity(entity2, velocity2, position2);

        var entity3 = new Entity(3, _entityManager);
        var velocity3 = new VelocityComponent(9f, 10f);
        var position3 = new PositionComponent(11f, 12f);
        _archetype.AddEntity(entity3, velocity3, position3);

        // Act
        var entities = _archetype.GetEntities();

        // Assert
        Assert.AreEqual(3, entities.Count());
        Assert.IsTrue(entities.Contains(entity1));
        Assert.IsTrue(entities.Contains(entity2));
        Assert.IsTrue(entities.Contains(entity3));
    }
}