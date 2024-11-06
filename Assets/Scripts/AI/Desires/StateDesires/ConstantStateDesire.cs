using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StateConstantDesire", menuName = "ScriptableObjects/Desires/States/ConstantDesire", order = 1)]
public class ConstantStateDesire : StateDesire
{
    [SerializeField]
    [Range(0f, 1f)]
    protected float constantDesire;

    protected override void CalculateDesire()
    {
        desireValue = bias * constantDesire;
    }
}
