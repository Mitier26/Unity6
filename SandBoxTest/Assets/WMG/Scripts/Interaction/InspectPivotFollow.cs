using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectPivotFollow : MonoBehaviour
{
    public Transform cameraTransform;
    public Vector3 offset = new Vector3(0, 0, 5f); // 카메라 앞 2m

    void LateUpdate()
    {
        if (cameraTransform != null)
        {
            transform.position = cameraTransform.position + cameraTransform.forward * offset.z;
            transform.rotation = cameraTransform.rotation;
        }
    }

}
