using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAI;

namespace ModularData
{
    [CreateAssetMenu(fileName = "New Pathfind Movement Reference", menuName = "ScriptableObjects/DynamicReferenceType/Pathfind Movement")]
    public class PathfindMovementReferenceType : DynamicReferenceType
    {
        public override Type GetDataObjectType()
        {
            return typeof(PathfindMovementComponent);
        }

        public override bool IsObjectValid(UnityEngine.Object checkObject)
        {
            return checkObject is PathfindMovementComponent;
        }
    }
}
