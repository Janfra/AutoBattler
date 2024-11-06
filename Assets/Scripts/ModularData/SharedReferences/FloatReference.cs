using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    [Serializable]
    public class FloatReference : SharedReference<float>
    {
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static FloatReference operator ++(FloatReference a)
        {
            a.Value += 1.0f;
            return a;
        }

        #region Operator Helper Functions
        public static int GetValueAsInt(FloatReference a)
        {
            return (int)a.Value;
        }

        private static int AddValues(int a, FloatReference b)
        {
            return a + GetValueAsInt(b);
        }

        private static float AddValues(float a, FloatReference b)
        {
            return a + b.Value;
        }

        private static int SubtractValue(int a, FloatReference b)
        {
            return a - GetValueAsInt(b);
        }

        private static float SubtractValue(float a, FloatReference b)
        {
            return a - b.Value;
        }
    #endregion

    #region Float Operators Overload
        public static float operator+(float a, FloatReference b)
        {
            return AddValues(a, b);
        }

        public static bool operator <(float a, FloatReference b)
        {
            return a < b.Value;
        }

        public static bool operator >(float a, FloatReference b)
        {
            return a > b.Value;
        }

        public static bool operator <=(float a, FloatReference b)
        {
            return a <= b.Value;
        }

        public static bool operator >=(float a, FloatReference b)
        {
            return a >= b.Value;
        }

        public static bool operator ==(float a, FloatReference b)
        {
            return a == b.Value;
        }

        public static bool operator !=(float a, FloatReference b)
        {
            return a != b.Value;
        }

        public static bool operator <(FloatReference a, float b)
        {
            return a.Value < b;
        }

        public static bool operator >(FloatReference a, float b)
        {
            return a.Value > b;
        }

        public static bool operator <=(FloatReference a, float b)
        {
            return a.Value <= b;
        }

        public static bool operator >=(FloatReference a, float b)
        {
            return a.Value >= b;
        }

        public static bool operator ==(FloatReference a, float b)
        {
            return a.Value == b;
        }

        public static bool operator !=(FloatReference a, float b)
        {
            return a.Value != b;
        }
        #endregion

    #region Int Operators Overload

        public static int operator +(int a, FloatReference b)
        {
            return AddValues(a, b);
        }

        public static bool operator <(int a, FloatReference b)
        {
            return a < GetValueAsInt(b);
        }

        public static bool operator >(int a, FloatReference b)
        {
            return a > GetValueAsInt(b);
        }

        public static bool operator <=(int a, FloatReference b)
        {
            return a <= GetValueAsInt(b);
        }

        public static bool operator >=(int a, FloatReference b)
        {
            return a >= GetValueAsInt(b);
        }

        public static bool operator ==(int a, FloatReference b)
        {
            return a == GetValueAsInt(b);
        }

        public static bool operator !=(int a, FloatReference b)
        {
            return a != GetValueAsInt(b);
        }

        public static bool operator <(FloatReference a, int b)
        {
            return GetValueAsInt(a) < b;
        }

        public static bool operator >(FloatReference a, int b)
        {
            return GetValueAsInt(a) > b;
        }

        public static bool operator <=(FloatReference a, int b)
        {
            return GetValueAsInt(a) <= b;
        }

        public static bool operator >=(FloatReference a, int b)
        {
            return GetValueAsInt(a) >= a;
        }

        public static bool operator ==(FloatReference a, int b)
        {
            return GetValueAsInt(a) == b;
        }

        public static bool operator !=(FloatReference a, int b)
        {
            return GetValueAsInt(a) != b;
        }
        #endregion
    }
}
