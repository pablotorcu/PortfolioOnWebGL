using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;  // El objeto alrededor del cual rotará la cámara
    public float distance = 10f;  // Distancia inicial de la cámara desde el objeto
    public float height = 5f;  // Altura inicial de la cámara
    public float rotationSpeed = 5f;  // Velocidad de rotación
    public float zoomSpeed = 2f;  // Velocidad del zoom

    public bool onlyHorizontal;
    private float currentRotationX = 0f;
    private float currentRotationY = 0f;
    private Vector3 offset;

    void Start()
    {
        // Establecer la posición inicial de la cámara con base en la distancia y altura
        offset = new Vector3(0, height, -distance);
        transform.position = target.position + offset;
        transform.LookAt(target);  // La cámara siempre mira al objetivo
    }

    void Update()
    {
        // Movimiento y rotación de la cámara
        if (Input.GetMouseButton(0))
        {
            float horizontal = Input.GetAxis("Mouse X");
            float vertical = Input.GetAxis("Mouse Y");

            if (onlyHorizontal)
            {
                vertical = 0;
            }

            currentRotationX += horizontal * rotationSpeed;
            currentRotationY -= vertical * rotationSpeed;
        }

        // Limitar la rotación vertical (evitar que la cámara se voltee)
        currentRotationY = Mathf.Clamp(currentRotationY, -80f, 80f);

        // Zoom en PC con la rueda del ratón
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            distance -= scroll * zoomSpeed * 100f * Time.deltaTime; // Cambiar la distancia en función del scroll
        }

        // Zoom en dispositivos móviles con el gesto de pellizcar
        if (Input.touchCount == 2) // Si hay dos toques en la pantalla (pellizcar)
        {
            float touchDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
            float previousTouchDistance = Vector2.Distance(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition,
                                                            Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

            float pinchAmount = touchDistance - previousTouchDistance;
            distance -= pinchAmount * zoomSpeed * 0.1f * Time.deltaTime; // Ajustar el zoom con el gesto de pellizcar
        }

        // Limitar la distancia de zoom
        distance = Mathf.Clamp(distance, 5f, 20f);

        // Calcular la nueva posición de la cámara
        Quaternion rotation = Quaternion.Euler(currentRotationY, currentRotationX, 0);
        transform.position = target.position + rotation * new Vector3(0, height, -distance);

        // Hacer que la cámara siempre mire al objeto
        transform.LookAt(target);
    }
}
