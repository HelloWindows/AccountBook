using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

class Test : MonoBehaviour {
    // A slider function that takes a SerializedProperty
    void Slider(Rect position, SerializedProperty property, float leftValue, float rightValue, GUIContent label) {
        label = EditorGUI.BeginProperty(position, label, property);

        EditorGUI.BeginChangeCheck();
        var newValue = EditorGUI.Slider(position, label, property.floatValue, leftValue, rightValue);
        // Only assign the value back if it was actually changed by the user.
        // Otherwise a single value will be assigned to all objects when multi-object editing,
        // even when the user didn't touch the control.
        if (EditorGUI.EndChangeCheck()) {
            property.floatValue = newValue;
        }
        EditorGUI.EndProperty();
    }
}