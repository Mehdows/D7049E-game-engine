using BEPUutilities;

namespace ArenaGame.Ecs.Components;

public class TransformComponent : IComponent
{
    public Vector3 Position { get; set; } = Vector3.Zero;
    public Quaternion Rotation { get; set; } = Quaternion.Identity;
    public Vector3 Scale { get; set; } = new Vector3(1,1,1);


    Matrix rotationMatrix;

    public TransformComponent()
    {
        rotationMatrix = Matrix.CreateFromQuaternion(Rotation);
    }

    //TODO: Kan säkert görs mkt snyggare men det funkar just nu
    public Vector3 Forward => new (
        Vector3.Forward.X * rotationMatrix.M11 + Vector3.Forward.Y * rotationMatrix.M21 + Vector3.Forward.Z * rotationMatrix.M31,
        Vector3.Forward.X * rotationMatrix.M12 + Vector3.Forward.Y * rotationMatrix.M22 + Vector3.Forward.Z * rotationMatrix.M32,
        Vector3.Forward.X * rotationMatrix.M13 + Vector3.Forward.Y * rotationMatrix.M23 + Vector3.Forward.Z * rotationMatrix.M33);
    public Vector3 Up => new (
        Vector3.Up.X * rotationMatrix.M11 + Vector3.Up.Y * rotationMatrix.M21 + Vector3.Up.Z * rotationMatrix.M31,
        Vector3.Up.X * rotationMatrix.M12 + Vector3.Up.Y * rotationMatrix.M22 + Vector3.Up.Z * rotationMatrix.M32,
        Vector3.Up.X * rotationMatrix.M13 + Vector3.Up.Y * rotationMatrix.M23 + Vector3.Up.Z * rotationMatrix.M33);

    public void LookAt(Vector3 target)
    {
        Vector3 direction = target - Position;
        if (direction != Vector3.Zero)
        {
            Rotation = Quaternion.CreateFromRotationMatrix(Matrix.CreateLookAtRH(Position, target, Up));
        }
    }
}