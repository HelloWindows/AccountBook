using DG.Tweening;
using UnityEngine;
using Json;

namespace JTween.Material {
    public class JTweenMaterialFloat : JTweenBase {
        private float m_beginFloat = 0;
        private float m_toFloat = 0;
        private string m_property = string.Empty;
        private int m_propertyID = -1;
        private UnityEngine.Material m_Material;

        public JTweenMaterialFloat() {
            m_tweenType = (int)JTweenMaterial.Float;
            m_tweenElement = JTweenElement.Material;
        }

        public float BeginFloat {
            get {
                return m_beginFloat;
            }
            set {
                m_beginFloat = value;
            }
        }

        public float ToFloat {
            get {
                return m_toFloat;
            }
            set {
                m_toFloat = value;
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
            if (!string.IsNullOrEmpty(m_property)) {
                m_beginFloat = m_Material.GetFloat(m_property);
            } else if (m_propertyID != -1) {
                m_beginFloat = m_Material.GetFloat(m_propertyID);
            } // end if
        }

        protected override Tween DOPlay() {
            if (null == m_Material) return null;
            // end if
            if (!string.IsNullOrEmpty(m_property)) {
                return m_Material.DOFloat(m_toFloat, m_property, m_duration);
            } else if (m_propertyID != -1) {
                return m_Material.DOFloat(m_toFloat, m_propertyID, m_duration);
            } // end if
            return null;
        }

        public override void Restore() {
            if (null == m_Material) return;
            // end if
            if (!string.IsNullOrEmpty(m_property)) {
                m_Material.SetFloat(m_property, m_beginFloat);
            } else if (m_propertyID != -1) {
                m_Material.SetFloat(m_propertyID, m_beginFloat);
            } // end if
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginFloat")) BeginFloat = json.GetFloat("beginFloat");
            // end if
            if (json.Contains("float")) m_toFloat = json.GetFloat("float");
            // end if
            if (json.Contains("property")) m_property = json.GetString("property");
            // end if
            if (json.Contains("propertyID")) m_propertyID = json.GetInt("propertyID");
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetFloat("beginFloat", m_beginFloat);
            json.SetFloat("float", m_toFloat);
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
            if (string.IsNullOrEmpty(m_property) && m_propertyID == -1) {
                errorInfo = GetType().FullName + " property and propertyID don't assignment";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
