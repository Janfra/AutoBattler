using System;
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

    public abstract class AnimationIntProviderConditional<T> : AnimationConditionalDataSetter<T>, IRuntimeScriptableObject where T : IAnimatorParameterSetter
    {
        [SerializeField]
        private SharedIntDataProvider valueOne;
        [SerializeField]
        private IntReference valueTwo;

        private int GetProviderValue()
        {
            if (valueOne.Value == null)
            {   
                throw new System.NullReferenceException($"{GetType().Name} does not have a valid data provider set, please set it before using it.");
            }

            IDataProvider<int> provider = valueOne.Value;
            return provider.OnProvideData();
        }

        public override bool OnEqualComparison()
        {
            return GetProviderValue() == valueTwo.Value;
        }

        public override bool OnLessThanComparison()
        {
            return GetProviderValue() < valueTwo.Value;
        }

        public override bool OnLessThanOrEqualComparison()
        {
            return GetProviderValue() <= valueTwo.Value;
        }

        public override bool OnMoreThanComparison()
        {
            return GetProviderValue() > valueTwo.Value;
        }

        public override bool OnMoreThanOrEqualComparison()
        {
            return GetProviderValue() >= valueTwo.Value;
        }

        public override bool OnNotEqualComparison()
        {
            return GetProviderValue() != valueTwo.Value;
        }

        public void OnReplaceReferences(ReferenceReplacer<ScriptableObject, IRuntimeScriptableObject> replacer)
        {
            if (replacer.HasBeenReplaced(this))
            {
                return;
            }

            replacer.SetReference(ref valueOne);
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
