using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI
{
    public class PathfindMovementComponent : MovementComponent
    {
        [SerializeField]
        private SharedGraphNodeHandle ownerPathfindNode;

        public void SetPathfindTarget(GraphNodeHandle target)
        {

        }
    }
}
