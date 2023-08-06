using System.Collections;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [SerializeField] private float speed = 30;
    [SerializeField] private float increasingSpeedWaitingTime = 10;
    [SerializeField] private float maxSpeed = 100f;
    [SerializeField] private float upgradeOffset = 10;
    
    
    private Rigidbody rb;
    private bool _increaseSpeed = true;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float angularVelocity = speed * Mathf.Deg2Rad;
        rb.angularVelocity = new Vector3(0f, angularVelocity, 0f);
        
        if (_increaseSpeed)
        {
            _increaseSpeed = false;
            StartCoroutine(IncreaseDestroyersSpeed());
        }
    }
    
    

    public float GetSpeed()
    {
        return speed;
    }
    
    private IEnumerator IncreaseDestroyersSpeed()
    {
        yield return new WaitForSeconds(increasingSpeedWaitingTime);
        
        if(speed < maxSpeed)
        {
            speed += upgradeOffset;
        }
        // if(increasingSpeedWaitingTime > 5) increasingSpeedWaitingTime -=0.1; 
        // by uncommenting the above line the time to increase speed will get shorter until it becomes 5
        _increaseSpeed = true;

    }
    
}
