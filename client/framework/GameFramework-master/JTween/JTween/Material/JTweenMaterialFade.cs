using DG.Tweening;
using UnityEngine;
using Json;

namespace JTween.Material {
    public class JTweenMaterialFade : JTweenBase {
        private Color m_beginColor = Color.white;
        private float m_toAlpha = 0;
        private string m_property = string.Empty;
        private int m_propertyID = -1;
        private UnityEngine.Material m_Material;

        public JTweenMaterialFade() {
            m_tweenType = (int)JTweenMaterial.Fade;
            m_tweenElement = JTweenElement.Material;
        }

        public Color BeginColor {
            get {
                return m_beginColor;
            }
            set {
                m_beginColor = value;
            }
        }

        public float ToAlpha {
            get {
                return m_toAlpha;
            }
            set {
                m_toAlpha = value;
            }
        }

        public string Property {
            get {
                return m_property;
            }
            set {
                m_property = value;
            }
        }

        public int PropertyID {
            get {
                return m_propertyID;
            }
            set {
                m_propertyID = value;
            }
        }

        protected override void Init() {
            if (null == m_target) return;
            // end if
            var renderer = m_target.GetComponent<Renderer>();
            if (null != renderer) m_Material = renderer.material;
            // end if
            if (null == m_Material) return;
            // end if
            m_beginColor = m_Material.color;
        }

        protected override Tween DOPlay() {
            if (null == m_Material) return null;
            // end if
            if (!string.IsNullOrEmpty(m_property)) {
                return m_Material.DOFade(m_toAlpha, m_property, m_duration);
            } else if (m_propertyID != -1) {
                return m_Material.DOFade(m_toAlpha, m_propertyID, m_duration);
            }
            return m_Material.DOFade(m_toAlpha, m_duration);
        }

        public override void Restore() {
            if (null == m_Material) return;
            // end if
            if (!string.IsNullOrEmpty(m_property)) {
                m_Material.SetColor(m_property, m_beginColor);
            } else if (m_propertyID != -1) {
                m_Material.SetColor(m_propertyID, m_beginColor);
            } // end if
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginColor")) BeginColor = JTweenUtils.JsonToColor(json.GetNode("beginColor"));
            // end if
            if (json.Contains("alpha")) m_toAlpha = json.GetFloat("alpha");
            // end if
            if (json.Contains("property")) m_property = json.GetString("property");
            // end if
            if (json.Contains("propertyID")) m_propertyID = json.GetInt("propertyID");
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetNode("beginColor", JTweenUtils.ColorJson(m_beginColor));
            json.SetFloat("alpha", m_toAlpha);
            if (!string.IsNullOrEmpty(m_property)) {
                json.SetString("property", m_property);
            } // end if
            if (-1 != m_propertyID) {
                json.SetInt("propertyID", m_propertyID);
            } // end if
        }

        protected override bool CheckValid(out string errorInfo) {
            if (null == m_Material) {
                errorInfo = GetType().FullName + " GetComponent<Renderer> is null or material is null";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
