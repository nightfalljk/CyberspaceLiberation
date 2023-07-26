using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CameraFacingBillboard : MonoBehaviour
{
    public Camera m_Camera;
    public Vector3 Rotation;

    private void OnEnable()
    {
        if(m_Camera==null)
            m_Camera = Camera.main;
    }

    //Orient the camera after all movement is completed this frame to avoid jittering
    void LateUpdate()
    {
        float z = -90;
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward, m_Camera.transform.rotation * Vector3.up);
        //transform.rotation.eulerAngles.z = z;
        //transform.eulerAngles = 
        transform.localEulerAngles = new Vector3(transform.localRotation.eulerAngles.x+Rotation.x, transform.localRotation.eulerAngles.y+Rotation.y, z+Rotation.z);
        //transform.SetPositionAndRotation(transform.position,Quaternion.Euler());
    }
}
