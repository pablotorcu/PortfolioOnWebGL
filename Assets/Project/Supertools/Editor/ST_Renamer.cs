using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ST_Renamer : EditorWindow
{
    public static string prefix;
    public static string sufix;
    public static bool numbered;
    public static bool canUndo;

    public static void ShowWindow()
    {
        GetWindow(typeof(ST_Renamer));
    }
    private void OnGUI()
    {
        CheckVariables();
        int height = 20;
        GUILayout.Label("", GUILayout.Height(height), GUILayout.Width(150));

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Prefix", GUILayout.Height(height), GUILayout.Width(40));
        prefix = EditorGUILayout.TextField(prefix, GUILayout.Height(height), GUILayout.Width(250));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Sufix", GUILayout.Height(height), GUILayout.Width(40));
        sufix = EditorGUILayout.TextField(sufix, GUILayout.Height(height), GUILayout.Width(250));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Numbered", GUILayout.Height(height), GUILayout.Width(65));
        numbered = EditorGUILayout.Toggle(numbered, GUILayout.Height(height), GUILayout.Width(250));
        EditorGUILayout.EndHorizontal();

        GUILayout.Label("", GUILayout.Height(height), GUILayout.Width(150));

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Rename Objects", GUILayout.Height(ST_SuperTools.buttonHeight), GUILayout.Width(150)))
        {
            if (sufix.Length == 0 && prefix.Length == 0 && !numbered)
            {
                return;
            }
            string targetName = "";
            for (int i = 0; i < Selection.count; i++)
            {
                targetName = prefix + Selection.objects[i].name + sufix;
                if (numbered)
                {
                    targetName += $"_{i}";
                }
                Selection.objects[i].name = targetName;
                EditorUtility.SetDirty(Selection.objects[i]);
                canUndo = true;
            }
        }
        if (canUndo)
        {
            if (GUILayout.Button("Undo Rename", GUILayout.Height(ST_SuperTools.buttonHeight), GUILayout.Width(150)))
            {
                if (sufix.Length == 0 && prefix.Length == 0 && !numbered)
                {
                    return;
                }
                string targetName = "";
                for (int i = 0; i < Selection.count; i++)
                {
                    int sufixAmount = sufix.Length;
                    targetName = Selection.objects[i].name;
                    if (numbered)
                    {
                        sufixAmount += 2;
                        if (i > 9)
                        {
                            sufixAmount++;
                        }
                    }
                    targetName = targetName.Remove(targetName.Length - sufixAmount, sufixAmount);
                    targetName = targetName.Remove(0, prefix.Length);
                    Selection.objects[i].name = targetName;
                    EditorUtility.SetDirty(Selection.objects[i]);
                    canUndo = false;
                }
            }
        }
        EditorGUILayout.EndHorizontal();
    }
    static void CheckVariables()
    {
        if (prefix == null)
        {
            prefix = "";
        }
        if (sufix == null)
        {
            sufix = "";
        }
    }
}
