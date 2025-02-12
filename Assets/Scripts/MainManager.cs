using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    [SerializeField] private Transform _startCameraTr, _cameraTr;
    [SerializeField] private Transform _defOrientation, _puzzleOrientation;
    [SerializeField] private TextMeshProUGUI _startText;
    [SerializeField] private GameObject _startCanvas, _sandParticles, _bigSand;
    [SerializeField] private AnimationCurve _aCurve;
    [SerializeField] private ParticleSystem[] _sandParts;

    void Start()
    {
        if (!WebGLInput.captureAllKeyboardInput)
        {
            foreach(ParticleSystem p in _sandParts)
            {
                var shape = p.shape;
                shape.radius = p.shape.radius / 2f;
            }
        }
        _startText.text = "Start";
        _cameraTr.GetComponent<CameraOrbit>().enabled = false;
        _cameraTr.position = _startCameraTr.position;
        _cameraTr.rotation = _startCameraTr.rotation;
    }

    public void OnStart()
    {
        StartCoroutine(CrAdaptCamera());
        IEnumerator CrAdaptCamera()
        {
            _sandParticles.SetActive(true);
            StartCoroutine(CrChangeText(1f, -1f));
            yield return StartCoroutine(CrOrientate(2f,_defOrientation));
            _sandParticles.SetActive(false);

            _startText.text = "Im\nPablo\nTorcuato";
            yield return StartCoroutine(CrChangeText(1f,0.35f));
            yield return new WaitForSeconds(1f);
            _sandParticles.SetActive(true);
            yield return StartCoroutine(CrChangeText(1f, -1f));

            _startText.text = "use the\npuzzle to\nview my\nportfolio";
            yield return StartCoroutine(CrChangeText(1f, 0.35f));
            yield return new WaitForSeconds(1.5f);
            _bigSand.SetActive(true);
            yield return StartCoroutine(CrChangeText(1f, -1f));
            //_startText.gameObject.SetActive(false);

            yield return StartCoroutine(CrOrientate(1f, _puzzleOrientation));
        }
    }


    public IEnumerator CrChangeText(float duration, float targetDil)
    {
        float startDil = _startText.fontMaterial.GetFloat("_FaceDilate");
        for (float i = 0; i < duration; i += Time.deltaTime)
        {
            _startText.fontMaterial.SetFloat("_FaceDilate", Mathf.Lerp(startDil, targetDil, i / duration));
            yield return null;
        }
        _startText.fontMaterial.SetFloat("_FaceDilate", targetDil);
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
}
