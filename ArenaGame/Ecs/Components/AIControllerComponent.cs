using ArenaGame.Core.AI;
using ArenaGame.Core.AI.BehaviorTrees;

namespace ArenaGame.Ecs.Components;

public enum EnemyType
{
   Basic,
   Fast,
   Heavy,
   Boss
}

public class AIControllerComponent : IComponent
{
   public BasicEnemyBehaviorTree BehaviorTree { get; set; }
   public EnemyType EnemyType { get; set; }
   public AIControllerComponent(EnemyType enemyType)
   {
      EnemyType = enemyType;
   }
}