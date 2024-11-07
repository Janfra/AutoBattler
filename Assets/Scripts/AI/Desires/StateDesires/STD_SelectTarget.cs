using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI
{
    [CreateAssetMenu(fileName = "New Select State Desire", menuName = "ScriptableObjects/Desires/States/Select Target")]
    public class STD_SelectTarget : StateDesire
    {
        protected override void CalculateDesire()
        {
            desireValue = 0;
        }
    }
}
