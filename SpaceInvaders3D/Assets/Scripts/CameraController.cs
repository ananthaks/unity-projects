using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraBoundary
{
    public float xMin = -5.3f;
    public float xMax = 5.3f;
    public float yMin = -2.0f;
    public float yMax = 12.0f;
}
public enum CameraType
{
    Fixed_Camera,
    LookAt_Camera,
    Follow_Camera
};

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject targetObject;
    [SerializeField] CameraType cameraType = CameraType.Fixed_Camera;
    [SerializeField] CameraBoundary cameraBoundary;

    private Vector3 m_offset;

    // Use this for initialization
    void Start ()
    {
        if(cameraType == CameraType.Follow_Camera)
        {
            m_offset = transform.position - targetObject.transform.position;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void LateUpdate()
    {
        if(targetObject == null)
        {
            return;
        }

        if (cameraType == CameraType.LookAt_Camera)
        {
            transform.LookAt(targetObject.transform);
        }
        else if (cameraType == CameraType.Follow_Camera)
        {
            Vector3 desiredPosition = targetObject.transform.position + m_offset;
            transform.position = desiredPosition;
        }

        // Constrain to boundary
        transform.position = new Vector3
            (
            Mathf.Clamp(transform.position.x, cameraBoundary.xMin, cameraBoundary.xMax),
            Mathf.Clamp(transform.position.y, cameraBoundary.yMin, cameraBoundary.yMax),
            transform.position.z
            );
    }
}
