namespace ArenaGame.Ecs.Components;

public class RenderComponent : IComponent
{
    public TransformComponent Transform { get; set; }
    public MeshComponent Mesh { get; set; }

    public RenderComponent(TransformComponent transform, MeshComponent mesh)
    {
        Transform = transform;
        Mesh = mesh;
    }
}