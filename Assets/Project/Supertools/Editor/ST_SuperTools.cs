using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;
using UnityEditor.SceneManagement;

public class ST_SuperTools : EditorWindow
{
    public static int miniHeight = 10, height = 20, buttonHeight = 30;
    [MenuItem("SuperTools/Open")]
    public static void ShowWindow()
    {
        ST_DataSaver.CheckInitialized();
        GetWindow(typeof(ST_SuperTools));
    }
    private void OnGUI()
    {
        GUILayout.Label("SUPER TOOLS", EditorStyles.largeLabel, GUILayout.Height(height), GUILayout.Width(150));
     
        //Scene Location
        GUILayout.Label("", GUILayout.Height(miniHeight/2), GUILayout.Width(150));
        GUILayout.Label("--- Scene Location",EditorStyles.boldLabel, GUILayout.Height(height), GUILayout.Width(150));
        if (GUILayout.Button("Open Scene Location", GUILayout.Height(buttonHeight), GUILayout.Width(250)))
        {
            ST_SceneLocation.ShowWindow();
        }

        //Build Version
        GUILayout.Label("", GUILayout.Height(miniHeight), GUILayout.Width(150));
        GUILayout.Label("--- Build Version Settings", EditorStyles.boldLabel, GUILayout.Height(height), GUILayout.Width(400));
        if (GUILayout.Button("Open Build Version Settings", GUILayout.Height(buttonHeight), GUILayout.Width(250)))
        {
            ST_BuildVersion.ShowWindow();
        }

        //Renamer
        GUILayout.Label("", GUILayout.Height(miniHeight), GUILayout.Width(150));
        GUILayout.Label("--- Renamer", EditorStyles.boldLabel, GUILayout.Height(height), GUILayout.Width(150));
        if (GUILayout.Button("Open Renamer", GUILayout.Height(buttonHeight), GUILayout.Width(250)))
        {
            ST_Renamer.ShowWindow();
        }

        //Customer
        GUILayout.Label("", GUILayout.Height(miniHeight), GUILayout.Width(150));
        GUILayout.Label("--- Customer", EditorStyles.boldLabel, GUILayout.Height(height), GUILayout.Width(150));
        if (GUILayout.Button("Open Customer", GUILayout.Height(buttonHeight), GUILayout.Width(250)))
        {
            ST_Customer.ShowWindow();
        }

        //Replacer
        GUILayout.Label("", GUILayout.Height(miniHeight), GUILayout.Width(150));
        GUILayout.Label("--- Replacer", EditorStyles.boldLabel, GUILayout.Height(height), GUILayout.Width(150));
        if (GUILayout.Button("Open Replacer", GUILayout.Height(buttonHeight), GUILayout.Width(250)))
        {
            ST_Replacer.ShowWindow();
        }
    }
    public static void SaveScene()
    {
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
    }
}