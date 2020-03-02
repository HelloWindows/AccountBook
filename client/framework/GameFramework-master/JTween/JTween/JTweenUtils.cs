using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using LitJson;

namespace JTween
{
    public static class JTweenUtils {
        public static void Identity(this UnityEngine.Transform trans)
        {
            trans.localScale = Vector3.one;
            trans.localPosition = Vector3.zero;
            trans.localRotation = Quaternion.identity;
        }

        public static void Clear(this StringBuilder sb)
        {
            sb.Length = 0;
        }

        public static void AppendLineEx(this StringBuilder sb, string str = "")
        {
            sb.Append(str + "\r\n"); 
        }

        static public long Ticks
        {
            get
            {
                return System.DateTime.Now.Ticks;
            }
        }
        public static string FormatSeconds(int time, string format)
        {
            string ret = format;
            int dd = time / (24 * 3600);
            int hh = time / 3600 % 24;
            int mm = time / 60 % 60;
            int ss = time % 60;

            if (format.Contains("dd"))
            {
                ret = ret.Replace("dd", dd >= 10 ? dd.ToString() : ("0" + dd));
            }
            else
            {
                hh = hh + dd * 24;
            }
            if (format.Contains("hh"))
            {
                ret = ret.Replace("hh", hh >= 10 ? hh.ToString() : ("0" + hh));
            }
            else
            {
                mm = mm + hh * 60;
            }
            if (format.Contains("mm"))
            {
                ret = ret.Replace("mm", mm >= 10 ? mm.ToString() : ("0" + mm));
            }
            else
            {
                ss = ss + mm * 60;
            }
            if (format.Contains("ss"))
            {
                ret = ret.Replace("ss", ss >= 10 ? ss.ToString() : ("0" + ss));
            }
            return ret;
        }
        public static void SetLayer(UnityEngine.Transform xform, int layer)
        {
            xform.gameObject.layer = layer;
            foreach (UnityEngine.Transform child in xform)
                SetLayer(child, layer);
        }

        public static void ChangeParent(UnityEngine.Transform objTran, UnityEngine.Transform parent)
        {
            if (parent != null)
            {
                objTran.SetParent(parent, false);
                SetLayer(objTran, parent.gameObject.layer);
            }
            else
            {
                objTran.SetParent(null, false);
            }
        }

        public static string GetTranPath(UnityEngine.Transform obj)
        {
            string ret = obj.name;
            UnityEngine.Transform parent = obj.parent;
            while(parent != null)
            {
                ret = parent.name + "/" + ret;
                parent = parent.parent;
            }
            return ret;
        }

        public static T GetComponentInParent<T>(UnityEngine.Transform obj) where T : Component
        {
            UnityEngine.Transform parent = obj.parent;
            T ret = null;
            while (parent != null)
            {
                ret = parent.GetComponent<T>();
                if (ret != null)
                    return ret;
                parent = parent.parent;
            }
            return ret;
        }

        #region Compare
        public static bool IsEqual(this Vector2 a, Vector2 b, float diffVar = 0.001f)
        {
            return Mathf.Abs(a.x - b.x) < diffVar && Mathf.Abs(a.y - b.y) < diffVar;
        }
        public static bool IsEqual(this Vector3 a, Vector3 b, float diffVar = 0.001f)
        {
            return Mathf.Abs(a.x - b.x) < diffVar && Mathf.Abs(a.y - b.y) < diffVar && Mathf.Abs(a.z - b.z) < diffVar;
        }
        public static bool IsEqual(this Vector4 a, Vector4 b, float diffVar = 0.001f)
        {
            return Mathf.Abs(a.x - b.x) < diffVar && Mathf.Abs(a.y - b.y) < diffVar && Mathf.Abs(a.z - b.z) < diffVar && Mathf.Abs(a.w - b.w) < diffVar;
        }
        public static bool IsEqual(this Quaternion a, Quaternion b, float diffVar = 0.001f)
        {
            return Mathf.Abs(a.x - b.x) < diffVar && Mathf.Abs(a.y - b.y) < diffVar && Mathf.Abs(a.z - b.z) < diffVar && Mathf.Abs(a.w - b.w) < diffVar;
        }
        public static bool IsEqual(this Color a, Color b, float diffVar = 0.001f)
        {
            return Mathf.Abs(a.r - b.r) < diffVar && Mathf.Abs(a.g - b.g) < diffVar && Mathf.Abs(a.b - b.b) < diffVar && Mathf.Abs(a.a - b.a) < diffVar;
        }
        public static bool IsEqual(float a, float b, float diffVar = 0.001f)
        {
            return Mathf.Abs(a - b) < diffVar;
        }
        #endregion

