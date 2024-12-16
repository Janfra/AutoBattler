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
        public List<DynamicReferenceType> linkedReferences;
    }

    public class BlackboardDI : BlackboardBase
    {
        [Serializable]
        public struct BlackboardTargetData
        {
            public DynamicReference reference;

            public bool makeUnique;

            [BlackboardReferenceConstraint(typeof(BlackboardDETargetReferenceType))]
            public List<DynamicReference> linkedTargets;

            public BlackboardTargetData GetWithUniqueKey()
            {
                reference = reference.GetWithUniqueConstraint();
                return this;
            }
        }

        [NonReorderable]
        [SerializeField]
        protected List<BlackboardTargetData> targets;

        public override DynamicReference[] GetDataContainersCopy()
        {
            List<DynamicReference> data = new List<DynamicReference>();
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
                foreach (DynamicReference linkedTarget in targetData.linkedTargets)
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
                        linkedReferenceTypes.linkedReferences = new List<DynamicReferenceType>
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

        protected override void SetDataContainers(DynamicReference[] newContainers)
        {
            throw new NotImplementedException();
        }
    }

    public interface IBlackboardDITarget
    {
        public void SetBlackboardReferences(LinkedReferenceTypes references);
        static protected void SetReferencesBasedOnType<T>(ref LinkedReferenceTypes references, ref T targetField) where T : DynamicReferenceType
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
