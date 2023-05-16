using System;
using ArenaGame.Ecs.Components;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace ArenaGame.Ecs.Tests;

[TestFixture]
public class ComponentManagerTests {

    [Test]
    public void TestRegisterComponent() {
        ComponentManager componentManager = ComponentManager.Instance;
        Type componentType = typeof(TestGameComponent);
        ComponentArray componentArray = componentManager.GetComponentArray(componentType);

        Assert.IsNotNull(componentArray);
    }

    [Test]
    public void TestGetComponentArray() {
        ComponentManager componentManager = ComponentManager.Instance;
        Type componentType = typeof(TestGameComponent);
        ComponentArray componentArray1 = componentManager.GetComponentArray(componentType);
        ComponentArray componentArray2 = componentManager.GetComponentArray(componentType);

        Assert.AreEqual(componentArray1, componentArray2);
    }

    [Test]
    public void TestGetComponentArrayThrowsExceptionWhenTypeIsNull() {
        ComponentManager componentManager = ComponentManager.Instance;

        Assert.Throws<ArgumentNullException>(() => componentManager.GetComponentArray(null));
    }

    // Helper class for testing
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