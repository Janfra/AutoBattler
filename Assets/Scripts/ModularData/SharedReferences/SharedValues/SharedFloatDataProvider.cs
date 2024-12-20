using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    [CreateAssetMenu(fileName = "New Shared Float Provider", menuName = "ScriptableObjects/SharedValues/Float Provider")]
    public class SharedFloatDataProvider : SharedValue<IDataProvider<float>>
    {
    }
}
