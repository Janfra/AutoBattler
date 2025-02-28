using UnityEngine;

namespace ModularData.Animation
{
    [CreateAssetMenu(fileName = "New Float Provider Conditional", menuName = "ScriptableObjects/AnimatorDataSetter/OneParam/ConditionResult/Float Provider")]
    public class FloatProviderConditionalResultBoolParam_OneParamDS : FloatProviderConditionalOneParamDS<AnimatorBoolConditionResultParameter>
    {
        public override void OnUpdate()
        {
            parameterSetter.SetParameterTo = IsConditionMet();
            UpdateAnimatorData();
        }
    }
}
