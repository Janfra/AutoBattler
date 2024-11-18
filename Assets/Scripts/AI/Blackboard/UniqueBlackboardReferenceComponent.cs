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

        [BlackboardReferenceConstraint(typeof(IUniqueBlackboardReferenceType))]
        [SerializeField]
        private List<BoardReferenceData> uniqueReferencers;
        [SerializeField]
        private List<BlackboardReferenceType> uniqueReferences;

        private void Awake()
        {
            if (blackboard == null)
            {
                return;
            }

            Dictionary<BlackboardReferenceType, BlackboardReferenceType> referenceReplacements = new Dictionary<BlackboardReferenceType, BlackboardReferenceType>();
            foreach (var reference in uniqueReferences)
            {
                BlackboardReferenceType uniqueReference = Instantiate(reference);
                uniqueReference.name = reference.name + " as Unique";
                referenceReplacements.Add(reference, uniqueReference);
            }

            ReferenceReplacer replacer = new ReferenceReplacer(referenceReplacements);
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
