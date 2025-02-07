using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
public class ST_BuildVersion : EditorWindow
{
    static string customBuildVersion;
    public static void ShowWindow()
    {
        customBuildVersion = "";
        GetWindow(typeof(ST_BuildVersion));
    }

    private void OnGUI()
    {
        int height = 20;
        GUILayout.Label("--- Auto Build Version Increment", EditorStyles.boldLabel, GUILayout.Height(height), GUILayout.Width(400));
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Units", GUILayout.Height(height), GUILayout.Width(40));
        ST_DataSaver.SetUnitsIncrement(EditorGUILayout.IntField(ST_DataSaver.GetSTData().unitsIncrement, GUILayout.Height(height), GUILayout.Width(50)));
        GUILayout.Label("Tens", GUILayout.Height(height), GUILayout.Width(40));
        ST_DataSaver.SetTensIncrement(EditorGUILayout.IntField(ST_DataSaver.GetSTData().tensIncrement, GUILayout.Height(height), GUILayout.Width(50)));
        GUILayout.Label("Hundreds", GUILayout.Height(height), GUILayout.Width(60));
        ST_DataSaver.SetHundredsIncrement(EditorGUILayout.IntField(ST_DataSaver.GetSTData().hundredsIncrement, GUILayout.Height(height), GUILayout.Width(50)));
        EditorGUILayout.EndHorizontal();
        GUILayout.Label($"Current V.{Application.version}", GUILayout.Height(height), GUILayout.Width(400));
        GUILayout.Label($"  > Next V.{GetNextBuildVersion()}", GUILayout.Height(height), GUILayout.Width(400));
        if (GUILayout.Button("Increment Build Version", GUILayout.Height(ST_SuperTools.buttonHeight)))
        {
            IncrementBuildVersion();
        }
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Custom Version (X.X.X)", GUILayout.Height(height));
        customBuildVersion = EditorGUILayout.TextField(customBuildVersion, GUILayout.Height(height));
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Set Custom Build Version", GUILayout.Height(ST_SuperTools.buttonHeight)))
        {
            PlayerSettings.bundleVersion = customBuildVersion;
            Debug.Log("Project Version Set to V." + PlayerSettings.bundleVersion);
        }
#if UNITY_ANDROID
        GUILayout.Label("", GUILayout.Height(height));
        GUILayout.Label("Auto Increment Android Bundle Code", GUILayout.Height(height));
        ST_DataSaver.SetAndroidBundleIncrement(EditorGUILayout.Toggle(ST_DataSaver.GetSTData().incrementAndroidBundleVersion));
        if (GUILayout.Button("Increment Android Bundle", GUILayout.Height(ST_SuperTools.buttonHeight)))
        {
            IncrementBundleVersion();
        }
#endif
    }
    public static void IncrementBuildVersion()
    {
        if (ST_DataSaver.GetSTData().unitsIncrement == 0 && ST_DataSaver.GetSTData().tensIncrement == 0 && ST_DataSaver.GetSTData().hundredsIncrement == 0)
        {
            Debug.LogWarning("Can't increment version: You must set any units, tens or hundreds!");
            return;
        }
        PlayerSettings.bundleVersion = GetNextBuildVersion();
        Debug.Log("Project Version Increased to V." + PlayerSettings.bundleVersion);
    }

    public static void IncrementBundleVersion()
    {
        PlayerSettings.Android.bundleVersionCode++;
    }

    public static string GetNextBuildVersion()
    {
        if (PlayerSettings.bundleVersion.Length == 0)
        {
            return "";
        }
        string[] splittedBuild = PlayerSettings.bundleVersion.Split('.');
        List<int> splittedVersion = new List<int>();
        for (int i = 0; i < splittedBuild.Length; i++)
        {
            splittedVersion.Add(int.Parse(splittedBuild[i]));
            switch (i)
            {
                case 0:
                    splittedVersion[i] += ST_DataSaver.GetSTData().unitsIncrement;
                    break;
                case 1:
                    splittedVersion[i] += ST_DataSaver.GetSTData().tensIncrement;
                    break;
                case 2:
                    splittedVersion[i] += ST_DataSaver.GetSTData().hundredsIncrement;
                    break;
            }
        }
        string nextVersion = "";
        for (int i = 0; i < splittedVersion.Count; i++)
        {
            nextVersion += splittedVersion[i].ToString() + ".";
        }
        nextVersion = nextVersion.Remove(nextVersion.Length - 1);
        return nextVersion;
    }
}
class MyCustomBuildProcessor : IPreprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }
    public void OnPreprocessBuild(BuildReport report)
    {
        ST_BuildVersion.IncrementBuildVersion();
        if (ST_DataSaver.GetSTData().incrementAndroidBundleVersion)
        {
            ST_BuildVersion.IncrementBundleVersion();
        }
    }
}