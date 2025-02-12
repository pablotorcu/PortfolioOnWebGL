using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePieceInstance : MonoBehaviour
{
    public int pieceIndex;
    public PuzzleManager puzzleManager;
    private MeshRenderer _mRenderer;

    private void Start()
    {
        _mRenderer = GetComponent<MeshRenderer>();
    }
    private void OnMouseDown()
    {
        puzzleManager.OnTouchPiece(pieceIndex);
    }

    public void SetMaterial(Material m)
    {
        _mRenderer.material = m;
    }
}
