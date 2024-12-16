using ModularData;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameAI
{
    public class Blackboard : BlackboardBase
    {
        [SerializeField]
        private DynamicReference[] referencesData;

        public override DynamicReference[] GetDataContainersCopy()
        {
            return referencesData;
        }

        protected override void SetDataContainers(DynamicReference[] newContainers)
        {
            referencesData = newContainers;
        }
    }

    public abstract class BlackboardBase : MonoBehaviour, IUniqueBlackboardReferencer
    {
        public delegate void OnReplace(DynamicReference[] newContainers);
        public delegate void Populated();

        public event Populated OnPopulated;

        private Dictionary<DynamicReferenceType, Object> sharedData = new Dictionary<DynamicReferenceType, Object>();

        private void Start()
        {
            if (sharedData == null)
            {
                sharedData = new Dictionary<DynamicReferenceType, Object>();
            }

            PopulateBlackboard();
        }

        public object this[DynamicReferenceType key]
        {
            get => !sharedData.ContainsKey(key) ? null : sharedData[key];
            protected set => sharedData[key] = (Object)value;
        }

        public bool ContainsKey(DynamicReferenceType key) => sharedData.ContainsKey(key);

        public T TryGetValue<T>(DynamicReferenceType key, T defaultValue)
        {
            if (!ContainsKey(key))
            {
                return defaultValue;
            }

            if (sharedData[key] is T t)
            {
                return t;
            }

            return defaultValue;
        }

        public abstract DynamicReference[] GetDataContainersCopy();

        public void AddReference(DynamicReference newReference)
        {
            if (newReference.HasValidReference())
            {
                sharedData[newReference.GetReferenceTypeConstraint()] = newReference.GetReference();
            }
        }

        protected void PopulateBlackboard()
        {
            BeforeInit();

            foreach (DynamicReference referenceData in GetDataContainersCopy())
            {
                referenceData.ValidateReference();
                if (referenceData.HasValidReference())
                {
                    DynamicReferenceType constraintType = referenceData.GetReferenceTypeConstraint();
                    if (sharedData.ContainsKey(constraintType))
                    {
                        Debug.LogWarning("Same blackboard constraint key given more than once, keeping the first seen");
                        continue;
                    }

                    sharedData[constraintType] = referenceData.GetReference();
                    Debug.Log("Added reference to: " + referenceData.GetReference().name);
                }
                else
                {
                    throw new NullReferenceException($"Blackboard containts an invalid reference - Object Name: {name}");
                }
            }

            OnPopulated?.Invoke();

            AfterInit();
        }

        protected virtual void BeforeInit() { }

        protected virtual void AfterInit() { }

        /// <summary>
        /// Purely for replacing references for interface before the blackboard is populated
        /// </summary>
        /// <param name="newContainers"></param>
        protected abstract void SetDataContainers(DynamicReference[] newContainers);

        public void OnReplaceReferences(ReferenceReplacer replacer)
        {
            if (replacer.HasBeenReplaced(this))
            {
                return;
            }

            if (sharedData.Count == 0)
            {
                replacer.SetBlackboardContainers(this, SetDataContainers);
            }
            else
            {
                replacer.SetBlackboardReferences(ref sharedData);
            }
        }
    }
}
