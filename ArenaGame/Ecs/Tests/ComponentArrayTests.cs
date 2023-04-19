using ArenaGame.Ecs.Components;
using NUnit.Framework;

namespace ArenaGame.Ecs.Tests;

[TestFixture]
public class ComponentArrayTests
{
    private ComponentArray componentArray;
    private EntityManager entityManager;
    private Entity entity;

    [SetUp]
    public void Setup()
    {
        componentArray = new ComponentArray();
        entityManager = new EntityManager();
        entity = new Entity(1, entityManager);
    }

    [Test]
    public void AddComponent_ShouldAddComponentToComponentArray()
    {
        // Arrange
        var component = new PositionComponent(1, 2);

        // Act
        componentArray.AddComponent(entity, component);

        // Assert
        Assert.IsTrue(componentArray.HasComponent<PositionComponent>(entity));
    }

    [Test]
    public void RemoveComponent_ShouldRemoveComponentFromComponentArray()
    {
        // Arrange
        var component = new PositionComponent(1, 2);
        componentArray.AddComponent(entity, component);

        // Act
        componentArray.RemoveComponent<PositionComponent>(entity);

        // Assert
        Assert.IsFalse(componentArray.HasComponent<PositionComponent>(entity));
    }

    [Test]
    public void GetComponent_ShouldReturnComponentIfItExistsInComponentArray()
    {
        // Arrange
        var component = new PositionComponent(1, 2);
        componentArray.AddComponent(entity, component);

        // Act
        var result = componentArray.GetComponent<PositionComponent>(entity);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<PositionComponent>(result);
        Assert.AreEqual(component.X, result.X);
        Assert.AreEqual(component.Y, result.Y);
    }

    [Test]
    public void GetComponent_ShouldReturnNullIfComponentDoesNotExistInComponentArray()
    {
        // Act
        var result = componentArray.GetComponent<PositionComponent>(entity);

        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public void HasComponent_ShouldReturnTrueIfComponentExistsInComponentArray()
    {
        // Arrange
        var component = new PositionComponent(1, 2);
        componentArray.AddComponent(entity, component);

        // Act
        var result = componentArray.HasComponent<PositionComponent>(entity);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public void HasComponent_ShouldReturnFalseIfComponentDoesNotExistInComponentArray()
    {
        // Act
        var result = componentArray.HasComponent<PositionComponent>(entity);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void RemoveEntity_ShouldRemoveEntityFromAllComponentArrays()
    {
        // Arrange
        var positionComponent = new PositionComponent(1, 2);
        var velocityComponent = new VelocityComponent();
        componentArray.AddComponent(entity, positionComponent);
        componentArray.AddComponent(entity, velocityComponent);

        // Act
        componentArray.RemoveEntity(entity);

        // Assert
        Assert.IsFalse(componentArray.HasComponent<PositionComponent>(entity));
        Assert.IsFalse(componentArray.HasComponent<VelocityComponent>(entity));
    }
}