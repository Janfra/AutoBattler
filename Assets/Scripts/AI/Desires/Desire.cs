using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameAI
{
    [Serializable]
    public abstract class Desire<T> : ScriptableObject
    {
        [SerializeField]
        protected int bias = 1;
        [SerializeField]
        protected T desireTarget;
        public T Target { get => desireTarget; }

        protected float desireValue = 0.0f;

        protected abstract void CalculateDesire();

        public virtual float GetDesireValue()
        {
            CalculateDesire();
            return desireValue;
        }

        public virtual bool IsValid()
        {
            return (desireTarget != null);
        }
    }
}