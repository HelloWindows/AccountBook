using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Material {
    public class JTweenMaterialTiling : JTweenBase {
        private Vector2 m_beginTiling = Vector2.zero;
        private Vector2 m_toTiling = Vector2.zero;
        private string m_property = string.Empty;
        private int m_propertyID = -1;
        private UnityEngine.Material m_Material;

        public JTweenMaterialTiling() {
            m_tweenType = (int)JTweenMaterial.Tiling;
            m_tweenElement = JTweenElement.Material;
        }

        public Vector2 BeginTiling {
            get {
                return m_beginTiling;
            }
            set {
                m_beginTiling = value;
                if (m_Material != null) {
                    if (!string.IsNullOrEmpty(m_property)) {
                        m_Material.SetTextureScale(m_property, m_beginTiling);
                    } else if (-1 != m_propertyID) {
                        m_Material.SetTextureScale(m_propertyID, m_beginTiling);
                    } // end if
                } // end if
            }
        }

        public Vector2 ToTiling {
            get {
                return m_toTiling;
            }
            set {
                m_toTiling = value;
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
                m_beginTiling = m_Material.GetTextureScale(m_property);
            } else if (-1 != m_propertyID) {
                m_beginTiling = m_Material.GetTextureScale(m_propertyID);
            } // end if
        }

        protected override Tween DOPlay() {
            if (null == m_Material) return null;
            // end if
            if (!string.IsNullOrEmpty(m_property)) {
                return m_Material.DOTiling(m_toTiling, m_property, m_duration);
            } // end if
            return m_Material.DOTiling(m_toTiling, m_duration);
        }

        public override void Restore() {
            if (null == m_Material) return;
            // end if
            if (!string.IsNullOrEmpty(m_property)) {
                m_Material.SetTextureScale(m_property, m_beginTiling);
            } else if (-1 != m_propertyID) {
                m_Material.SetTextureScale(m_propertyID, m_beginTiling);
            } // end if
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("beginTiling")) BeginTiling = JTweenUtils.JsonToVector2(json["beginTiling"]);
            // end if
            if (json.Contains("tiling")) m_toTiling = JTweenUtils.JsonToVector2(json["tiling"]);
            // end if
            if (json.Contains("property")) m_property = (string)json["property"];
            // end if
            if (json.Contains("propertyID")) m_propertyID = (int)json["propertyID"];
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["beginTiling"] = JTweenUtils.Vector2Json(m_beginTiling);
            json["tiling"] = JTweenUtils.Vector2Json(m_toTiling);
            if (!string.IsNullOrEmpty(m_property)) {
                json["property"] = m_property;
            } // end if
            if (-1 != m_propertyID) {
                json["propertyID"] = m_propertyID;
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
