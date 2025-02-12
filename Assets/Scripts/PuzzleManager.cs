using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private Material _selectedMat, _defMat;
    [SerializeField] private bool[] _currentPuzzle;
    [SerializeField] private PuzzlePieceInstance[] _pieces;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTouchPiece(int pieceIndex)
    {
        _currentPuzzle[pieceIndex] = !_currentPuzzle[pieceIndex];
        if (_currentPuzzle[pieceIndex])
        {
            _pieces[pieceIndex].SetMaterial(_selectedMat);
        }
        else
        {
            _pieces[pieceIndex].SetMaterial(_defMat);
        }
    }
}
