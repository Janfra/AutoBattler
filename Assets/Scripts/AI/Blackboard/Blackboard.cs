using ModularData;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameAI
{
    [Serializable]
    public struct BoardReferenceData
    {
        [SerializeField]
        private BlackboardReferenceType constraint;

        [ReadOnlyInspector]
        [SerializeField]
        private Object objectReference;

        public Object GetReference()
        {
            return objectReference;
        }

        public bool HasValidConstraint()
        {
            return constraint != null;
        }

        public bool HasValidReference()
        {
            return objectReference != null;
        }

        public bool SetReference(Object reference)
        {
            if (reference == null)
            {
                return false;
            }

            if (!constraint.IsObjectValid(reference))
            {
                return false;
            }

            objectReference = reference;
            return true;
        }

        public Type GetExpectedType()
        {
            if (!HasValidConstraint())
            {
                return null;
            }

            return constraint.GetDataObjectType();
        }

        public BlackboardReferenceType GetReferenceTypeConstraint()
        {
            return constraint;
        }
        
        public void ValidateReference()
        {
            if (!constraint || !constraint.IsObjectValid(objectReference))
            {
                objectReference = null;
            }
        }

        public BoardReferenceData GetWithUniqueConstraint()
        {
            if(constraint == null)
            {
                return new BoardReferenceData();
            }

            constraint = ScriptableObject.CreateInstance<BlackboardReferenceType>();
            return this;
        }
    }

    public class Blackboard : BlackboardBase
    {
        [SerializeField]
        private BoardReferenceData[] referencesData;

        public override BoardReferenceData[] GetDataContainersCopy()
        {
            return referencesData;
        }
    }

    public abstract class BlackboardBase : MonoBehaviour
    {
        private Dictionary<BlackboardReferenceType, Object> sharedData = new Dictionary<BlackboardReferenceType, Object>();

        private void Awake()
        {
            if (sharedData == null)
            {
                sharedData = new Dictionary<BlackboardReferenceType, Object>();
            }

            PopulateBlackboard();
            
        }

        public object this[BlackboardReferenceType key]
        {
            get => !sharedData.ContainsKey(key) ? null : sharedData[key];
            protected set => sharedData[key] = (Object)value;
        }

        public bool ContainsKey(BlackboardReferenceType key) => sharedData.ContainsKey(key);

        public T TryGetValue<T>(BlackboardReferenceType key, T defaultValue)
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

        public abstract BoardReferenceData[] GetDataContainersCopy();

        public void AddReference(BoardReferenceData newReference)
        {
            if (newReference.HasValidReference())
            {
                sharedData[newReference.GetReferenceTypeConstraint()] = newReference.GetReference();
            }
        }

        protected void PopulateBlackboard()
        {
            BeforeInit();

            foreach (BoardReferenceData referenceData in GetDataContainersCopy())
            {
                referenceData.ValidateReference();
                if (referenceData.HasValidReference())
                {
                    BlackboardReferenceType constraintType = referenceData.GetReferenceTypeConstraint();
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

            AfterInit();
        }

        protected virtual void BeforeInit() { }

        protected virtual void AfterInit() { }
    }
}
