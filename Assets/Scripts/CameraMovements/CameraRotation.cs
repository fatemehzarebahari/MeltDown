using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    
    [SerializeField] Transform rotationCenter;
    [SerializeField] float rotationSpeed = 5.0f;

    private void Update()
    {
        float rotationAngle =  rotationSpeed * Time.deltaTime;
        transform.RotateAround(rotationCenter.position, Vector3.up, rotationAngle);
        transform.LookAt(rotationCenter);
    }
}
