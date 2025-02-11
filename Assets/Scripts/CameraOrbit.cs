using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target; // Punto objetivo al que se acerca la cámara
    public float zoomSpeed = 5f; // Velocidad de zoom
    public float minZoom = 5f; // Zoom mínimo
    public float maxZoom = 20f; // Zoom máximo
    public float positionSmoothTime = 0.5f; // Tiempo de suavizado para la posición

    private Camera cam;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        HandleMouseZoom();
        HandleTouchZoom();
        SmoothAdjustPosition();
    }

    void HandleMouseZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            ZoomCamera(scroll * zoomSpeed);
        }
    }

    void HandleTouchZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            float prevDistance = (touch0.position - touch0.deltaPosition - (touch1.position - touch1.deltaPosition)).magnitude;
            float currentDistance = (touch0.position - touch1.position).magnitude;
            float deltaDistance = currentDistance - prevDistance;

            ZoomCamera(deltaDistance * 0.01f * zoomSpeed);
        }
    }

    void ZoomCamera(float increment)
    {
        float targetSize = cam.orthographic ? cam.orthographicSize - increment : Vector3.Distance(transform.position, target.position) - increment;
        targetSize = Mathf.Clamp(targetSize, minZoom, maxZoom);

        if (cam.orthographic)
        {
            cam.orthographicSize = targetSize;
        }
        else
        {
            Vector3 targetPosition = target.position - (transform.forward * targetSize);
            transform.position = targetPosition;
        }
    }

    void SmoothAdjustPosition()
    {
        Vector3 targetPosition = target.position - transform.forward * Mathf.Clamp(Vector3.Distance(transform.position, target.position), minZoom, maxZoom);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, positionSmoothTime);
    }
}

