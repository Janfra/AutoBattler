using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    [CreateAssetMenu(fileName = "New Dynamic Float Setter", menuName = "ScriptableObjects/AnimatorDataSetter/OneParam/DynamicSetter/Float Provider")]
    public class DynamicFloatProvider_OneParamDS : DynamicOneParamAnimDS<AnimatorDynamicFloatParameter>
    {
        [SerializeField]
        private SharedFloatDataProvider DataProvider;

        public override void SetParameterSetter()
        {
            parameterSetter.SetParameterTo = DataProvider.Value.OnProvideData();
        }
    }
}
