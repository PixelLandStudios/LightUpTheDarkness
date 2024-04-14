using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
        }
    }

    void LateUpdate()
    {
        if (mainCamera != null)
        {
            // Make the canvas face the camera
            //transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
            //                 mainCamera.transform.rotation * Vector3.up);

            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward);

            // Optionally, ensure the canvas is always at a fixed distance from the camera
            //transform.position = mainCamera.transform.position + mainCamera.transform.forward * 10f;
        }
    }
}
