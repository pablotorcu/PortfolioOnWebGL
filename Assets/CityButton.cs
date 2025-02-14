using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityButton : MonoBehaviour
{
    [SerializeField] private MainManager _mainManager;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    private void OnMouseDown()
    {
        if (_animator.GetBool("On"))
        {
            _mainManager.LaunchCity();
        }
    }
}
