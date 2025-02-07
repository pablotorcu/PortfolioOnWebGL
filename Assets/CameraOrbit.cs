using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target; // Objeto al que la c�mara siempre estar� mirando
    public float distance = 5f; // Distancia de la c�mara desde el objeto
    public float xSpeed = 250f; // Velocidad de rotaci�n en el eje X
    public float ySpeed = 120f; // Velocidad de rotaci�n en el eje Y
    public float zoomSpeed = 3f; // Velocidad de rotaci�n en el eje Y
    public float yMinLimit = -20f; // L�mite inferior de la rotaci�n vertical
    public float yMaxLimit = 80f; // L�mite superior de la rotaci�n vertical
    public float distanceMin = 2f; // Distancia m�nima de la c�mara
    public float distanceMax = 10f; // Distancia m�xima de la c�mara

    private float x = 0f; // Rotaci�n en el eje X (horizontal)
    private float y = 0f; // Rotaci�n en el eje Y (vertical)

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

        // Control del rat�n
        if (Input.GetMouseButton(0))
        {
            // Movimiento del rat�n
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
        }

        // L�mite de la rotaci�n vertical
        y = Mathf.Clamp(y, yMinLimit, yMaxLimit);

        // Calculamos la posici�n de la c�mara
        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 offset = new Vector3(0f, 0f, -distance);
        transform.position = target.position + rotation * offset;

        // Mantener la c�mara mirando al objeto
        transform.LookAt(target);

        // Zoom (m�vil y rat�n)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            distance = Mathf.Clamp(distance - scroll * zoomSpeed, distanceMin, distanceMax);
        }

        // Para m�vil, usar el pinch-to-zoom (si hay dos dedos en la pantalla)
        if (Input.touchCount == 2)
        {
            float touchDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
            float zoomChange = touchDistance * zoomSpeed/2f;
            distance = Mathf.Clamp(distance - zoomChange, distanceMin, distanceMax);
        }
    }
}

