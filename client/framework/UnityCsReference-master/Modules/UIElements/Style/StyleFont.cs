// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.UIElements.StyleSheets;

namespace UnityEngine.UIElements
{
    public struct StyleFont : IStyleValue<Font>, IEquatable<StyleFont>
    {
        public Font value
        {
            get { return m_Keyword == StyleKeyword.Undefined ? m_Value : null; }
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

        public StyleFont(Font v)
            : this(v, StyleKeyword.Undefined)
        {}

        public StyleFont(StyleKeyword keyword)
            : this(null, keyword)
        {}

        internal StyleFont(GCHandle gcHandle, StyleKeyword keyword)
            : this(gcHandle.IsAllocated ? gcHandle.Target as Font : null, keyword)
        {}

        internal StyleFont(Font v, StyleKeyword keyword)
        {
            m_Keyword = keyword;
            m_Value = v;
        }

        private StyleKeyword m_Keyword;
        private Font m_Value;

        public static bool operator==(StyleFont lhs, StyleFont rhs)
        {
            return lhs.m_Keyword == rhs.m_Keyword && lhs.m_Value == rhs.m_Value;
        }

        public static bool operator!=(StyleFont lhs, StyleFont rhs)
        {
            return !(lhs == rhs);
        }

        public static implicit operator StyleFont(StyleKeyword keyword)
        {
            return new StyleFont(keyword);
        }

        public static implicit operator StyleFont(Font v)
        {
            return new StyleFont(v);
        }

        public bool Equals(StyleFont other)
        {
            return other == this;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is StyleFont))
            {
                return false;
            }

            var v = (StyleFont)obj;
            return v == this;
        }

        public override int GetHashCode()
        {
            var hashCode = 917506989;
            hashCode = hashCode * -1521134295 + m_Keyword.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Font>.Default.GetHashCode(m_Value);
            return hashCode;
        }

        public override string ToString()
        {
            return this.DebugString();
        }
    }
}
