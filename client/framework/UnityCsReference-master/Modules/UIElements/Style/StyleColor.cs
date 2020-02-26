// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements
{
    public struct StyleColor : IStyleValue<Color>, IEquatable<StyleColor>
    {
        public Color value
        {
            get { return m_Keyword == StyleKeyword.Undefined ? m_Value : Color.clear; }
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

        public StyleColor(Color v)
            : this(v, StyleKeyword.Undefined)
        {}

        public StyleColor(StyleKeyword keyword)
            : this(Color.clear, keyword)
        {}

        internal StyleColor(Color v, StyleKeyword keyword)
        {
            m_Keyword = keyword;
            m_Value = v;
        }

        private StyleKeyword m_Keyword;
        private Color m_Value;

        public static bool operator==(StyleColor lhs, StyleColor rhs)
        {
            return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
        }

        public static bool operator!=(StyleColor lhs, StyleColor rhs)
        {
            return !(lhs == rhs);
        }

        public static bool operator==(StyleColor lhs, Color rhs)
        {
            var styleColor = new StyleColor(rhs);
            return lhs == styleColor;
        }

        public static bool operator!=(StyleColor lhs, Color rhs)
        {
            return !(lhs == rhs);
        }

        public static implicit operator StyleColor(StyleKeyword keyword)
        {
            return new StyleColor(keyword);
        }

        public static implicit operator StyleColor(Color v)
        {
            return new StyleColor(v);
        }

        public bool Equals(StyleColor other)
        {
            return other == this;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is StyleColor))
            {
                return false;
            }

            var v = (StyleColor)obj;
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
