using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ModularData
{
    [CreateAssetMenu(fileName = "New Interface Reference", menuName = "ScriptableObjects/DynamicReferenceType/Int Data Provider")]
    public class IDataProviderIntReferenceType : DynamicInterfaceReferenceType<IDataProvider<int>>
    {
    }
}
