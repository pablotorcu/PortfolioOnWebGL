using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSoundManager : MonoBehaviour
{
    [SerializeField] AudioSource[] _aSources;
    private int _currentIndex;

    void Start()
    {
        _aSources[_currentIndex].Play();
        StartCoroutine(CrWaitForNext());
    }

    IEnumerator CrWaitForNext()
    {
        yield return new WaitForSeconds(_aSources[0].clip.length - 1.5f);
        _currentIndex = (_currentIndex + 1) % _aSources.Length;
        _aSources[_currentIndex].pitch = Random.Range(0.95f, 1.05f);
        _aSources[_currentIndex].Play();
        StartCoroutine(CrWaitForNext());
    }
}
