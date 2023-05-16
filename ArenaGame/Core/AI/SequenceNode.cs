using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ArenaGame.Core.AI;

public class SequenceNode : BehaviorNode
{
    private readonly List<BehaviorNode> childNodes = new List<BehaviorNode>();

    public void AddChildNode(BehaviorNode node)
    {
        childNodes.Add(node);
    }

    public override NodeStatus Execute(GameTime gameTime)
    {
        foreach (var node in childNodes)
        {
            var status = node.Execute(gameTime);
            if (status != NodeStatus.Success)
                return status;
        }
        return NodeStatus.Success;
    } 
}