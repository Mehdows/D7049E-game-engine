using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUphysics.Entities;
using BEPUutilities;

namespace ArenaGame;

public class CollisionShape : Entity<ConvexCollidable<ConvexShape>>
{
    public CollisionShape(Vector3 position, ConvexShape shape, float mass)
    {
        this.Position = position;
        Initialize(new ConvexCollidable<ConvexShape>(shape), mass);
    }
    
}