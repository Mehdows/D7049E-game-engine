using ArenaGame.Ecs.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Matrix = BEPUutilities.Matrix;

namespace ArenaGame.Ecs.Systems;

public class RenderingSystem : ISystem
{
    public CameraComponent ActiveCamera { get; set; }

    public RenderingSystem(CameraComponent activeCamera)
    {
        ActiveCamera = activeCamera;
    }

    public void Draw()
    {
        // TODO: Ask if this is the best way to do this
        var meshComponentArray = ComponentManager.Instance.GetComponentArray(typeof(MeshComponent));
        foreach (var (entityID, meshComponent) in meshComponentArray.GetEntityComponents())
        {
            var entity = EntityManager.Instance.GetEntity(entityID);
            MeshComponent mesh = (MeshComponent)meshComponent;
            TransformComponent transform = (TransformComponent)entity.GetComponent<TransformComponent>();
            if (transform != null && mesh != null)
            {
                Matrix worldMatrix = Matrix.CreateScale(transform.Scale) *
                                     Matrix.CreateFromQuaternion(transform.Rotation) *
                                     Matrix.CreateTranslation(transform.Position);
                DrawModel(mesh.Model, worldMatrix);
            }
        }
        
        /*
        List<Entity> entities = new List<Entity>();
        Player3DArchetype player3DArchetype = (Player3DArchetype)ArchetypeFactory.GetArchetype(EArchetype.Player3D);
        entities.AddRange(EntityManager.Instance.GetEntitiesWithArchetype(player3DArchetype));
        foreach (Entity entity in entities )
        {
            MeshComponent mesh = (MeshComponent)entity.GetComponent<MeshComponent>();
            TransformComponent transform = (TransformComponent)entity.GetComponent<TransformComponent>();
            if (transform != null && mesh != null)
            {
                Matrix worldMatrix = Matrix.CreateScale(transform.Scale) *
                                     Matrix.CreateFromQuaternion(transform.Rotation) *
                                     Matrix.CreateTranslation(transform.Position);
                DrawModel(mesh.Model, worldMatrix);
            }
        }
        */
        
    }

    private void DrawModel(Model model, Matrix worldMatrix)
    {
        Matrix viewMatrix =  ActiveCamera.ViewMatrix;
        Matrix projectionMatrix = ActiveCamera.ProjectionMatrix;

        foreach (ModelMesh mesh in model.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.World = MathConverter.Convert(worldMatrix);
                effect.View = MathConverter.Convert(viewMatrix);
                effect.Projection = MathConverter.Convert(projectionMatrix);
                effect.EnableDefaultLighting();
            }

            mesh.Draw();
        }
    }

    public void Update(GameTime gameTime)
    {
        
    }
}