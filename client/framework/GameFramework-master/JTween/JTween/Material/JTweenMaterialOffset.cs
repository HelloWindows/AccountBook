using DG.Tweening;
using UnityEngine;
using Json;

namespace JTween.Material {
    public class JTweenMaterialOffset : JTweenBase {
        private Vector2 m_beginOffset = Vector2.zero;
        private Vector2 m_toOffset = Vector2.zero;
        private string m_property = string.Empty;
        private int m_propertyID = -1;
        private UnityEngine.Material m_Material;

        public JTweenMaterialOffset() {
            m_tweenType = (int)JTweenMaterial.Offset;
            m_tweenElement = JTweenElement.Material;
        }

        public Vector2 BeginOffset {
            get {
                return m_beginOffset;
            }
            set {
                m_beginOffset = value;
            }
        }

        public Vector2 ToOffset {
            get {
                return m_toOffset;
            }
            set {
                m_toOffset = value;
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
                m_beginOffset = m_Material.GetTextureOffset(m_property);
            } else if (-1 != m_propertyID) {
                m_beginOffset = m_Material.GetTextureOffset(m_propertyID);
            } // end if
        }

        protected override Tween DOPlay() {
            if (null == m_Material) return null;
            // end if
            if (!string.IsNullOrEmpty(m_property)) {
                return m_Material.DOOffset(m_toOffset, m_property, m_duration);
            } // end if
            return m_Material.DOOffset(m_toOffset, m_duration);
        }

        public override void Restore() {
            if (null == m_Material) return;
            // end if
            if (!string.IsNullOrEmpty(m_property)) {
                m_Material.SetTextureOffset(m_property, m_beginOffset);
            } else if (-1 != m_propertyID) {
                m_Material.SetTextureOffset(m_propertyID, m_beginOffset);
            } // end if
        }

        protected override void JsonTo(IJsonNode json) {
            if (json.Contains("beginOffset")) BeginOffset = JTweenUtils.JsonToVector2(json.GetNode("beginOffset"));
            // end if
            if (json.Contains("offset")) m_toOffset = JTweenUtils.JsonToVector2(json.GetNode("offset"));
            // end if
            if (json.Contains("property")) m_property = json.GetString("property");
            // end if
            if (json.Contains("propertyID")) m_propertyID = json.GetInt("propertyID");
            // end if
            Restore();
        }

        protected override void ToJson(ref IJsonNode json) {
            json.SetNode("beginOffset", JTweenUtils.Vector2Json(m_beginOffset));
            json.SetNode("offset", JTweenUtils.Vector2Json(m_toOffset));
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
