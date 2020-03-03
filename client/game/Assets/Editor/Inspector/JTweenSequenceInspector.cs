using System.Collections;
using System.Collections.Generic;
using System.IO;
using JTween;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Editor;

namespace JTween.Editor {
    [CustomEditor(typeof(JTweenSequence), true)]
    [CanEditMultipleObjects]
    public class JTweenSequenceInspector : GameFrameworkInspector {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            serializedObject.Update();
            GUILayout.BeginVertical("box"); {
                EditorGUILayout.LabelField("Load Json", EditorStyles.boldLabel);
                //DirectoryInfo info = new DirectoryInfo(EditorTool.TWEENER_PATH);
                //if (info != null) {
                //    FileInfo[] files = info.GetFiles("*.json");
                //    if (files != null && files.Length > 0) {
                //        tweenerFiles.Add("");
                //        int lastIndex = 0;
                //        int i = 1;
                //        foreach (FileInfo f in files) {
                //            string fileName = Path.GetFileNameWithoutExtension(f.Name);
                //            tweenerFiles.Add(fileName);
                //            if (t != null && !string.IsNullOrEmpty(t.Name)) {
                //                if (lastIndex == 0 && fileName == t.Name)
                //                    lastIndex = i;
                //            }
                //            ++i;
                //        }
                //    }
                //}
            } GUILayout.EndVertical();
        }
    }
}
