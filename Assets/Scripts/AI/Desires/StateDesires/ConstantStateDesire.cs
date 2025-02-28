using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI
{
    [CreateAssetMenu(fileName = "StateConstantDesire", menuName = "ScriptableObjects/Desires/States/ConstantDesire", order = 1)]
    public class ConstantStateDesire : StateDesire
    {
        [SerializeField]
        [Range(0f, 1f)]
        protected float constantDesire;

        /// <summary>
        /// Return a defined value that is modified by the bias
        /// </summary>
        protected override void CalculateDesire()
        {
            desireValue = bias * constantDesire;
        }
    }
}