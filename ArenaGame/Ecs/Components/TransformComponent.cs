using System.Security.Cryptography;
using ArenaGame.Ecs.Components;
using BEPUutilities;
using Microsoft.Xna.Framework;
using Matrix = BEPUutilities.Matrix;
using Quaternion = BEPUutilities.Quaternion;
using Vector3 = BEPUutilities.Vector3;

namespace ArenaGame;

public class TransformComponent : IComponent 
{
    public Vector3 Scale { get; set; } = new Vector3(1,1,1);
    
    internal Vector3 position;
    internal Vector3 rotation;
    internal Quaternion orientation = Quaternion.Identity;
    internal Matrix3x3 orientationMatrix = Matrix3x3.Identity;
    internal Matrix3x3 inertiaTensorInverse;
    internal Matrix3x3 inertiaTensor;
    internal Matrix3x3 localInertiaTensor;
    internal Matrix3x3 localInertiaTensorInverse;
    
    public Vector3 Position
    {
      get => this.position;
      set
      {
        this.position = value;
      }
    }
    public Quaternion Orientation
    {
      get => this.orientation;
      set
      {
        Quaternion.Normalize(ref value, out this.orientation);
        Matrix3x3.CreateFromQuaternion(ref this.orientation, out this.orientationMatrix);
        Matrix3x3 result;
        Matrix3x3.MultiplyTransposed(ref this.orientationMatrix, ref this.localInertiaTensorInverse, out result);
        Matrix3x3.Multiply(ref result, ref this.orientationMatrix, out this.inertiaTensorInverse);
        Matrix3x3.MultiplyTransposed(ref this.orientationMatrix, ref this.localInertiaTensor, out result);
        Matrix3x3.Multiply(ref result, ref this.orientationMatrix, out this.inertiaTensor);
      }
    }

    public Vector3 Rotation
    {
      get => this.rotation;
      set
      {
        Matrix matrix = Matrix.CreateFromQuaternion(this.orientation);
        rotation = matrix.Translation;
      }
    }
    
    public Matrix3x3 OrientationMatrix
    {
      get => this.orientationMatrix;
      set
      {
        Quaternion.CreateFromRotationMatrix(ref value, out this.orientation);
        this.Orientation = this.orientation;
      }
    }
    public Matrix WorldTransform
    {
      get
      {
        Matrix b;
        Matrix3x3.ToMatrix4X4(ref this.orientationMatrix, out b);
        b.Translation = this.position;
        return b;
      }
      set
      {
        Quaternion.CreateFromRotationMatrix(ref value, out this.orientation);
        this.Orientation = this.orientation;
        this.position = value.Translation;
      }
    }
    

    public TransformComponent(float x, float y, float z)
    {
        Position = new Vector3(x, y, z);
        // RotationMatrix = Matrix.CreateFromQuaternion(Rotation);
    }
    public TransformComponent(): this(0,0,0)
    {
        // RotationMatrix = Matrix.CreateFromQuaternion(Rotation);
    }

    //TODO: Kan säkert görs mkt snyggare men det funkar just nu
    public Vector3 Forward => Vector3.Backward;
    public Vector3 Up => Vector3.Up; 
    public Vector3 Right => Vector3.Right;
    

    // public void LookAt(Vector3 target)
    // {
    //     // Vector3 direction = target - Position;
    //     // if (direction != Vector3.Zero)
    //     // {
    //     //     Rotation = Quaternion.CreateFromRotationMatrix(Matrix.CreateLookAtRH(Position, target, Up));
    //     // }
    // }

}