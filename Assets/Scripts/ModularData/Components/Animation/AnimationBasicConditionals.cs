using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    public abstract class AnimationIntConditional<T> : AnimationConditionalDataSetter<T> where T : IAnimatorParameterSetter
    {
        [SerializeField]
        private IntReference valueOne;
        [SerializeField]
        private IntReference valueTwo;

        public override bool OnEqualComparison()
        {
            return valueOne.Value == valueTwo.Value;
        }

        public override bool OnLessThanComparison()
        {
            return valueOne.Value < valueTwo.Value;
        }

        public override bool OnLessThanOrEqualComparison()
        {
            return valueOne <= valueTwo.Value;
        }

        public override bool OnMoreThanComparison()
        {
            return valueOne > valueTwo.Value;
        }

        public override bool OnMoreThanOrEqualComparison()
        {
            return valueOne >= valueTwo.Value;
        }

        public override bool OnNotEqualComparison()
        {
            return valueOne.Value != valueTwo.Value;
        }
    }

    public abstract class AnimationFloatConditional<T> : AnimationConditionalDataSetter<T> where T : IAnimatorParameterSetter
    {
        [SerializeField]
        private FloatReference valueOne;
        [SerializeField]
        private FloatReference valueTwo;

        public override bool OnEqualComparison()
        {
            return valueOne.Value == valueTwo.Value;
        }

        public override bool OnLessThanComparison()
        {
            return valueOne.Value < valueTwo.Value;
        }

        public override bool OnLessThanOrEqualComparison()
        {
            return valueOne <= valueTwo.Value;
        }

        public override bool OnMoreThanComparison()
        {
            return valueOne > valueTwo.Value;
        }

        public override bool OnMoreThanOrEqualComparison()
        {
            return valueOne >= valueTwo.Value;
        }

        public override bool OnNotEqualComparison()
        {
            return valueOne.Value != valueTwo.Value;
        }
    }
}
