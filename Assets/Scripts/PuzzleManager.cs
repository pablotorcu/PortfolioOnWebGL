using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private Material _selectedMat, _defMat;
    [SerializeField] private bool[] _currentPuzzle;
    [SerializeField] private PuzzlePieceInstance[] _pieces;
    [SerializeField] private MainManager _mainManager;
    [SerializeField] private AudioSource[] _sounds;

    public void OnTouchPiece(int pieceIndex)
    {
        _currentPuzzle[pieceIndex] = !_currentPuzzle[pieceIndex];
        if (_currentPuzzle[pieceIndex])
        {
            _sounds[0].Play();
            _pieces[pieceIndex].SetMaterial(_selectedMat);
        }
        else
        {
            _sounds[1].Play();
            _pieces[pieceIndex].SetMaterial(_defMat);
        }
        if (_mainManager.CheckPad(_currentPuzzle))
        {
            _sounds[2].Play();
        }
    }

    public void ResetPuzzle()
    {
        for (int i = 0; i < _currentPuzzle.Length; i++)
        {
            if (_currentPuzzle[i])
            {
                OnTouchPiece(i);
            }
        }
    }
}
