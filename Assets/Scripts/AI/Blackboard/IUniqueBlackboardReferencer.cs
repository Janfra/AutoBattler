using ModularData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAI
{
    public class ReferenceReplacer
    {
        private Dictionary<BlackboardReferenceType, BlackboardReferenceType> referenceReplaceData;
        private List<IUniqueBlackboardReferencer> replaced = new List<IUniqueBlackboardReferencer>();

        public ReferenceReplacer(Dictionary<BlackboardReferenceType, BlackboardReferenceType> referenceReplaceData) 
        {
            this.referenceReplaceData = referenceReplaceData;
        }

        // Redo the dictionary for now
        public void SetBlackboardReferences(ref Dictionary<BlackboardReferenceType, Object> sharedData)
        {
            Stack<BlackboardReferenceType> keyToRemove = new Stack<BlackboardReferenceType>();
            Stack<Object> objectCopies = new Stack<Object>();

            foreach (var item in sharedData)
            {
                if (ContainsReference(item.Key))
                {
                    objectCopies.Push(item.Value);
                    keyToRemove.Push(item.Key);
                }
            }

            while (keyToRemove.Count > 0)
            {
                BlackboardReferenceType currentKey = keyToRemove.Pop();
                sharedData[referenceReplaceData[currentKey]] = objectCopies.Pop();
                sharedData.Remove(currentKey);
            }
        }
    
        public void SetReference<T>(ref T original) where T : BlackboardReferenceType
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

        private bool ContainsReference(BlackboardReferenceType referenceType)
        {
            return referenceReplaceData.ContainsKey(referenceType);
        }
    }

    public interface IUniqueBlackboardReferencer
    {
        public void OnReplaceReferences(ReferenceReplacer replacer);
    }
}

