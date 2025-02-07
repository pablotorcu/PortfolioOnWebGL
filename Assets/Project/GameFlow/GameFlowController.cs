using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameFlowController : MonoBehaviour
{
    [SerializeField]
    TransitionsController _transitionsController;
    float _sceneLoadAmount;


    public void Awake()
    {
        GameFlowEvents.LoadScene.AddListener(LoadScene);
        GameFlowEvents.LoadSceneAdditive.AddListener(LoadSceneAdditive);
        GameFlowEvents.LoadSceneInstantly.AddListener(LoadSceneInstantly);
        GameFlowEvents.LoadSceneWaiting.AddListener(LoadScene);
    }

    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(CoLoadSceneAsync(sceneName));
    }
    public IEnumerator CoLoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        while (asyncLoad.progress < 0.9f)
        {
            _sceneLoadAmount = asyncLoad.progress;
            yield return null;
        }
        yield return StartCoroutine(_transitionsController.coFadeToBlack());

        asyncLoad.allowSceneActivation = true;
    }
    public float GetLevelLoadAmount()
    {
        return _sceneLoadAmount;
    }
    public void LoadScene(string sceneName)
    {
        StartCoroutine(CrLoadScene(sceneName));
    }
    public void LoadScene(string sceneName, float waitTime)
    {
        StartCoroutine(CrLoadScene(sceneName, waitTime));
    }

    public void LoadSceneInstantly(string sceneName)
    {
        PlayerPrefs.SetInt("NoTransition", 1);
        LastLoadScene(sceneName);
    }
    IEnumerator CrLoadScene(string sn)
    {
        yield return StartCoroutine(_transitionsController.coFadeToBlack());
        yield return null;
        LastLoadScene(sn);
    }
    IEnumerator CrLoadScene(string sn, float waitTime)
    {
        yield return StartCoroutine(_transitionsController.coFadeToBlack(waitTime));
        yield return null;
        LastLoadScene(sn);
    }
    public void LoadSceneAdditive(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public void LastLoadScene(string sceneName)
    {
        //GameEvents.SaveOnDB.Invoke();
        SceneManager.LoadScene(sceneName);
    }
}
