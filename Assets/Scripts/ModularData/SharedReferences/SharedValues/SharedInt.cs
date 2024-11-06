using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    [CreateAssetMenu(fileName = "New Shared Int", menuName = "ScriptableObjects/SharedValues/Int")]
    public class SharedInt : SharedValue<int>
    {
    }
}
