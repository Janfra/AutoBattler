using ModularData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    public abstract class AnimatorDataSetter : ScriptableObject
    {
        [SerializeField]
        private bool isPerFrameCheck;
        public bool IsPerFrameCheck => isPerFrameCheck;

        [NonSerialized]
        protected Animator animator;

        public virtual void Init(Animator newAnimator)
        {
            SetAnimator(newAnimator);

            string[] targetParameters = GetTargetParameters();
            foreach (var param in targetParameters)
            {
                CheckForParameter(param);
            }
        }

        public abstract string[] GetTargetParameters();

        public virtual void Enabled() { }

        public virtual void Disabled() { }

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

        private void CheckForParameter(string parameterName)
        {
            if (animator == null)
            {
                throw new NullReferenceException($"Animator is null inside animation data setter - {name}");
            }

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
                Debug.LogError($"Parameter name '{parameterName}' not found inside animator for {animator.gameObject.name} using controller {animator.runtimeAnimatorController.name}");
                isPerFrameCheck = false;
            }
        }
    }

    #region One Param
    public abstract class OneParamAnimDS<T> : AnimatorDataSetter where T : IAnimatorParameterSetter
    {
        [SerializeField]
        private string parameterName;
        protected string ParameterName => parameterName;

        [SerializeField]
        protected T parameterSetter;

        public sealed override string[] GetTargetParameters()
        {
            return new string[]{ parameterName };
        }

        public override void UpdateAnimatorData()
        {
            if (animator == null)
            {
                throw new NullReferenceException($"Animator is null inside animation data setter - {name}");
            }

            parameterSetter.SetAnimatorParameter(animator, ParameterName);
        }
    }

    public abstract class DynamicOneParamAnimDS<T> : OneParamAnimDS<T> where T : IAnimatorParameterSetter
    {
        public override void OnUpdate()
        {
            SetParameterSetter();
            UpdateAnimatorData();
        }

        public abstract void SetParameterSetter();
    }

    public abstract class ConditionalOneParamAnimDS<T> : OneParamAnimDS<T> where T : IAnimatorParameterSetter
    {
        private delegate bool ConditionCheck();

        [SerializeField]
        private EComparisonOptions comparisonType;

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

    // Not used for now to avoid duplicating the condition checks, adding it for demonstration purposes. May move condition checks to a separate class for easier reusability.
    public abstract class ConditionResultOneParamAnimDS : ConditionalOneParamAnimDS<AnimatorBoolConditionResultParameter>
    {
        public override void OnUpdate()
        {
            parameterSetter.SetParameterTo = IsConditionMet();
            UpdateAnimatorData();
        }
    }
    #endregion

    #region Two Param
    public abstract class TwoParamAnimDS<T> : AnimatorDataSetter where T : IAnimatorParameterSetter
    {
        [SerializeField]
        private string firstParameterName;
        protected string FirstParameterName => firstParameterName;
        [SerializeField]
        protected T firstParameterSetter;

        [SerializeField]
        private string secondParameterName;
        protected string SecondParameterName => secondParameterName;
        [SerializeField]
        protected T secondParameterSetter;

        public sealed override string[] GetTargetParameters()
        {
            return new string[] { firstParameterName, secondParameterName };
        }

        public override void UpdateAnimatorData()
        {
            if (animator == null)
            {
                throw new NullReferenceException($"Animator is null inside animation data setter - {name}");
            }

            firstParameterSetter.SetAnimatorParameter(animator, FirstParameterName);
            secondParameterSetter.SetAnimatorParameter(animator, SecondParameterName);
        }
    }

    public abstract class DynamicTwoParamAnimDS<T> : TwoParamAnimDS<T> where T : IAnimatorParameterSetter
    {
        public override void OnUpdate()
        {
            SetParameterSetters();
            UpdateAnimatorData();
        }

        public abstract void SetParameterSetters();
    }

    #endregion
}