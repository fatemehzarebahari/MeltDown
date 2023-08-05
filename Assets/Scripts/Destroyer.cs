using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private float speed ;

    void Update()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
    }
    

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    
    
}
