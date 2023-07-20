using UnityEngine;

public class FlyingObject : MonoBehaviour
{
    public float floatingHeight = 5f;
    public float floatingSpeed = 2f;
    public float flyingSpeed = 5f;
    public Transform cameraTransform;
    public float cameraDistance = 3f; // Variable for controlling camera distance

    private Vector3 initialPosition;
    private Vector3 targetFlyingPosition;
    private bool isFloating = false;
    private bool isRotatingLateral = false;
    private bool isRotatingLongitudinal = false;
    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private Vector3 previousMousePosition;
    private Collider objectCollider;

    public float lateralRotationSpeed = 200f;
    public float longitudinalRotationSpeed = 100f;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        objectCollider = GetComponent<Collider>();
    }

    void Update()
    {
        if (isFloating)
        {
            Vector3 targetPosition = new Vector3(transform.position.x, floatingHeight, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, floatingSpeed * Time.deltaTime);

            transform.position = Vector3.MoveTowards(transform.position, targetFlyingPosition, flyingSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, flyingSpeed * Time.deltaTime);

            transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, Time.deltaTime * 5f);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cameraTransform.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider == objectCollider)
            {
                isFloating = !isFloating;

                if (isFloating)
                {
                    targetFlyingPosition = cameraTransform.position + cameraTransform.forward * cameraDistance; // Use the cameraDistance variable here
                }
                else
                {
                    targetRotation = transform.rotation;
                }
            }
        }

        // Only allow rotation when the object is floating
        if (isFloating)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                isRotatingLateral = true;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                isRotatingLateral = true;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                isRotatingLongitudinal = true;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                isRotatingLongitudinal = true;
            }

            if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
            {
                isRotatingLateral = false;
            }

            if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
            {
                isRotatingLongitudinal = false;
            }

            if (isRotatingLateral)
            {
                float rotationAmount = Input.GetKey(KeyCode.RightArrow) ? -1f : 1f;
                transform.Rotate(Vector3.up, rotationAmount * lateralRotationSpeed * Time.deltaTime, Space.World);
            }

            if (isRotatingLongitudinal)
            {
                float rotationAmount = Input.GetKey(KeyCode.DownArrow) ? -1f : 1f;
                transform.Rotate(Vector3.right, rotationAmount * longitudinalRotationSpeed * Time.deltaTime, Space.World);
            }
        }
    }
}