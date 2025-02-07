using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class ST_SceneLocation : EditorWindow
{
    static List<string> sceneNames;
    static List<string> foldersDirectories;
    static List<string> locatedPaths;

    public static void ShowWindow()
    {
        GetWindow(typeof(ST_SceneLocation));
    }

    public void OnGUI()
    {
        int height = 20;
        GUILayout.Label("", EditorStyles.boldLabel, GUILayout.Height(height), GUILayout.Width(150));
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Refresh Scenes", GUILayout.Height(ST_SuperTools.buttonHeight)))
        {
            LocateScenes();
        }
        if (SceneToolExist())
        {
            if (GUILayout.Button("Hide Open Scene Menu Item", GUILayout.Height(ST_SuperTools.buttonHeight)))
            {
                DeleteScript();
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    public static void LocateScenes()
    {
        CheckCreationFolders();
        sceneNames = new List<string>();
        foldersDirectories = new List<string>();
        foldersDirectories.Add("Assets/Scenes");
        LoadSubFoldersPath("Assets/Scenes");
        locatedPaths = new List<string>();
        GetScenesFromPaths();
        File.WriteAllText(Application.dataPath+"/Editor/SceneTool.cs", GetCodeContent(locatedPaths));
        AssetDatabase.Refresh();
    }

    public static bool SceneToolExist()
    {
        bool exist = false;
        if (File.Exists(Application.dataPath + "/Editor/SceneTool.cs"))
        {
            exist = true;
        }
        return exist;
    }

    public static void DeleteScript()
    {
        if (File.Exists(Application.dataPath + "/Editor/SceneTool.cs"))
        {
            File.Delete(Application.dataPath + "/Editor/SceneTool.cs");
            AssetDatabase.Refresh();
        }
    }

    public static string GetProcessedDataPath()
    {
        string dataPath = Application.dataPath;
        for (int i = 0; i < 6; i++) // delete Assets from path
        {
            dataPath = dataPath.Remove(dataPath.Length - 1);
        }
        return dataPath;
    }

    public static string ProcessPath(string rawPath)
    {
        string[] pathSceneNames = rawPath.Split('\\');
        string nameExtract = string.Empty;
        int diff = pathSceneNames.Length - 9;
        if (diff > 0) //Check if path is into any folder to create directory on tool
        {
            for (int i = 0; i <= diff; i++)
            {
                nameExtract += "/" +pathSceneNames[pathSceneNames.Length - diff + i - 1];
            }
        }
        else
        {
            nameExtract = pathSceneNames[pathSceneNames.Length - 1];
        }
        for (int i = 0; i < 6; i++) // delete .unity extension from name
        {
            nameExtract = nameExtract.Remove(nameExtract.Length - 1);
        }
        sceneNames.Add(nameExtract);
        string target = "Assets";
        string processedPath = "";
        bool finded = false;
        for (int i = 0; i < rawPath.Length; i++)
        {
            if (rawPath[i] == target[0])
            {
                for (int j = 0; j < target.Length; j++)
                {
                    if (rawPath[i+j] == target[j])
                    {
                        if (j == target.Length-1)
                        {
                            finded = true;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if (finded)
            {
                if (rawPath[i] == '\\')
                {
                    processedPath += "/";
                }
                else
                {
                    processedPath += rawPath[i];
                }
            }
        }
        return processedPath;
    }

    public static string GetCodeContent(List<string> scenePaths)
    {
        string codeContent = "using UnityEngine;\nusing UnityEditor;\nusing UnityEditor.SceneManagement;\n";
        codeContent += "public class SceneTool : MonoBehaviour{\n";
        for (int i = 0; i < scenePaths.Count; i++)
        {
            codeContent += $"[MenuItem(\"Open Scene/{sceneNames[i]}\")]\n";
            codeContent += $"public static void LoadScene{i}()\n";
            codeContent += "{\n LoadScene(\"" + scenePaths[i]+ "\");\n}";
        }
    //End of class
    codeContent += "\n" + @"public static void LoadScene(string scenePath)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(scenePath);
        }
    }
}";//End of class

        return codeContent;
    }
    public static void CheckCreationFolders()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Editor"))
        {
            AssetDatabase.CreateFolder("Assets", "Editor");
        }
        if (File.Exists(Application.dataPath + "/Editor/SceneTool.cs"))
        {
            File.Delete(Application.dataPath + "/Editor/SceneTool.cs");
        }
    }

    public static void LoadSubFoldersPath(string targetPath)
    {
        string[] subfolders = AssetDatabase.GetSubFolders(targetPath);
        for (int i = 0; i < subfolders.Length; i++)
        {
            foldersDirectories.Add(subfolders[i]);
            LoadSubFoldersPath(subfolders[i]);
        }
    }

    public static void GetScenesFromPaths()
    {
        for (int i = 0; i < foldersDirectories.Count; i++)
        {
            DirectoryInfo d = new DirectoryInfo(GetProcessedDataPath() + foldersDirectories[i]);
            foreach (var file in d.GetFiles("*.unity"))
            {
                locatedPaths.Add(ProcessPath(file.ToString()));
            }
        }
    }
}