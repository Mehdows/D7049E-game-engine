using System;
using Microsoft.Xna.Framework;

namespace ArenaGame.Core.AI;

public class ConditionNode : BehaviorNode
{
    private readonly Func<bool> condition;

    public ConditionNode(Func<bool> condition)
    {
        this.condition = condition;
    }

    public override NodeStatus Execute(GameTime gameTime)
    {
        return condition() ? NodeStatus.Success : NodeStatus.Failure;
    } 
}