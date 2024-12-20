using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace ModularData
{
    public abstract class IntConditionalOneParamAnimDS<T> : ConditionalOneParamAnimDS<T> where T : IAnimatorParameterSetter
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

    public abstract class IntProviderConditionalOneParamDS<T> : ConditionalOneParamAnimDS<T>, IRuntimeScriptableObject where T : IAnimatorParameterSetter
    {
        [SerializeField]
        protected SharedIntDataProvider valueOne;
        [SerializeField]
        protected IntReference valueTwo;

        private int GetProviderValue()
        {
            if (valueOne.Value == null)
            {   
                throw new System.NullReferenceException($"{GetType().Name} does not have a valid data provider set, please set it before using it.");
            }

            IDataProvider<int> provider = valueOne.Value;
            return provider.OnProvideData();
        }

        public sealed override bool OnEqualComparison()
        {
            return GetProviderValue() == valueTwo.Value;
        }

        public sealed override bool OnLessThanComparison()
        {
            return GetProviderValue() < valueTwo.Value;
        }

        public sealed override bool OnLessThanOrEqualComparison()
        {
            return GetProviderValue() <= valueTwo.Value;
        }

        public sealed override bool OnMoreThanComparison()
        {
            return GetProviderValue() > valueTwo.Value;
        }

        public sealed override bool OnMoreThanOrEqualComparison()
        {
            return GetProviderValue() >= valueTwo.Value;
        }

        public sealed override bool OnNotEqualComparison()
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

    public abstract class FloatProviderConditionalOneParamDS<T> : ConditionalOneParamAnimDS<T>, IRuntimeScriptableObject where T : IAnimatorParameterSetter
    {
        [SerializeField]
        protected SharedFloatDataProvider valueOne;
        [SerializeField]
        protected FloatReference valueTwo;

        private float GetProviderValue()
        {
            if (valueOne.Value == null)
            {
                throw new System.NullReferenceException($"{GetType().Name} does not have a valid data provider set, please set it before using it.");
            }

            IDataProvider<float> provider = valueOne.Value;
            return provider.OnProvideData();
        }

        public sealed override bool OnEqualComparison()
        {
            return GetProviderValue() == valueTwo.Value;
        }

        public sealed override bool OnLessThanComparison()
        {
            return GetProviderValue() < valueTwo.Value;
        }

        public sealed override bool OnLessThanOrEqualComparison()
        {
            return GetProviderValue() <= valueTwo.Value;
        }

        public sealed override bool OnMoreThanComparison()
        {
            return GetProviderValue() > valueTwo.Value;
        }

        public sealed override bool OnMoreThanOrEqualComparison()
        {
            return GetProviderValue() >= valueTwo.Value;
        }

        public sealed override bool OnNotEqualComparison()
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

    public abstract class FloatConditionalOneParamDS<T> : ConditionalOneParamAnimDS<T> where T : IAnimatorParameterSetter
    {
        [SerializeField]
        private FloatReference valueOne;
        [SerializeField]
        private FloatReference valueTwo;

        public sealed override bool OnEqualComparison()
        {
            return valueOne.Value == valueTwo.Value;
        }

        public sealed override bool OnLessThanComparison()
        {
            return valueOne.Value < valueTwo.Value;
        }

        public sealed override bool OnLessThanOrEqualComparison()
        {
            return valueOne <= valueTwo.Value;
        }

        public sealed override bool OnMoreThanComparison()
        {
            return valueOne > valueTwo.Value;
        }

        public sealed override bool OnMoreThanOrEqualComparison()
        {
            return valueOne >= valueTwo.Value;
        }

        public sealed override bool OnNotEqualComparison()
        {
            return valueOne.Value != valueTwo.Value;
        }
    }
}
