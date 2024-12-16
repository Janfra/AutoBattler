using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    [CreateAssetMenu(fileName = "New Attack Component Reference", menuName = "ScriptableObjects/DynamicReferenceType/Attack Component")]
    public class AttackComponentReferenceType : DynamicReferenceType
    {
        public override Type GetDataObjectType()
        {
            return typeof(AttackComponent);
        }

        public override bool IsObjectValid(UnityEngine.Object checkObject)
        {
            return checkObject is AttackComponent;
        }
    }
}       
