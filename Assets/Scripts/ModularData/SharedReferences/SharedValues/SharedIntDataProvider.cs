using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    [CreateAssetMenu(fileName = "New Shared Int Provider", menuName = "ScriptableObjects/SharedValues/Int Provider")]
    public class SharedIntDataProvider : SharedValue<IDataProvider<int>>
    {
    }
}
