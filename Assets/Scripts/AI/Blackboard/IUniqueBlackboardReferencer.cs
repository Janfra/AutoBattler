using ModularData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI
{
    public class ReferenceReplacer
    {
        private Dictionary<DynamicReferenceType, DynamicReferenceType> referenceReplaceData;
        private List<IUniqueBlackboardReferencer> replaced = new List<IUniqueBlackboardReferencer>();

        public ReferenceReplacer(Dictionary<DynamicReferenceType, DynamicReferenceType> referenceReplaceData) 
        {
            this.referenceReplaceData = referenceReplaceData;
        }

        public void SetBlackboardContainers(BlackboardBase blackboard, BlackboardBase.OnReplace containerSetter)
        {
            if (blackboard == null || containerSetter == null)
            {
                return;
            }

            DynamicReference[] referenceContainers = blackboard.GetDataContainersCopy();

            for (int i = 0; i < referenceContainers.Length; i++)
            {
                DynamicReference referenceData = referenceContainers[i];
                DynamicReferenceType key = referenceData.GetReferenceTypeConstraint();

                if (ContainsReference(key))
                {
                    Object reference = referenceData.GetReference();
                    if (reference is ScriptableObject)
                    {
                        reference = ScriptableObject.Instantiate(reference);
                        reference.name += " as Unique";
                    }

                    referenceContainers[i] = new DynamicReference(referenceReplaceData[key], reference);
                }
            }

            containerSetter.Invoke(referenceContainers);
        }

        // Redo the dictionary for now
        public void SetBlackboardReferences(ref Dictionary<DynamicReferenceType, Object> sharedData)
        {
            Stack<DynamicReferenceType> keyToRemove = new Stack<DynamicReferenceType>();
            Stack<Object> objectCopies = new Stack<Object>();

            foreach (var item in sharedData)
            {
                if (ContainsReference(item.Key))
                {
                    if (item.Value is ScriptableObject)
                    {
                        Debug.Log("instantiating");
                        var uniqueReference = ScriptableObject.Instantiate(item.Value);
                        uniqueReference.name += " as Unique";
                        objectCopies.Push(uniqueReference);
                    }
                    else
                    {
                        objectCopies.Push(item.Value);
                    }
                    keyToRemove.Push(item.Key);
                }
            }

            while (keyToRemove.Count > 0)
            {
                DynamicReferenceType currentKey = keyToRemove.Pop();
                sharedData[referenceReplaceData[currentKey]] = objectCopies.Pop();
                sharedData.Remove(currentKey);
            }
        }
    
        public void SetReference<T>(ref T original) where T : DynamicReferenceType
        {
            if (!ContainsReference(original)) return;
            if (referenceReplaceData[original] is T t)
            {
                original = t;
            }
            else
            {
                throw new System.ArgumentException($"{typeof(T).Name} is not the type of the reference set on the original reference type. It is set to: {referenceReplaceData[original].GetType().Name}");
            }
        }

        public bool HasBeenReplaced(IUniqueBlackboardReferencer replacer)
        {
            if (replaced.Contains(replacer))
            {
                return true;
            }
            else
            {
                replaced.Add(replacer);
                return false;
            }
        }

        private bool ContainsReference(DynamicReferenceType referenceType)
        {
            return referenceReplaceData.ContainsKey(referenceType);
        }
    }

    public interface IUniqueBlackboardReferencer
    {
        public void OnReplaceReferences(ReferenceReplacer replacer);
    }
}

