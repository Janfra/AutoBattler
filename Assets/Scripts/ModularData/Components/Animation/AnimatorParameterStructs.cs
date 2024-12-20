using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    public interface IAnimatorParameterSetter
    {
        void SetAnimatorParameter(Animator animator, string parameterName);
    }

    [Serializable]
    public struct AnimatorBoolParameter : IAnimatorParameterSetter
    {
        public bool SetParameterTo;

        public void SetAnimatorParameter(Animator animator, string parameterName)
        {
            animator.SetBool(parameterName, SetParameterTo);
        }
    }

    // Should be set to the condition result of the data setter
    [Serializable]
    public struct AnimatorBoolConditionResultParameter : IAnimatorParameterSetter
    {
        [HideInInspector]
        public bool SetParameterTo;

        public void SetAnimatorParameter(Animator animator, string parameterName)
        {
            animator.SetBool(parameterName, SetParameterTo);
        }
    }

    [Serializable]
    public struct AnimatorIntParamater : IAnimatorParameterSetter
    {
        public int SetParameterTo;

        public void SetAnimatorParameter(Animator animator, string parameterName)
        {
            animator.SetInteger(parameterName, SetParameterTo);
        }
    }

    [Serializable]
    public struct AnimatorFloatParamater : IAnimatorParameterSetter
    {
        public float SetParameterTo;

        public void SetAnimatorParameter(Animator animator, string parameterName)
        {
            animator.SetFloat(parameterName, SetParameterTo);
        }
    }

    // Must be set dynamically
    [Serializable]
    public struct AnimatorDynamicFloatParameter : IAnimatorParameterSetter
    {
        [HideInInspector]
        public float SetParameterTo;

        public void SetAnimatorParameter(Animator animator, string parameterName)
        {
            animator.SetFloat(parameterName, SetParameterTo);
        }
    }

    [Serializable]
    public struct AnimatorTriggerParameter : IAnimatorParameterSetter
    {
        public void SetAnimatorParameter(Animator animator, string parameterName)
        {
            animator.SetTrigger(parameterName);
        }
    }
}
