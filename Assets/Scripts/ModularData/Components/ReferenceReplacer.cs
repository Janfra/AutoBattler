using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    public class ReferenceReplacer<ReplacementType, ObjectReferenceType> where ReplacementType : ScriptableObject
    {
        protected Dictionary<ReplacementType, ReplacementType> referenceReplaceData;
        protected List<ObjectReferenceType> replaced = new List<ObjectReferenceType>();

        public ReferenceReplacer(Dictionary<ReplacementType, ReplacementType> referenceReplaceData)
        {
            this.referenceReplaceData = referenceReplaceData;
        }

        public ReferenceReplacer(List<ReplacementType> originalReferenceList)
        {
            InitialiseReplacementData(originalReferenceList.ToArray());
        }

        public ReferenceReplacer(ReplacementType[] originalReferenceArray)
        {
            InitialiseReplacementData(originalReferenceArray);
        }

        public void SetReference<T>(ref T original) where T : ReplacementType
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

        public bool HasBeenReplaced(ObjectReferenceType replacer)
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

        public bool ContainsReference(ReplacementType referenceType)
        {
            if (referenceType == null)
            {
                throw new System.ArgumentNullException($"{nameof(ContainsReference)} method does not take in null arguments, part of {GetType().Name} component");
            }

            return referenceReplaceData.ContainsKey(referenceType);
        }

        private void InitialiseReplacementData(ReplacementType[] originalReferenceArray)
        {
            referenceReplaceData = new Dictionary<ReplacementType, ReplacementType>();
            foreach (var reference in originalReferenceArray)
            {
                ReplacementType uniqueReference = ScriptableObject.Instantiate(reference);
                uniqueReference.name = reference.name + " as Unique";
                referenceReplaceData.Add(reference, uniqueReference);
            }
        }
    }
}
