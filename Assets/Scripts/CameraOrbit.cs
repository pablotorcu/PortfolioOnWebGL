using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target; // Objeto al que la cámara siempre estará mirando
    public float distance = 5f; // Distancia de la cámara desde el objeto
    public float xSpeed = 250f; // Velocidad de rotación en el eje X
    public float ySpeed = 120f; // Velocidad de rotación en el eje Y
    public float zoomSpeed = 3f; // Velocidad de rotación en el eje Y
    public float yMinLimit = -20f; // Límite inferior de la rotación vertical
    public float yMaxLimit = 80f; // Límite superior de la rotación vertical
    public float distanceMin = 2f; // Distancia mínima de la cámara
    public float distanceMax = 10f; // Distancia máxima de la cámara

    private float x = 0f; // Rotación en el eje X (horizontal)
    private float y = 0f; // Rotación en el eje Y (vertical)

    private void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        // Control del ratón
        if (Input.GetMouseButton(0))
        {
            // Movimiento del ratón
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
        }

        // Límite de la rotación vertical
        y = Mathf.Clamp(y, yMinLimit, yMaxLimit);

        // Calculamos la posición de la cámara
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 offset = new Vector3(0f, 0f, -distance);
        transform.position = target.position + rotation * offset;

        // Mantener la cámara mirando al objeto
        transform.LookAt(target);

        // Zoom (móvil y ratón)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            distance = Mathf.Clamp(distance - scroll * zoomSpeed, distanceMin, distanceMax);
        }

        // Para móvil, usar el pinch-to-zoom (si hay dos dedos en la pantalla)
        if (Input.touchCount == 2)
        {
            float touchDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
            float zoomChange = touchDistance * zoomSpeed/2f;
            distance = Mathf.Clamp(distance - zoomChange, distanceMin, distanceMax);
        }
    }
}

