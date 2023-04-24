using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace ArenaGame.Ecs.Components;

public class TransformComponent : IComponent
{
    public Vector3 Position { get; set; } = Vector3.Zero;
    public Quaternion Rotation { get; set; } = Quaternion.Identity;
    public Vector3 Scale { get; set; } = Vector3.One;


    public Vector3 Forward => Vector3.Transform(Vector3.Forward, Rotation);
    public Vector3 Up => Vector3.Transform(Vector3.Up, Rotation);

    public void LookAt(Vector3 target)
    {
        Vector3 direction = target - Position;
        if (direction != Vector3.Zero)
        {
            Rotation = Quaternion.CreateFromRotationMatrix(Matrix.CreateLookAt(Position, target, Up));
        }
    }
}