using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    [CreateAssetMenu(fileName = "New Interface Reference", menuName = "ScriptableObjects/DynamicReferenceType/Runtime Scriptable Object", order = 1)]
    public class IRuntimeScriptableObjectReferenceType : DynamicInterfaceReferenceType<IRuntimeScriptableObject>
    {
    }
}

