using UnityEngine;

namespace ModularData.Animation
{
    [CreateAssetMenu(fileName = "New Dynamic Vector Setter", menuName = "ScriptableObjects/AnimatorDataSetter/TwoParam/DynamicSetter/Shared Vector")]
    public class DynamicSharedVector_TwoParamDS : DynamicTwoParamAnimDS<AnimatorDynamicFloatParameter>, IUniqueRuntimeScriptableObject
    {
        [SerializeField]
        private SharedVector2 sharedVector;
        [SerializeField]
        private bool isNormalised;

        public override void SetParameterSetters()
        {
            Vector2 value = sharedVector.Value;
            if (isNormalised) 
            {
                value = value.normalized;
            }

            firstParameterSetter.SetParameterTo = value.x;
            secondParameterSetter.SetParameterTo = value.y;
        }

        public void OnReplaceReferences(ReferenceReplacer<ScriptableObject, IUniqueRuntimeScriptableObject> replacer)
        {
            if (replacer.HasBeenReplaced(this))
            {
                return;
            }

            replacer.SetReference(ref sharedVector);
        }
    }
}
