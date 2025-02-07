using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ST_Customer : EditorWindow
{
    public static Color targetColor;
    private Color color;
    public static void ShowWindow()
    {
        GetWindow(typeof(ST_Customer));
    }

    private void OnGUI()
    {
        int height = 20;
        GUILayout.Label("", GUILayout.Height(height), GUILayout.Width(150));

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Color Changer", GUILayout.Height(height), GUILayout.Width(100));
        //targetColor = GUILayout.
        EditorGUILayout.EndHorizontal();
        color = EditorGUILayout.ColorField("Target Color",color);
        if (GUILayout.Button("Change Color", GUILayout.Height(ST_SuperTools.buttonHeight)))
        {

        }
    }
}