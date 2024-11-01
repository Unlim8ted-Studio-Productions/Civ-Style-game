using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 50f; // Speed for panning the camera
    public float zoomSpeed = 2f; // Speed for zooming in and out
    public float minZoom = 1f; // Minimum orthographic size
    public float maxZoom = 50f; // Maximum orthographic size

    private Vector3 dragOrigin;
    public GameObject cam;


    void Update()
    {
        HandleMovement();
        HandleZoomAndPan();
    }

    void HandleMovement()
    {
        // Panning using WASD or arrow keys
        Vector3 move = new Vector3();

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            move.z += 1;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            move.z -= 1;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            move.x -= 1;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            move.x += 1;

        // Apply panning movement
        transform.Translate(move * panSpeed * Time.deltaTime, Space.World);
    }

    void HandleZoomAndPan()
    {
        // Zooming by changing the orthographic size
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Vector3 zoom = cam.transform.position;
        zoom.y -= scroll * zoomSpeed;

        // Clamp the orthographic size within the min/max range
        zoom.y = Mathf.Clamp(zoom.y, minZoom, maxZoom);

        cam.transform.position=zoom;

        // Panning with left mouse button
        if (Input.GetMouseButtonDown(2))
            dragOrigin = Input.mousePosition;

        if (Input.GetMouseButton(2))
        {
            Vector3 difference = dragOrigin - Input.mousePosition;
            dragOrigin = Input.mousePosition;

            Vector3 pan = new Vector3(difference.x * 0.1f, 0, difference.y * 0.1f);
            transform.Translate(pan * panSpeed * Time.deltaTime, Space.World);
        }
    }

}
