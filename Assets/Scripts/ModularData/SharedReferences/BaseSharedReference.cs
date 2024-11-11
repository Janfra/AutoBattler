using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ModularData
{
    public abstract class SharedValue<ValueType> : ScriptableObject
    {
        public delegate void valueChanged();
        public event valueChanged OnValueChanged;

#if UNITY_EDITOR
        public bool shouldReset;
        public void CheckReset()
        {
            if (shouldReset)
            {
                Reset();
            }
        }
        private void Reset()
        {
            value = default;
        }
#endif

        public ValueType Value {
            get { return value; }
            set 
            { 
                this.value = value;
                if (OnValueChanged != null)
                {
                    OnValueChanged.Invoke();
                }
            } 
        }

        [SerializeField]    
        private ValueType value;
    }

    public abstract class SharedList<ValueType> : ScriptableObject
    {
        public delegate void valueAdded();
        public delegate void valueElementChanged(ValueType newValue, int valueIndex);
        public event valueAdded OnValueAdded;
        public event valueElementChanged OnValueElementChanged;

        public int Count => value.Count;

#if UNITY_EDITOR
        public bool shouldReset;
        public void CheckReset()
        {
            if (shouldReset)
            {
                Reset();
            }
        }
        private void Reset()
        {
            value.Clear();
        }
#endif

        public ValueType GetValueAtIndex(int index)
        {
            if (index >= value.Count)
            {
                return default;
            }

            return value[index];
        }

        public void SetValueAtIndex(int index, ValueType newValue)
        {
            if (index >= value.Count)
            {
                return;
            }

            value[index] = newValue;
            OnValueElementChanged?.Invoke(newValue, index);
        }

        public void AddValue(ValueType newValue)
        {
            value.Add(newValue);
            OnValueAdded?.Invoke();
        }

        public void Clear()
        {
            value.Clear();
        }

        public ValueType[] ToArray()
        {
            return value.ToArray();
        }

        [SerializeField]
        private List<ValueType> value;
    }

    public abstract class SharedLockableValue<ValueType> : ScriptableObject
    {
        public struct LockHandle
        {
            const int INVALID_INDEX = -1;
            int index;

            public LockHandle(int setIndex = INVALID_INDEX)
            {
                index = setIndex;
            }

            public bool IsValid()
            {
                return index != INVALID_INDEX;
            }

            public bool IsCorrectHandle(LockHandle otherHandle)
            {
                return index == otherHandle.index;
            }
        }

        private LockHandle requiredKey;
        private ValueType value;
        public ValueType Value { get { return value; } }

        public LockHandle LockSharedValue(UnityEngine.Object LockOwner)
        {
            if (!requiredKey.IsValid())
            {
                return new LockHandle();
            }

            requiredKey = new LockHandle(LockOwner.GetHashCode());
            return requiredKey;
        }

        public bool IsKeyRequired()
        {
            return requiredKey.IsValid();
        }

        public void SetValue(ValueType newValue, LockHandle key = new LockHandle())
        {
            if (IsKeyRequired())
            {
                if (!key.IsCorrectHandle(requiredKey))
                {
                    return;
                }
            }

            value = newValue;
        }
    }

    [Serializable]
    public abstract class SharedReference<ValueType>
    {
        [SerializeField]
        private bool IsConstant = true;
        [SerializeField]
        protected ValueType ConstantValue;
        [SerializeField]
        protected SharedValue<ValueType> SharedValue;

        public virtual ValueType Value 
        {
            get { return IsConstant ? ConstantValue : SharedValue.Value; }
            set
            {
                if (!IsConstant)
                {
                    SharedValue.Value = value;
                }
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
