using ArenaGame.Ecs.Components;
using Microsoft.Xna.Framework;
using Matrix = BEPUutilities.Matrix;
using Quaternion = BEPUutilities.Quaternion;
using Vector3 = BEPUutilities.Vector3;

namespace ArenaGame;

public class TransformComponent : IComponent 
{
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; } = Quaternion.Identity;
    public Vector3 Scale { get; set; } = new Vector3(1,1,1);

    Matrix rotationMatrix;

    public TransformComponent(Vector3 position)
    {
        Position = position;
        rotationMatrix = Matrix.CreateFromQuaternion(Rotation);
    }
    public TransformComponent(): this(new Vector3(0,0,0))
    {
        rotationMatrix = Matrix.CreateFromQuaternion(Rotation);
    }

    //TODO: Kan säkert görs mkt snyggare men det funkar just nu
    public Vector3 Forward => Vector3.Backward;
    public Vector3 Up => Vector3.Up; 

    public void LookAt(Vector3 target)
    {
        Vector3 direction = target - Position;
        if (direction != Vector3.Zero)
        {
            Rotation = Quaternion.CreateFromRotationMatrix(Matrix.CreateLookAtRH(Position, target, Up));
        }
    }

}