using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ArenaGame;

class Camera {
    public const float CAM_HEIGHT_OFFSET = 80f;

    public const float FAR_PLANE = 2000f;
    public Vector3 pos, target;
    public Matrix view, projection, view_projection;
    public Vector3 up;
    float current_angle;
    float angle_velocity;
    float radiues = 100.0f;
    Vector3 unit_direction;

    Input input;

    public Camera(GraphicsDevice gpu, Vector3 UpDirection, Input input){
        up = UpDirection;
        pos = new Vector3(20, -30, -50);
        target = Vector3.Zero;
        view = Matrix.CreateLookAt(pos, target, up);
        projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, gpu.Viewport.AspectRatio, 0.1f, FAR_PLANE);
        view_projection = view * projection;
        this.input = input;
        unit_direction = view.Forward;
        unit_direction.Normalize();
        
    }

    public void MoveCamera(Vector3 move){
        pos += move;
        view = Matrix.CreateLookAt(pos, target, up);
        view_projection = view * projection;
    }

    public void UpdateTarget(Vector3 new_target){
        target = new_target;
        target.Y -= 10;
        view = Matrix.CreateLookAt(pos, target, up);
        view_projection = view * projection;
    }

    public void UpdatePlayerCam(){
        #region TEMPORARY_ADDITIONAL_CAMERA_MOVEMENT
        if(input.KeyDown(Keys.A)) { pos.Y += 5; }
        if(input.KeyDown(Keys.Z)) { pos.Y -= 5; }
        if(input.KeyDown(Keys.Q)) { pos.X += 5; }
        if(input.KeyDown(Keys.W)) { pos.X -= 5; }
        if(input.KeyDown(Keys.X)) { pos.Z += 5; }
        if(input.KeyDown(Keys.S)) { pos.Z -= 5; }
        #endregion

        UpdateTarget(Vector3.Zero); //UpdateTarget(Player.pos) <-- Later
    }
}