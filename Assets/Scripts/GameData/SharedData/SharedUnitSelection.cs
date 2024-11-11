using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    [CreateAssetMenu(fileName = "New Shared Unit Data", menuName = "ScriptableObjects/SharedValues/Unit Data")]
    public class SharedUnitSelection : SharedValue<UnitDefinition>
    {
    }
}
