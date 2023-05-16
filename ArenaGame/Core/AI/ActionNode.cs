using System;
using Microsoft.Xna.Framework;

namespace ArenaGame.Core.AI;

public class ActionNode : BehaviorNode
{
    private readonly Action<GameTime> action;
    public string ActionName { get; }
    public NodeStatus Status { get; private set; }

    public ActionNode(Action<GameTime> action, string actionName)
    {
        this.action = action;
        ActionName = actionName;
        Status = NodeStatus.Ready;
    }

    public override NodeStatus Execute(GameTime gametime)
    {
        Status = NodeStatus.Running;
        action.Invoke(gametime);
        Status = NodeStatus.Success;
        return Status;
    } 
}