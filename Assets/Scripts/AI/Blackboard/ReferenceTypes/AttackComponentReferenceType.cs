using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    [CreateAssetMenu(fileName = "New Attack Component Reference", menuName = "ScriptableObjects/BlackboardReferenceType/Attack Component")]
    public class AttackComponentReferenceType : BlackboardReferenceType
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
