using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private GameObject playerObj;
    private Transform playerTransform;
    public float smoothTime = 0.1f;
    private Vector3 velocity = Vector3.zero;
    public bool isCameraManual = false;
    private Vector3 manualPosition;
    public float x;
    public float y;

    void Start()
    {
        playerObj = GameObject.Find("Player");
        playerTransform = playerObj.transform;
    }
    private void LateUpdate()
    {
        if (playerTransform != null)
        {
            if (!isCameraManual)
            {
                Vector3 targetPosition = playerTransform.position;
                targetPosition.z = transform.position.z;
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            }
            else
            {
                manualPosition = new Vector3(x, y, transform.position.z);
                transform.position = manualPosition;
            }
        }

    }
}
