using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        if (cameraType == CameraType.LookAt_Camera)
        {
            transform.LookAt(targetObject.transform);
        }
        else if (cameraType == CameraType.Follow_Camera)
        {
            Vector3 desiredPosition = targetObject.transform.position + m_offset;
            transform.position = desiredPosition;
            //transform.LookAt(targetObject.transform);
        }
    }
}
