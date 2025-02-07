using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ST_Replacer : EditorWindow
{
    public static Object replaceModel;
    public static void ShowWindow()
    {
        GetWindow(typeof(ST_Replacer));
        replaceModel = null;
    }

    private void OnGUI()
    {
        int height = 20;
        GUILayout.Label("--- Replacer ---", GUILayout.Height(height), GUILayout.Width(150));

        replaceModel = EditorGUILayout.ObjectField(replaceModel, typeof(object), true);

        if(Selection.gameObjects.Length > 0)
        {
            if (GUILayout.Button("Replace", GUILayout.Height(ST_SuperTools.buttonHeight), GUILayout.Width(150)))
            {
                foreach(GameObject g in Selection.gameObjects)
                {
                    GameObject instance = (GameObject)Instantiate(replaceModel, g.transform);
                    instance.transform.SetParent(null);
                    DestroyImmediate(g);
                }
            }
        }
    }
}
