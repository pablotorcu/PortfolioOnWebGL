using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    [SerializeField] private Transform _cameraTr;
    [SerializeField] private Transform[] _orientations;
    [SerializeField] private TextMeshProUGUI _startText, _currentModeText, _clickText, _instructionText;
    [SerializeField] private GameObject _startCanvas, _sandParticles, _bigSand, _mainCanvas;
    [SerializeField] private AnimationCurve _aCurve;
    [SerializeField] private ParticleSystem[] _sandParts;
    [SerializeField] private Animator _touchStoneAnim, _puzzleAnim, _ruinsAnim, _dragInstruction, _mainCanvasAnim;
    [SerializeField] private PadData[] _padPostions;
    private PadData _currentPadData;
    private bool _puzzleAvailable;
    private Coroutine _changeTextCR, _clickTextCR;

    void Start()
    {
        float aspectRatio = (float)Screen.width / Screen.height;
        if (aspectRatio < 1)
        {
            foreach (ParticleSystem part in _sandParts) 
            {
                var r = part.shape.radius;
                r = r * aspectRatio;
                _cameraTr.GetComponent<Camera>().fieldOfView = 85;
            }
            _startText.fontSize *= 2;
            _instructionText.fontSize *= 3;
            _currentModeText.fontSize *= 2;
            _instructionText.rectTransform.localPosition = Vector3.up * 850;
        }
        _currentModeText.text = "";
        _clickText.text = "";
        _startText.text = "Start";
        _clickText.fontMaterial.SetFloat("_FaceDilate",-1);
        _currentModeText.fontMaterial.SetFloat("_FaceDilate",-1);
        _cameraTr.GetComponent<CameraOrbit>().enabled = false;
        _cameraTr.position = _orientations[0].position;
        _cameraTr.rotation = _orientations[0].rotation;
    }

    public void OnStart()
    {
        StartCoroutine(CrAdaptCamera());
        IEnumerator CrAdaptCamera()
        {
            _sandParticles.SetActive(true);
            StartCoroutine(CrChangeText(_startText,1f, -1f));
            yield return StartCoroutine(CrOrientate(2f, _orientations[1]));
            _sandParticles.SetActive(false);

            _startText.text = "Im\nPablo\nTorcuato";
            yield return StartCoroutine(CrChangeText(_startText, 1f,0.35f));
            yield return new WaitForSeconds(1f);
            _sandParticles.SetActive(true);
            yield return StartCoroutine(CrChangeText(_startText, 1f, -1f));

            _startText.text = "use the\npuzzle to\nview my\nportfolio";
            yield return StartCoroutine(CrChangeText(_startText, 1f, 0.35f));
            yield return new WaitForSeconds(1.5f);
            _bigSand.SetActive(true);
            yield return StartCoroutine(CrChangeText(_startText, 1f, -1f));
            yield return StartCoroutine(CrOrientate(1f, _orientations[2]));
            _puzzleAnim.SetBool("On", true);
            _ruinsAnim.SetBool("On", true);
            _puzzleAvailable = true;
            yield return new WaitForSeconds(1.5f);
            _startText.gameObject.SetActive(false);
            _mainCanvas.SetActive(true);
        }
    }


    public IEnumerator CrChangeText(TextMeshProUGUI text, float duration, float targetDil)
    {
        float startDil = text.fontMaterial.GetFloat("_FaceDilate");
        if (startDil == -1)
        {
            text.gameObject.SetActive(true);
        }
        for (float i = 0; i < duration; i += Time.deltaTime)
        {
            text.fontMaterial.SetFloat("_FaceDilate", Mathf.Lerp(startDil, targetDil, i / duration));
            yield return null;
        }
        text.fontMaterial.SetFloat("_FaceDilate", targetDil);
        if (targetDil == -1 && text != _startText)
        {
            text.gameObject.SetActive(false);
        }
    }

    public IEnumerator CrOrientate(float duration, Transform targetOrientation)
    {
        Quaternion startRot = _cameraTr.rotation;
        Vector3 startPos = _cameraTr.position;

        for (float i = 0; i < duration; i += Time.deltaTime)
        {
            _cameraTr.rotation = Quaternion.Lerp(startRot, targetOrientation.rotation, _aCurve.Evaluate(i / duration));
            _cameraTr.position = Vector3.Lerp(startPos, targetOrientation.position, _aCurve.Evaluate(i / duration));
            yield return null;
        }
        _cameraTr.rotation = targetOrientation.rotation;
        _cameraTr.position = targetOrientation.position;
    }

    public void CheckPad(bool[] padConfig)
    {
        bool same = false;
        foreach (PadData p in _padPostions)
        {
            if (CheckIfEqual(p.padConfig, padConfig)) 
            {
                _currentModeText.text = p.padName;
                _currentPadData = p;
                same = true;
            }
        }
        if (_changeTextCR != null)
        {
            StopCoroutine(_changeTextCR);
            StopCoroutine(_clickTextCR);
        }
        float targetDil = -1f;
        if (same)
        {
            _clickText.text = "Click";
            targetDil = 0.35f;
        }
        if (_currentModeText.fontMaterial.GetFloat("_FaceDilate") != targetDil)
        {
            _changeTextCR = StartCoroutine(CrChangeText(_currentModeText, 1, targetDil));
            _clickTextCR = StartCoroutine(CrChangeText(_clickText, 1, targetDil));
        }
        _touchStoneAnim.SetBool("On", same);
    }

    public void HelpButton()
    {
        _mainCanvasAnim.SetBool("Show", !_mainCanvasAnim.GetBool("Show"));
    }

    public void LaunchCity()
    {
        if (!_puzzleAvailable)
        {
            return;
        }
        StartCoroutine(CrLaunchCity()); 
        IEnumerator CrLaunchCity()
        {
            HidePuzzle();
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(CrOrientate(1f, _orientations[3]));
            _currentPadData.targetCity.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            yield return StartCoroutine(CrOrientate(1f, _orientations[4]));
            _cameraTr.GetComponent<CameraOrbit>().enabled = true;
            _dragInstruction.SetBool("On", true);
        }
    }

    void HidePuzzle()
    {
        _mainCanvas.SetActive(false);
        _puzzleAvailable = false;
        _touchStoneAnim.SetBool("On", false);
        _changeTextCR = StartCoroutine(CrChangeText(_currentModeText, 0.5f, -1));
        _clickTextCR = StartCoroutine(CrChangeText(_clickText, 0.5f, -1));
        _puzzleAnim.SetBool("On", false);
        _ruinsAnim.SetBool("On", false);
    }


    public void BackToPuzzle()
    {
        _mainCanvas.SetActive(true);
        _puzzleAvailable = true;
        StartCoroutine(CrOrientate(1f, _orientations[1]));
        _cameraTr.GetComponent<CameraOrbit>().enabled = false;
    }

    public void SetStone(bool state)
    {
        _touchStoneAnim.SetBool("On", state);
    }

    public bool CheckIfEqual(bool[] a1, bool[] a2)
    {
        for (int i = 0; i < a1.Length; i++)
        {
            if (a1[i] != a2[i])
            {
                return false;
            }
        }
        return true;
    }
}

[System.Serializable]
public class PadData
{
    public string padName;
    public bool[] padConfig;
    public GameObject targetCity;
}
