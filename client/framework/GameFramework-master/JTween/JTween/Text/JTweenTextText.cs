using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Text {
    public class JTweenTextText : JTweenBase {
        private string m_beginStr = string.Empty;
        private string m_toStr = string.Empty;
        private bool m_richTextEnabled = true;
        private ScrambleMode m_scrambleMode = ScrambleMode.None;
        private string m_scrambleChars = string.Empty;
        private UnityEngine.UI.Text m_text;

        public JTweenTextText() {
            m_tweenType = (int)JTweenText.Text;
            m_tweenElement = JTweenElement.Text;
        }

        public string BeginStr {
            get {
                return m_beginStr;
            }
            set {
                m_beginStr = value;
                if (m_beginStr != null) {
                    m_text.text = m_beginStr;
                } // end if
            }
        }

        public string ToStr {
            get {
                return m_toStr;
            }
            set {
                m_toStr = value;
            }
        }
        /// <summary>
        /// If TRUE (default), rich text will be interpreted correctly while animated, otherwise all tags will be considered as normal
        /// text. 
        /// </summary>
        public bool RichTextEnabled {
            get {
                return m_richTextEnabled;
            }
            set {
                m_richTextEnabled = value;
            }
        }
        /// <summary>
        /// The type of scramble mode to use, if any.
        /// If different than ScrambleMode.None the string will appear from a random animation of characters, 
        /// otherwise it will compose itself regularly.
        /// None (default): no scrambling will be applied.
        /// All/Uppercase/Lowercase/Numerals: type of characters to be used while scrambling.
        /// Custom: will use the custom characters in scrambleChars.
        /// </summary>
        public ScrambleMode ScrambleMode {
            get {
                return m_scrambleMode;
            }
            set {
                m_scrambleMode = value;
            }
        }
        /// <summary>
        /// A string containing the characters to use for custom scrambling. Use as many characters as possible (minimum 10) 
        /// because DOTween uses a fast scramble mode which gives better results with more characters. 
        /// </summary>
        public string ScrambleChars {
            get {
                return m_scrambleChars;
            }
            set {
                m_scrambleChars = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            m_text = m_target.GetComponent<UnityEngine.UI.Text>();
            if (null == m_text) return;
            // end if
            m_beginStr = m_text.text;
        }

        protected override Tween DOPlay() {
            if (null == m_text) return null;
            // end if
            return m_text.DOText(m_toStr, m_duration, m_richTextEnabled, m_scrambleMode, m_scrambleChars);
        }

        public override void Restore() {
            if (null == m_text) return;
            // end if
            m_text.text = m_beginStr;
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("beginStr")) BeginStr = json["beginStr"].ToString();
            // end if
            if (json.Contains("str")) m_toStr = json["str"].ToString();
            // end if
            if (json.Contains("rich")) m_richTextEnabled = json["rich"].ToBool();
            // end if
            if (json.Contains("mode")) m_scrambleMode = (ScrambleMode)json["mode"].ToInt32();
            // end if
            if (json.Contains("char")) m_scrambleChars = json["char"].ToString();
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["beginStr"] = m_beginStr;
            json["str"] = m_toStr;
            json["rich"] = m_richTextEnabled;
            json["mode"] = (int)m_scrambleMode;
            json["char"] = m_scrambleChars;
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_text) {
                errorInfo = GetType().FullName + " GetComponent<Text> is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
