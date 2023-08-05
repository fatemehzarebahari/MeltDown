using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class automatePlayer : PlayersOrigin
{
    [SerializeField] private Transform hipLevel; 
    [SerializeField]private float  jumpAnimationLength;
    [SerializeField]private float crunchAnimationLength;
    
    private Transform target;
    private Transform destroyersCenter;
    private float destroyerCurrentSpeed;

    private bool isWaitingToTakeAction = false;
    private float time = 0;
    private float waitingTime = 0;

    private float FallingRangeTimeError = 0.1f;
    private float FallingRangeTimeincreasingAmount = 0.1f;

    private float fallingTimer = 0;
    private float fallingRangeIncreaseTime = 10;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Bar") && other.transform != target)
        {
            target = other.transform;
            // StartCoroutine(ActivateAction(CalculateRemainingTimeBeforeCollision(target)));
            CalculateWaitingTime(CalculateRemainingTimeBeforeCollision(target));
        }
        
    }

    private void Update()
    {
        if (isWaitingToTakeAction)
        {
            time += Time.deltaTime;
            if (time >= waitingTime)
            {
                isWaitingToTakeAction = false;
                time = 0;
                TakeAction();
            }
        }

        if (_inGme)
        {
            fallingTimer += Time.deltaTime;
            if (fallingTimer >= fallingRangeIncreaseTime)
            {
                fallingTimer = 0;
                FallingRangeTimeError += FallingRangeTimeincreasingAmount;
            }
            
        }
    }

    // private IEnumerator ActivateAction(float time)
    // {
    //     
    //     if (IsBarBelowHip())
    //     {
    //         waitingTime = time -  jumpAnimationLength;
    //     }
    //     else
    //     {
    //         waitingTime = time - crunchAnimationLength;
    //     }
    //     yield return new WaitForSeconds(waitingTime);
    //     
    //     if (IsBarBelowHip())
    //     {
    //         Jump();
    //     }
    //     else
    //     {
    //         Crunch();
    //     }
    // }
    private void CalculateWaitingTime(float time)
    {
        if (_inGme)
        {
            if (IsBarBelowHip())
            {
                waitingTime = time - jumpAnimationLength + Random.Range(-FallingRangeTimeError, FallingRangeTimeError);
            }
            else
            {
                waitingTime = time - crunchAnimationLength + Random.Range(-FallingRangeTimeError, FallingRangeTimeError);
            }
        }
        else
        {
            if (IsBarBelowHip())
            {
                waitingTime = time - jumpAnimationLength;
            }
            else
            {
                waitingTime = time - crunchAnimationLength;
            }
        }

        isWaitingToTakeAction = true;
    }
    private void TakeAction()
    {
        if (IsBarBelowHip())
        {
            Jump();
        }
        else
        {
            Crunch();
        }
    }
    private bool IsBarBelowHip()
    {
        return hipLevel.position.y > target.position.y;
    }

    private float CalculateRemainingTimeBeforeCollision(Transform bar)
    {
        Vector3 barVector = bar.position - destroyersCenter.position;
        Vector3 playerVector = transform.position - destroyersCenter.position;

        float angle =  GetCircularDistance(barVector, playerVector);

        return angle / destroyerCurrentSpeed;
    }
    private float GetCircularDistance(Vector3 point1, Vector3 point2)
    {
        Vector2 point1Circle = new Vector2(point1.x, point1.z); 
        Vector2 point2Circle = new Vector2(point2.x, point2.z);

        float circularDistance = Vector2.Angle(point1Circle, point2Circle);
        
        return circularDistance;
    }

    public void SetDestroyersSpeed(float newSpeed)
    {
        destroyerCurrentSpeed = newSpeed;
    }
    public void SetDestroyersCenter(Transform center)
    {
        destroyersCenter = center;
    }

    
}