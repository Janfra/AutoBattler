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
        private List<DynamicReference> uniqueReferencers;
        [SerializeField]
        private List<DynamicReferenceType> uniqueReferences;

        private void Awake()
        {
            if (blackboard == null)
            {
                return;
            }

            Dictionary<DynamicReferenceType, DynamicReferenceType> referenceReplacements = new Dictionary<DynamicReferenceType, DynamicReferenceType>();
            foreach (var reference in uniqueReferences)
            {
                DynamicReferenceType uniqueReference = Instantiate(reference);
                uniqueReference.name = reference.name + " as Unique";
                referenceReplacements.Add(reference, uniqueReference);
            }

            BlackboardReferenceReplacer replacer = new BlackboardReferenceReplacer(referenceReplacements);
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
