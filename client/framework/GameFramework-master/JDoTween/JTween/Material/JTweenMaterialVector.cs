using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;
using LitJson;
using UnityEngine;

namespace JTween.Material {
    public class JTweenMaterialVector : JTweenBase {
        private Vector4 m_beginVector = Vector4.zero;
        private Vector4 m_toVector = Vector4.zero;
        private string m_property = string.Empty;
        private int m_propertyID = -1;
        private UnityEngine.Material m_Material;

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

        public override void Init() {
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

        protected override void Restore() {
            if (null == m_Material) return;
            // end if
            if (!string.IsNullOrEmpty(m_property)) {
                m_Material.SetVector(m_property, m_beginVector);
            } else if (-1 != m_propertyID) {
                m_Material.SetVector(m_propertyID, m_beginVector);
            } // end if
        }

        protected override void JsonTo(JsonData json) {
            if (json.Contains("vector")) m_toVector = Utility.Utils.JsonToVector4(json["vector"]);
            // end if
            if (json.Contains("property")) m_property = (string)json["property"];
            // end if
            if (json.Contains("propertyID")) m_propertyID = (int)json["propertyID"];
            // end if
        }

        protected override void ToJson(ref JsonData json) {
            json["vector"] = Utility.Utils.Vector4Json(m_toVector);
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
            if (string.IsNullOrEmpty(m_property) && m_propertyID == -1) {
                errorInfo = GetType().FullName + " property and propertyID don't assignment";
                return false;
            } // end if
            errorInfo = string.Empty;
            return true;
        }
    }
}
