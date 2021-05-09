using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Target;
    public float CameraSpeed;
    public Vector3 Offset;

    public void Update()
    {
        if(Target != null)
            FollowCamera();
    }

    public void FollowCamera()
    {
        Vector3 pos = new Vector3(Target.transform.position.x - Offset.x, Offset.y,
            Target.transform.position.z - Offset.z);
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * CameraSpeed);
    }

}
