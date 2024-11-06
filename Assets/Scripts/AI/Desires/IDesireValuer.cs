using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameAI
{
    public interface IDesireValuer
    {
        public float GetDesireCalculationValue();
    }
}