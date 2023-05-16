namespace ArenaGame.Ecs.Components;

public class WeaponComponent : IComponent
{
    public Entity WeaponEntity { get; set; }
    public float Radius { get; set; }
    public float Speed { get; set; }

    public WeaponComponent(Entity weaponEntity, float radius, float rotationSpeed)
    {
        WeaponEntity = weaponEntity;
        Radius = radius;
        Speed = rotationSpeed;
    }
}