namespace ArenaGame.Ecs.Archetypes;

public enum EArchetype {
    StaticMesh = 0, // PostionComponent, RotationComponent, MeshComponent
    DynamicMesh = 1, // StaticMesh, VelocityComponent 
    Enemy = 2, // DynamicMesh,  AnimationComponent, AIComponent
    Projectile = 3, // DynamicMesh 
    Light = 4,  // PositionComponent, LightComponent, RotationComponent
    Particle = 5, //
    Trigger = 6,
    Player = 7, // TransformComponent, InputComponent
    Weapon = 8,
    Spawner = 9
} 