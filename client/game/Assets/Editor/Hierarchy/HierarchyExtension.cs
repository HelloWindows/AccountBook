using System.Collections;
using System.Collections.Generic;
using JTween;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace UnityEditorExtension {
    public static class HierarchyExtension {
        [MenuItem("GameObject/JTween/Sequence", false, priority = 11)]
        private static void AddJTweenSequenceComponent() {
            Selection.activeGameObject.GetOrAddComponent<JTweenSequence>();
        }

        [MenuItem("GameObject/JTween/Sequence",true, priority = 11)]
        private static bool CheckAddJTweenSequenceComponent() {
            if (!Application.isPlaying) {
                Log.Warning("JTweenSequence must in playing editor!");
                return false;
            } // end if
            if (Selection.activeGameObject == null) {
                Log.Warning("JTweenSequence must select gameObject!");
                return false;
            } // end if
            if (!Selection.activeGameObject.InScene()) {
                Log.Warning("JTweenSequence selected gameObject must in the scene!");
                return false;
            } // end if
            return true;
        }
    }
}