        #region JsonTools
        public static JsonData Vector2Json(Vector2 arg)
        {
            JsonData ret = new JsonData();
            ret["x"] = System.Math.Round(arg.x, 4);
            ret["y"] = System.Math.Round(arg.y, 4);
            return ret;
        }

        public static JsonData Vector3Json(Vector3 arg)
        {
            JsonData ret = new JsonData();
            ret["x"] = System.Math.Round(arg.x, 4);
            ret["y"] = System.Math.Round(arg.y, 4);
            ret["z"] = System.Math.Round(arg.z, 4);
            return ret;
        }

        public static JsonData Vector4Json(Vector4 arg)
        {
            JsonData ret = new JsonData();
            ret["x"] = System.Math.Round(arg.x, 4);
            ret["y"] = System.Math.Round(arg.y, 4);
            ret["z"] = System.Math.Round(arg.z, 4);
            ret["w"] = System.Math.Round(arg.w, 4);
            return ret;
        }

        public static JsonData ColorJson(Color arg)
        {
            JsonData ret = new JsonData();
            ret["r"] = System.Math.Round(arg.r, 4);
            ret["g"] = System.Math.Round(arg.g, 4);
            ret["b"] = System.Math.Round(arg.b, 4);
            ret["a"] = System.Math.Round(arg.a, 4);
            return ret;
        }

        public static JsonData AnimationCurveJson(AnimationCurve arg)
        {
            JsonData ret = new JsonData();
            JsonData jsonKeys = new JsonData();
            Keyframe[] keys = arg.keys;
            for (int i = 0, imax = keys.Length; i < imax; ++i)
            {
                Keyframe k = keys[i];
                JsonData oneKey = new JsonData();
                oneKey["T"] = System.Math.Round(k.time, 4);
                oneKey["V"] = System.Math.Round(k.value, 4);
                oneKey["I"] = System.Math.Round(k.inTangent, 4);
                oneKey["O"] = System.Math.Round(k.outTangent, 4);
                oneKey["M"] = k.tangentMode;
                jsonKeys.Add(oneKey);
            }
            ret["keys"] = jsonKeys;
            ret["pre"] = (int)arg.preWrapMode;
            ret["post"] = (int)arg.postWrapMode;
            return ret;
        }

        public static Vector2 JsonToVector2(JsonData json)
        {
            return new Vector2(json["x"].ToFloat(), json["y"].ToFloat());
        }

        public static Vector3 JsonToVector3(JsonData json)
        {
            return new Vector3(json["x"].ToFloat(), json["y"].ToFloat(), json["z"].ToFloat());
        }

        public static Vector4 JsonToVector4(JsonData json)
        {
            return new Vector4(json["x"].ToFloat(), json["y"].ToFloat(), json["z"].ToFloat(), json["w"].ToFloat());
        }

        public static Color JsonToColor(JsonData json)
        {
            return new Color(json["r"].ToFloat(), json["g"].ToFloat(), json["b"].ToFloat(), json["a"].ToFloat());
        }

        public static AnimationCurve JsonAnimationCurve(JsonData json)
        {
            JsonData jsonKeys = json["keys"];
            int count = jsonKeys.Count;
            Keyframe[] keys = new Keyframe[count];
            for (int i = 0; i < count; ++i)
            {
                JsonData oneKey = jsonKeys[i];
                keys[i] = new Keyframe(oneKey["T"].ToFloat(), oneKey["V"].ToFloat(), oneKey["I"].ToFloat(), oneKey["O"].ToFloat());
                keys[i].tangentMode = oneKey["M"].ToInt32();
            }
            AnimationCurve ret = new AnimationCurve(keys);
            ret.preWrapMode = (WrapMode)json["pre"].ToInt32();
            ret.postWrapMode = (WrapMode)json["post"].ToInt32();
            return ret;
        }

        public static int UnicodeStringLength(string str)
        {
            UnicodeEncoding encode = new UnicodeEncoding();
            return encode.GetByteCount(str);
        }
        #endregion

        public static string GetStringFont(string str, int num)
        {
            int length = str.Length;
            if (num >= length)
                return str;
            else
                return str.Substring(0, num);
        }
    }
}

