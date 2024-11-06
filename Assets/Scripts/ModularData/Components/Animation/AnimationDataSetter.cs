using ModularData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    public abstract class AnimationDataSetter : ScriptableObject
    {
        [SerializeField]
        private string parameterName;
        protected string ParameterName { get { return parameterName; } }

        [NonSerialized]
        protected Animator animator;

        public virtual void Init(Animator newAnimator)
        {
            SetAnimator(newAnimator);

            bool isNameFound = false;
            foreach (var param in animator.parameters)
            {
                if (param.name == parameterName)
                {
                    Debug.Log("param found");
                    isNameFound = true;
                    break;
                }
            }

            if (!isNameFound)
            {
                Debug.LogError("Parameter name not found inside animator for " + animator.gameObject.name);
            }
        }

        public void OnInvokeEvent()
        {
            UpdateAnimatorData();
        }

        public virtual void OnUpdate() { }

        public abstract void UpdateAnimatorData();

        protected void SetAnimator(Animator newAnimator)
        {
            animator = newAnimator;
        }
    }

    public abstract class AnimatorEventDataSetter<T> : AnimationDataSetter where T : IAnimatorParameterSetter
    {
        [SerializeField]
        private T parameterSetter;

        public override void UpdateAnimatorData()
        {
            if (animator == null)
            {
                Debug.LogError("Animator is null inside animation data setter - " + name);
                return;
            }

            parameterSetter.SetAnimatorParameter(animator, ParameterName);
        }
    }

    public abstract class AnimationConditionalDataSetter<T> : AnimationDataSetter where T : IAnimatorParameterSetter
    {
        private delegate bool ConditionCheck();

        [SerializeField]
        private EComparisonOptions comparisonType;

        [SerializeField]
        private T parameterSetter;

        private ConditionCheck onConditionCheck;

        public override void Init(Animator newAnimator)
        {
            base.Init(newAnimator);
            SetConditionCheck();
        }

        public bool IsConditionMet()
        {
            return onConditionCheck.Invoke();
        }

        public override void OnUpdate()
        {
            if (IsConditionMet())
            {
                UpdateAnimatorData();
            }
        }

        public override void UpdateAnimatorData()
        {
            if (animator == null)
            {
                Debug.LogError("Animator is null inside animation data setter - " + name);
                return;
            }

            parameterSetter.SetAnimatorParameter(animator, ParameterName);
        }

        public abstract bool OnEqualComparison();

        public abstract bool OnNotEqualComparison();

        public abstract bool OnLessThanComparison();

        public abstract bool OnLessThanOrEqualComparison();

        public abstract bool OnMoreThanComparison();

        public abstract bool OnMoreThanOrEqualComparison();

        private void SetConditionCheck()
        {
            switch (comparisonType)
            {
                case EComparisonOptions.Equal:
                    onConditionCheck = OnEqualComparison;
                    break;
                case EComparisonOptions.NotEqual:
                    onConditionCheck = OnNotEqualComparison;
                    break;
                case EComparisonOptions.LessThan:
                    onConditionCheck = OnLessThanComparison;
                    break;
                case EComparisonOptions.LessThanOrEqual:
                    onConditionCheck = OnLessThanOrEqualComparison;
                    break;
                case EComparisonOptions.MoreThan:
                    onConditionCheck = OnMoreThanComparison;
                    break;
                case EComparisonOptions.MoreThanOrEqual:
                    onConditionCheck = OnMoreThanOrEqualComparison;
                    break;

                default:
                    Debug.LogError("Comparison condition given has not been defined in Animation Data: IsConditionMet");
                    return;
            }
        }
    }
}