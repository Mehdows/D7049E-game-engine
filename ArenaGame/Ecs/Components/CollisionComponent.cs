using ArenaGame.Ecs.Components;
using BEPUphysics.BroadPhaseEntries.MobileCollidables;
using BEPUphysics.CollisionShapes.ConvexShapes;
using BEPUphysics.Entities;
using BEPUphysics.Entities.Prefabs;
using BEPUutilities;

namespace ArenaGame;

public class  CollisionComponent: IComponent
{
    public Matrix Transform;
    public ConvexShape Shape { get; set; }
    public CollisionShape CollisionEntity { get; set; } 

    public CollisionComponent(Vector3 startPosition, ConvexShape shape, Vector3 transformScale, string tag)
    {
        // new Capsule(new Vector3(0,20,20),10,2,3);
        Shape = shape;
        CollisionEntity = new CollisionShape(startPosition, Shape, 10f);
        CollisionEntity.Tag = tag;
        CollisionEntity.AngularDamping = 0f;
        CollisionEntity.LocalInertiaTensorInverse = new Matrix3x3(0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0.0f);
        CollisionEntity.Gravity = new Vector3(0, -150.82f, 0);
        if (Shape as CapsuleShape != null)
        {
            Transform =  Matrix.CreateScale(((CapsuleShape)Shape).Radius / transformScale.X, ((CapsuleShape)Shape).Length / transformScale.Y, ((CapsuleShape)Shape).Radius / transformScale.Z);
        }else if (Shape as BoxShape != null)
        {
            Transform =  Matrix.CreateScale(((BoxShape)Shape).Width / transformScale.X, ((BoxShape)Shape).Height / transformScale.Y, ((BoxShape)Shape).Length / transformScale.Z);
        }
    }
}