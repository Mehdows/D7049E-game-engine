namespace ArenaGame.Ecs.Archetypes;

public enum EArchetype {
    StaticMesh = 0, // PostionComponent, RotationComponent, MeshComponent
    DynamicMesh = 1, // StaticMesh, VelocityComponent 
    Enemy = 3, // DynamicMesh,  AnimationComponent, AIComponent
    Projectile = 4, // DynamicMesh 
    Light = 5,  // PositionComponent, LightComponent, RotationComponent
    Particle = 6, //
    Trigger = 7,
    Player = 8 // TransformComponent, InputComponent
} 