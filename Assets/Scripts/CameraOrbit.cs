using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public float rotationSpeed = 5f;  // Velocidad de rotación de la cámara

    private Vector3 lastPosition;
    private bool isDragging = false;

    void Update()
    {
        // Detectar la entrada de la PC (ratón)
        if (Input.GetMouseButtonDown(0))
        {
            lastPosition = Input.mousePosition;
            isDragging = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // Detectar la entrada táctil para dispositivos móviles
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                lastPosition = Input.GetTouch(0).position;
                isDragging = true;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                isDragging = false;
            }
        }

        // Si está arrastrando, rotar la cámara
        if (isDragging)
        {
            Vector3 delta = Vector3.zero;

            if (Input.touchCount > 0) // Entrada táctil
            {
                delta = (Vector3)Input.GetTouch(0).position - lastPosition;
            }
            else if (Input.GetMouseButton(0)) // Entrada de ratón
            {
                delta = Input.mousePosition - lastPosition;
            }

            // Aplicar la rotación solo en el eje Y
            float rotationAmount = delta.x * rotationSpeed * Time.deltaTime;

            transform.Rotate(0, -rotationAmount, 0, Space.World);

            lastPosition = Input.touchCount > 0 ? (Vector3)Input.GetTouch(0).position : Input.mousePosition;
        }
    }
}

