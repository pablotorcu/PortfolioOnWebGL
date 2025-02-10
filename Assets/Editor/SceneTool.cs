using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
public class SceneTool : MonoBehaviour{
[MenuItem("Open Scene/Main")]
public static void LoadScene0()
{
 LoadScene("Assets/Scenes/Main.unity");
}
public static void LoadScene(string scenePath)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(scenePath);
        }
    }
}