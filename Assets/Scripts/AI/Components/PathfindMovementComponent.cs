using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI
{
    public class PathfindMovementComponent : MovementComponent
    {
        [SerializeField]
        private SharedGraphNodeHandle ownerPathfindNode;

        public void SetPathfindNode(SharedGraphNodeHandle sharedHandle)
        {
            if (sharedHandle != null && sharedHandle.Value.IsValid())
            {
                ownerPathfindNode = sharedHandle;
            }
        }

        public void SetPathfindTarget(GraphNodeHandle target)
        {

        }
    }
}
