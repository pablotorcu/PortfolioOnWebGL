using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] private Animator _dragInstruction;
    public float rotationSpeed = 5f;
    private Vector3 lastPosition;
    private bool isDragging = false;

    private void OnEnable()
    {
        isDragging = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastPosition = Input.mousePosition;
            isDragging = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

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

        if (isDragging)
        {
            Vector3 delta = Vector3.zero;
            _dragInstruction.SetBool("On", false);
            if (Input.touchCount > 0) // Entrada táctil
            {
                delta = (Vector3)Input.GetTouch(0).position - lastPosition;
            }
            else if (Input.GetMouseButton(0)) // Entrada de ratón
            {
                delta = Input.mousePosition - lastPosition;
            }

            float rotationAmount = delta.x * rotationSpeed * Time.deltaTime;

            transform.Rotate(0, -rotationAmount, 0, Space.World);

            lastPosition = Input.touchCount > 0 ? (Vector3)Input.GetTouch(0).position : Input.mousePosition;
        }
    }
}

