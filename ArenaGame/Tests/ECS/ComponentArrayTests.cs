using System;
using ArenaGame.Ecs.Components;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace ArenaGame.Ecs.Tests;

[TestFixture]
public class ComponentArrayTests
{
    private Type componentType;
    private ComponentArray componentArray;

    [SetUp]
    public void Setup()
    {
        componentType = typeof(TestGameComponent);
        componentArray = new ComponentArray(componentType);
    }

    [Test]
    public void AddComponent_WhenCalled_AddsComponentToArray()
    {
        // Arrange
        int entityId = 1;
        var component = new TestGameComponent();

        // Act
        componentArray.AddComponent(entityId, component);

        // Assert
        Assert.AreEqual(component, componentArray.GetComponent(entityId));
    }

    [Test]
    public void RemoveComponent_WhenCalled_RemovesComponentFromArray()
    {
        // Arrange
        int entityId = 1;
        var component = new TestGameComponent();
        componentArray.AddComponent(entityId, component);

        // Act
        componentArray.RemoveComponent(entityId);

        // Assert
        Assert.IsNull(componentArray.GetComponent(entityId));
    }

    [Test]
    public void HasComponent_WhenComponentExists_ReturnsTrue()
    {
        // Arrange
        int entityId = 1;
        var component = new TestGameComponent();
        componentArray.AddComponent(entityId, component);

        // Act
        bool hasComponent = componentArray.HasComponent(entityId);

        // Assert
        Assert.IsTrue(hasComponent);
    }

    [Test]
    public void HasComponent_WhenComponentDoesNotExist_ReturnsFalse()
    {
        // Arrange
        int entityId = 1;

        // Act
        bool hasComponent = componentArray.HasComponent(entityId);

        // Assert
        Assert.IsFalse(hasComponent);
    }

    [Test]
    public void GetComponentType_WhenCalled_ReturnsComponentType()
    {
        // Act
        Type returnedComponentType = componentArray.GetComponentType();

        // Assert
        Assert.AreEqual(componentType, returnedComponentType);
    }

    private class TestGameComponent : IComponent
    {
        public override void Initialize()
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}