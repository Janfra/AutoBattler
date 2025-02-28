using GameAI;
using ModularData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    public class UniqueBlackboardReferenceComponent : MonoBehaviour
    {
        [SerializeField]
        private BlackboardBase blackboard;

        [DynamicReferenceTypeConstraint(typeof(IUniqueBlackboardReferenceType))]
        [SerializeField]
        private DynamicReference[] uniqueReferencers;
        [SerializeField]
        private DynamicReferenceType[] uniqueReferences;

        private void Awake()
        {
            if (blackboard == null)
            {
                return;
            }

            BlackboardReferenceReplacer replacer = new BlackboardReferenceReplacer(uniqueReferences);
            blackboard.OnReplaceReferences(replacer);
            Debug.Log($"Replaced references in blackboard for: {blackboard.name}");
            foreach (var referencer in uniqueReferencers)
            {
                IUniqueBlackboardReferencer referencerObject = referencer.GetReference() as IUniqueBlackboardReferencer;
                if (referencerObject != null)
                {
                    referencerObject.OnReplaceReferences(replacer);
                    Debug.Log($"Replaced references for: {referencer.GetReference().name}");
                }
            }
        }
    }
}
