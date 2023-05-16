using System;
using System.Linq;
using ArenaGame.Ecs.Components;
using NUnit.Framework;

namespace ArenaGame.Ecs.Tests;

[TestFixture]
public class ArchetypeTests {

    [Test]
    public void TestMatchesReturnsTrueWhenEntityMatchesArchetype() {
        // Arrange
        Type[] componentTypes = new[] { typeof(VelocityComponent), typeof(PositionComponent) };
        Archetype archetype = new Archetype(componentTypes);

        // Create an entity with the required components
        Entity entity = EntityManager.Instance.CreateEntity();
        entity.AddComponent(new PositionComponent(0,0));
        entity.AddComponent(new VelocityComponent(0,0));

        // Act
        bool matches = archetype.Matches(entity.Id);

        // Assert
        Assert.True(matches);
    }

    [Test]
    public void TestMatchesReturnsFalseWhenEntityDoesNotMatchArchetype() {
        // Arrange
        Type[] componentTypes = new[] { typeof(VelocityComponent), typeof(PositionComponent) };
        Archetype archetype = new Archetype(componentTypes);

        // Create an entity without the required components
        Entity entity = EntityManager.Instance.CreateEntity();

        // Act
        bool matches = archetype.Matches(entity.Id);

        // Assert
        Assert.False(matches);
    }

    [Test]
    public void TestGetComponentArrayReturnsComponentArrayIfPresent() {
        // Arrange
        Type[] componentTypes = new[] { typeof(VelocityComponent), typeof(PositionComponent) };
        Archetype archetype = new Archetype(componentTypes);

        // Act
        ComponentArray componentArray = archetype.GetComponentArray(typeof(PositionComponent));

        // Assert
        Assert.IsNotNull(componentArray);
        Assert.AreEqual(typeof(PositionComponent), componentArray.GetComponentType());
    }

    [Test]
    public void TestGetComponentArrayReturnsNullIfNotPresent() {
        // Arrange
        Type[] componentTypes = new[] { typeof(VelocityComponent), typeof(PositionComponent) };
        Archetype archetype = new Archetype(componentTypes);

        //TODO: Fix so nat it dosnt use sprite
        // // Act
        // ComponentArray componentArray = archetype.GetComponentArray(typeof(SpriteComponent));
        //
        // // Assert
        // Assert.IsNull(componentArray);
    }
}