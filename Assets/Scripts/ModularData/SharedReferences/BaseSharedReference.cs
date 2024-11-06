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
