namespace ArenaGame.Ecs.Archetypes;

public enum EArchetype {
    StaticMesh = 0, // PostionComponent, RotationComponent, MeshComponent
    DynamicMesh = 1, // StaticMesh, VelocityComponent 
    Player = 2, // DynamicMesh ,  AnimationComponent, InputComponent
    Enemy = 3, // DynamicMesh,  AnimationComponent, AIComponent
    Projectile = 4, // DynamicMesh 
    Light = 5,  // PositionComponent, LightComponent, RotationComponent
    Particle = 6, //
    Trigger = 7,
    Player3D = 8 // TransformComponent, InputComponent
} 