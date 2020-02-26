using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestWindow : EditorWindow {
    // Add menu item
    [MenuItem("Example/Popup Example")]
    static void Init() {
        EditorWindow window = CreateInstance<TestWindow>();
        window.Show();
    }

    Rect buttonRect;
    void OnGUI() {
        {
            GUILayout.Label("Editor window with Popup example", EditorStyles.boldLabel);
            if (GUILayout.Button("Popup Options", GUILayout.Width(200))) {
                PopupWindow.Show(buttonRect, new PopupExample());
            }
            if (Event.current.type == EventType.Repaint) buttonRect = GUILayoutUtility.GetLastRect();
        }
    }
}

public class PopupExample : PopupWindowContent {
    bool toggle1 = true;
    bool toggle2 = true;
    bool toggle3 = true;

    public override Vector2 GetWindowSize() {
        return new Vector2(200, 150);
    }
    Vector2 scrollPos;
    int selGridInt = -1;
    string[] selStrings = { "radio1", "radio2", "radio3" };

    public override void OnGUI(Rect rect) {
        EditorGUILayout.BeginVertical();
        GUILayout.Label("Popup Options Example", EditorStyles.boldLabel);
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        selGridInt = GUILayout.SelectionGrid(selGridInt, selStrings, 1);
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    public override void OnOpen() {
        Debug.Log("Popup opened: " + this);
    }

    public override void OnClose() {
        Debug.Log("Popup closed: " + this);
    }
}
