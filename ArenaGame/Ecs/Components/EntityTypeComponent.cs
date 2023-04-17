namespace ArenaGame.Ecs.Components;

public enum EntityType 
{
    Player,
    Enemy,
    NPC
}

public class EntityTypeComponent
{
    public EntityType type;
}