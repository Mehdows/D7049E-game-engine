using Microsoft.Xna.Framework;

namespace ArenaGame.Ecs.Components;

public class TransformComponent : IComponent
{
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
    public Vector3 Scale { get; set; }


    public Vector3 Forward => Vector3.Transform(Vector3.Forward, Rotation);
    public Vector3 Up => Vector3.Transform(Vector3.Up, Rotation);

    public TransformComponent(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        Position = position;
        Rotation = rotation;
        Scale = scale;
    }

    public void LookAt(Vector3 target)
    {
        Vector3 direction = target - Position;
        if (direction != Vector3.Zero)
        {
            Rotation = Quaternion.CreateFromRotationMatrix(Matrix.CreateLookAt(Position, target, Up));
        }
    }
}