using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ModularData
{
    [Serializable]
    public class IntReference : SharedReference<int>
    {
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static IntReference operator ++(IntReference a)
        {
            a.Value += 1;
            return a;
        }

        #region Operator Helper Functions
        private static int AddValues(int a, IntReference b)
        {
            return a + b.Value;
        }

        private static float AddValues(float a, IntReference b)
        {
            return a + b.Value;
        }

        private static int SubtractValue(int a, IntReference b)
        {
            return a - b.Value;
        }

        private static float SubtractValue(float a, IntReference b)
        {
            return a - b.Value;
        }
        #endregion

        #region Float Operators Overload
        public static float operator +(float a, IntReference b)
        {
            return AddValues(a, b);
        }

        public static bool operator <(float a, IntReference b)
        {
            return a < b.Value;
        }

        public static bool operator >(float a, IntReference b)
        {
            return a > b.Value;
        }

        public static bool operator <=(float a, IntReference b)
        {
            return a <= b.Value;
        }

        public static bool operator >=(float a, IntReference b)
        {
            return a >= b.Value;
        }

        public static bool operator ==(float a, IntReference b)
        {
            return a == b.Value;
        }

        public static bool operator !=(float a, IntReference b)
        {
            return a != b.Value;
        }

        public static bool operator <(IntReference a, float b)
        {
            return a.Value < b;
        }

        public static bool operator >(IntReference a, float b)
        {
            return a.Value > b;
        }

        public static bool operator <=(IntReference a, float b)
        {
            return a.Value <= b;
        }

        public static bool operator >=(IntReference a, float b)
        {
            return a.Value >= b;
        }

        public static bool operator ==(IntReference a, float b)
        {
            return a.Value == b;
        }

        public static bool operator !=(IntReference a, float b)
        {
            return a.Value != b;
        }
        #endregion

        #region Int Operators Overload

        public static int operator +(int a, IntReference b)
        {
            return AddValues(a, b);
        }

        public static bool operator <(int a, IntReference b)
        {
            return a < b.Value;
        }

        public static bool operator >(int a, IntReference b)
        {
            return a > b.Value;
        }

        public static bool operator <=(int a, IntReference b)
        {
            return a <= b.Value;
        }

        public static bool operator >=(int a, IntReference b)
        {
            return a >= b.Value;
        }

        public static bool operator ==(int a, IntReference b)
        {
            return a == b.Value;
        }

        public static bool operator !=(int a, IntReference b)
        {
            return a != b.Value;
        }

        public static bool operator <(IntReference a, int b)
        {
            return a.Value < b;
        }

        public static bool operator >(IntReference a, int b)
        {
            return a.Value > b;
        }

        public static bool operator <=(IntReference a, int b)
        {
            return a.Value <= b;
        }

        public static bool operator >=(IntReference a, int b)
        {
            return a.Value >= a;
        }

        public static bool operator ==(IntReference a, int b)
        {
            return a.Value == b;
        }

        public static bool operator !=(IntReference a, int b)
        {
            return a.Value != b;
        }
        #endregion
    }
}
