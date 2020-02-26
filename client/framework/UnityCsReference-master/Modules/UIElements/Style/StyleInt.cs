// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements
{
    public struct StyleInt : IStyleValue<int>, IEquatable<StyleInt>
    {
        public int value
        {
            get { return m_Keyword == StyleKeyword.Undefined ? m_Value : default(int); }
            set
            {
                m_Value = value;
                m_Keyword = StyleKeyword.Undefined;
            }
        }

        public StyleKeyword keyword
        {
            get { return m_Keyword; }
            set { m_Keyword = value; }
        }

        public StyleInt(int v)
            : this(v, StyleKeyword.Undefined)
        {}

        public StyleInt(StyleKeyword keyword)
            : this(default(int), keyword)
        {}

        internal StyleInt(int v, StyleKeyword keyword)
        {
            m_Keyword = keyword;
            m_Value = v;
        }

        private StyleKeyword m_Keyword;
        private int m_Value;

        public static bool operator==(StyleInt lhs, StyleInt rhs)
        {
            return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
        }

        public static bool operator!=(StyleInt lhs, StyleInt rhs)
        {
            return !(lhs == rhs);
        }

        public static implicit operator StyleInt(StyleKeyword keyword)
        {
            return new StyleInt(keyword);
        }

        public static implicit operator StyleInt(int v)
        {
            return new StyleInt(v);
        }

        public bool Equals(StyleInt other)
        {
            return other == this;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is StyleInt))
            {
                return false;
            }

            var v = (StyleInt)obj;
            return v == this;
        }

        public override int GetHashCode()
        {
            var hashCode = 917506989;
            hashCode = hashCode * -1521134295 + m_Keyword.GetHashCode();
            hashCode = hashCode * -1521134295 + m_Value.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return this.DebugString();
        }
    }
}
