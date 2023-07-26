using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayUp : MonoBehaviour
{
    // Start is called before the first frame update
    private Quaternion originalRotation;
    void Start()
    {
        originalRotation = Quaternion.Euler(transform.rotation.eulerAngles);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rot = transform.rotation.eulerAngles;
        //transform.SetPositionAndRotation(transform.position, Quaternion.Euler(new Vector3(rot.x,originalRotation.eulerAngles.y, rot.z)));
        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(new Vector3(0,0,0)));
    }
}
