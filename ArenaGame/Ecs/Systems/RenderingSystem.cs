using ArenaGame.Ecs.Archetypes;
using ArenaGame.Ecs.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

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
        List<Entity> entities = new List<Entity>();

        // Fetch entities from all archetypes to be rendered OR implement way to get all entities who has a mesh component
        Player3DArchetype player3DArchetype = (Player3DArchetype)ArchetypeFactory.GetArchetype(EArchetype.Player3D);
        entities.AddRange(EntityManager.Instance.GetEntitiesWithArchetype(player3DArchetype));

        foreach (Entity entity in entities)
        {
            TransformComponent transform = (TransformComponent)entity.GetComponent<TransformComponent>();
            MeshComponent mesh = (MeshComponent)entity.GetComponent<MeshComponent>();
            
            if (transform != null && mesh != null)
            {
                Matrix worldMatrix = Matrix.CreateScale(transform.Scale) *
                                     Matrix.CreateFromQuaternion(transform.Rotation) *
                                     Matrix.CreateTranslation(transform.Position);
                DrawModel(mesh.Model, worldMatrix);
            }
        }
    }

    private void DrawModel(Model model, Matrix worldMatrix)
    {
        Matrix viewMatrix = ActiveCamera.ViewMatrix;
        Matrix projectionMatrix = ActiveCamera.ProjectionMatrix;

        foreach (ModelMesh mesh in model.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.World = worldMatrix;
                effect.View = viewMatrix;
                effect.Projection = projectionMatrix;
                effect.EnableDefaultLighting();
            }

            mesh.Draw();
        }
    }

    public void Update(GameTime gameTime)
    {
        
    }
}