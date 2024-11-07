using Codice.CM.SEIDInfo;
using ModularData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI
{
    [CreateAssetMenu(fileName = "New Select Target State", menuName = "ScriptableObjects/State/Select Target")]
    public class ST_SelectTarget : State, IBlackboardDITarget
    {
        [SerializeField]
        private FloatReferenceType test1;

        [SerializeField]
        private FloatReferenceType test2;

        [SerializeField]
        private FloatReferenceType test3;


        public override bool IsBlackboardValidForState(BlackboardBase data)
        {
            return data.ContainsKey(test1) && data.ContainsKey(test2) && data.ContainsKey(test3);
        }

        public override void RunState()
        {

        }

        public void SetBlackboardReferences(LinkedReferenceTypes references)
        {
            IBlackboardDITarget.SetReferencesBasedOnType(ref references, ref test1);
            IBlackboardDITarget.SetReferencesBasedOnType(ref references, ref test2);
            IBlackboardDITarget.SetReferencesBasedOnType(ref references, ref test3);

            Debug.Log(test1);
            Debug.Log(test2);
            Debug.Log(test3);
        }
    }
}
