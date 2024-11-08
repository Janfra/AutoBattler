using ModularData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GameAI
{
    public struct LinkedReferenceTypes
    {
        public List<BlackboardReferenceType> linkedReferences;
    }

    public class BlackboardDI : BlackboardBase
    {
        [Serializable]
        public struct BlackboardTargetData
        {
            public BoardReferenceData reference;

            public bool makeUnique;

            [BlackboardReferenceConstraint(typeof(BlackboardDETargetReferenceType))]
            public List<BoardReferenceData> linkedTargets;

            public BlackboardTargetData GetWithUniqueKey()
            {
                reference = reference.GetWithUniqueConstraint();
                return this;
            }
        }

        [NonReorderable]
        [SerializeField]
        protected List<BlackboardTargetData> targets;

        public override BoardReferenceData[] GetDataContainersCopy()
        {
            List<BoardReferenceData> data = new List<BoardReferenceData>();
            foreach(BlackboardTargetData targetData in targets)
            {
                data.Add(targetData.reference);
            }

            return data.ToArray();
        }

        protected override void BeforeInit()
        {
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i].makeUnique)
                {
                    targets[i] = targets[i].GetWithUniqueKey();
                }
            }

            SetLinkedTargetsReferences();
        }

        private void SetLinkedTargetsReferences()
        {
            Dictionary<IBlackboardDITarget, LinkedReferenceTypes> linkedReferencesPerObject = new Dictionary<IBlackboardDITarget, LinkedReferenceTypes>();
            foreach (BlackboardTargetData targetData in targets)
            {
                foreach (BoardReferenceData linkedTarget in targetData.linkedTargets)
                {
                    IBlackboardDITarget targetInterface = linkedTarget.GetReference() as IBlackboardDITarget;
                    if (targetInterface == null)
                    {
                        Debug.LogError("Target interface was null");
                        continue;
                    }

                    if (linkedReferencesPerObject.ContainsKey(targetInterface))
                    {
                        linkedReferencesPerObject[targetInterface].linkedReferences.Add(targetData.reference.GetReferenceTypeConstraint());
                    }
                    else
                    {
                        LinkedReferenceTypes linkedReferenceTypes = new LinkedReferenceTypes();
                        linkedReferenceTypes.linkedReferences = new List<BlackboardReferenceType>
                        {
                            targetData.reference.GetReferenceTypeConstraint()
                        };
                        linkedReferencesPerObject[targetInterface] = linkedReferenceTypes;
                    }
                }
            }

            foreach (var item in linkedReferencesPerObject)
            {
                item.Key.SetBlackboardReferences(item.Value);
            }
        }
    }

    public interface IBlackboardDITarget
    {
        public void SetBlackboardReferences(LinkedReferenceTypes references);
        static protected void SetReferencesBasedOnType<T>(ref LinkedReferenceTypes references, ref T targetField) where T : BlackboardReferenceType
        {
            for (int i = 0; i < references.linkedReferences.Count; i++)
            {
                if (references.linkedReferences[i] is T t)
                {
                    targetField = t;
                    references.linkedReferences.RemoveAt(i);
                    return;
                }
            }
        }
    }

}
