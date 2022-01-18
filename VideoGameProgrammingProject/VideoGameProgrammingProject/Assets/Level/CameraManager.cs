using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Andrea Mansi - Università Degli Studi di Udine

    [Header(" - - - Camera Main Settings - - - ")]

    [Tooltip("If True, camera will always look at targer")]
    [SerializeField] bool lookAtTarget = true;

    [Tooltip("If True, camera will follow target in x and z plane")]
    [SerializeField] bool followTarget = true;

    [SerializeField] GameObject targetRef;

    [Tooltip("An offset applied to the target position applied when setting the lookAt point")]
    [SerializeField] Vector3 targetPosOffset;

    [SerializeField] float minOffset = 20.0f;
    [SerializeField] float maxOffset = 60.0f;
    [SerializeField] float zOffset = 10.0f;
    [SerializeField] float zoomSensitivity;

    void Start()
    {
        targetPosOffset += new Vector3(0, 0, zOffset);
    }

    void Update()
    {
        float scroll = zoomSensitivity * Input.GetAxis("Mouse ScrollWheel");
        Vector3 change = new Vector3(0, -scroll, -scroll);
        targetPosOffset = targetPosOffset + change;
        
        // Clamp between min and max
        targetPosOffset = new Vector3(targetPosOffset.x, Mathf.Clamp(targetPosOffset.y, minOffset, maxOffset), Mathf.Clamp(targetPosOffset.z, minOffset+ zOffset, maxOffset+ zOffset));

        transform.position = targetRef.transform.position + targetPosOffset;

        if (lookAtTarget)
        {
            transform.LookAt(targetRef.transform);
        }
        

    }
}
