using System;
using Microsoft.Xna.Framework;
using MathHelper = BEPUutilities.MathHelper;
using Quaternion = BEPUutilities.Quaternion;
using Vector3 = BEPUutilities.Vector3;

namespace ArenaGame.Core.AI.BehaviorTrees;

public class BasicEnemyBehaviorTree
{
    private SelectorNode rootSelector;
    private TransformComponent enemyTransform;
    private CollisionComponent enemyCollision;
    private Entity aiEntity;
    private Entity playerEntity;
    private float detectionRange;
    
    private int minValue = -1;
    private int maxValue = 1;
    
    
    float speed = 5;
    float maxSpeed = 300;
    float brakeSpeed = 0.05f;


    public BasicEnemyBehaviorTree(Entity ai, Entity player, float range)
    {
        aiEntity = ai;
        playerEntity = player;
        detectionRange = range;
        
        enemyTransform = (TransformComponent)aiEntity.GetComponent<TransformComponent>();
        enemyCollision = (CollisionComponent)aiEntity.GetComponent<CollisionComponent>();

        rootSelector = new SelectorNode();
        
        var followPlayerCondition = new ConditionNode(IsPlayerInRange);
        var followPlayerAction = new ActionNode(FollowPlayer, "Follow Player");
        var randomWalkAction = new ActionNode(RandomWalk, "Random Walk");
        
        rootSelector.AddChildNode(followPlayerCondition);
        rootSelector.AddChildNode(followPlayerAction);
        rootSelector.AddChildNode(randomWalkAction);
        
    }
    
    public void Update(GameTime gameTime)
    {
        NodeStatus status = rootSelector.Execute(gameTime);
        
        // Log the current action being executed
        if (status == NodeStatus.Running)
        {
            Log("Executing action: " + GetCurrentActionName());
        }
        
    }
    
    private string GetCurrentActionName()
    {
        // Find the current action node being executed
        ActionNode currentActionNode = FindCurrentActionNode(rootSelector);

        // Get the name of the action node
        return currentActionNode?.ActionName ?? "None";
    }

    private ActionNode FindCurrentActionNode(BehaviorNode node)
    {
        if (node is ActionNode actionNode)
        {
            if (actionNode.Status == NodeStatus.Running)
            {
                return actionNode;
                
            }
        }

        return null;
    }

    private void Log(string message)
    {
        Console.WriteLine(message);
    }

    
    private bool IsPlayerInRange()
    {
        var target = ((TransformComponent) playerEntity.GetComponent<TransformComponent>()).Position;
        float distance = Vector3.Distance(enemyTransform.Position, target);
        Console.WriteLine($"AI: Distance to player -> {distance}");
        
        return distance <= detectionRange;
    }
    
    private void FollowPlayer(GameTime gameTime)
    {
        Vector3 direction = ((TransformComponent)playerEntity.GetComponent<TransformComponent>()).position - enemyTransform.position;
        direction.Normalize();
        
        // enemyTransform.WorldTransform = enemyCollision.CollisionEntity.WorldTransform;
        // Vector3 newVelocity = Accelerate(direction, speed);
        // enemyCollision.CollisionEntity.LinearVelocity = new Vector3(newVelocity.X, enemyCollision.CollisionEntity.LinearVelocity.Y, newVelocity.Z);
        // enemyTransform.WorldTransform = enemyCollision.CollisionEntity.WorldTransform;
        // enemyTransform.Orientation = Rotate(enemyTransform, direction);
    }
    
    private void RandomWalk(GameTime gameTime)
    {
        
        Vector3 randomDirection = new Vector3(Utils.GetRandomFloatInRange(-1f, 1f), 0f, Utils.GetRandomFloatInRange(-1f, 1f));
        randomDirection.Normalize();
        
        // enemyTransform.WorldTransform = enemyCollision.CollisionEntity.WorldTransform;
        // Vector3 newVelocity = Accelerate(randomDirection, speed);
        // enemyCollision.CollisionEntity.LinearVelocity = new Vector3(newVelocity.X, enemyCollision.CollisionEntity.LinearVelocity.Y, newVelocity.Z);
        // enemyTransform.WorldTransform = enemyCollision.CollisionEntity.WorldTransform;
        // enemyTransform.Orientation = Rotate(enemyTransform, randomDirection);
    }
    
    
    private Vector3 Accelerate(Vector3 direction, float speed)
    {
        // Get the current velocity
        Vector3 velocity = ((CollisionComponent)aiEntity.GetComponent<CollisionComponent>()).CollisionEntity.LinearVelocity;
        
        // Calculate the new velocity
        Vector3 newVelocity = velocity + (direction * speed); 
        
        if (newVelocity.LengthSquared() > maxSpeed * maxSpeed)
        {
            newVelocity.Normalize();
            newVelocity *= maxSpeed;
        }
        
        // Check if the player is running in the opposite direction and apply a braking force
        if (Vector3.Dot(direction, velocity) < 0 && Math.Abs(velocity.Length()) > 0.1f)
        {
            Vector3 brakingForce = velocity * brakeSpeed;
            // Console.Out.WriteLine($"Braking force {brakingForce} ");
            newVelocity -= brakingForce;
        }
        
        // Set the new velocity
        return newVelocity;
    }

    private Quaternion Rotate(TransformComponent aiTransform, Vector3 movementDirection)
    {
        float angle = (float)Math.Atan2(movementDirection.X, movementDirection.Z);
        angle = MathHelper.ToDegrees(angle);
        
        // Round the angle to the nearest 45 degrees
        angle = (float)(Math.Round(angle / 45) * 45);
    
        // Convert the angle to a quaternion and set the player's rotation
        return Quaternion.CreateFromAxisAngle(aiTransform.Up, MathHelper.ToRadians(angle));
    }
}