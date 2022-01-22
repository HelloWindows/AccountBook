using DG.Tweening;
using UnityEngine;
using Json;

namespace JTween.Material {
    public class JTweenMaterialVector : JTweenBase {
        private Vector4 m_beginVector = Vector4.zero;
        private Vector4 m_toVector = Vector4.zero;
        private string m_property = string.Empty;
        private int m_propertyID = -1;
        private UnityEngine.Material m_Material;

        public JTweenMaterialVector() {
            m_tweenType = (int)JTweenMaterial.Vector;
            m_tweenElement = JTweenElement.Material;
        }

        public Vector4 BeginVector {
            get {
                return m_beginVector;
            }
            set {
                m_beginVector = value;
            }
        }

        public Vector4 ToVector {
            get {
                return m_toVector;
            }
            set {
                m_toVector = value;
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
                m_beginVector = m_Material.GetVector(m_property);
            } else if (-1 != m_propertyID) {
                m_beginVector = m_Material.GetVector(m_propertyID);
            } // end if
        }

        protected override Tween DOPlay() {
            if (null == m_Material) return null;
            // end if
            if (!string.IsNullOrEmpty(m_property)) {
                return m_Material.DOVector(m_toVector, m_property, m_duration);
            } else if (m_propertyID != -1) {
                return m_Material.DOVector(m_toVector, m_propertyID, m_duration);
            } // end if
            return null;
        }

        public override void Restore() {
            if (null == m_Material) return;
            // end if
            if (!string.IsNullOrEmpty(m_property)) {
                m_Material.SetVector(m_property, m_beginVector);
            } else if (-1 != m_propertyID) {
                m_Material.SetVector(m_propertyID, m_beginVector);
            } // end if
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("vector")) m_toVector = JTweenUtils.JsonToVector4(json.GetNode("vector"));
            // end if
            if (json.Contains("property")) m_property = json.GetString("property");
            // end if
            if (json.Contains("propertyID")) m_propertyID = json.GetInt("propertyID");
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetNode("vector", JTweenUtils.Vector4Json(m_toVector));
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
