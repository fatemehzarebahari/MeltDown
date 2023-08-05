using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class characterSelectionCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float transitionDuration = 5f;
    [SerializeField] private float distanceFromCharacter = 10f;

    [SerializeField] private GameObject MenuObject;

    void Start()
    {
        Vector3 targetDest = target.position;
        if (targetDest.z > transform.position.z)
        {
            targetDest.z -= distanceFromCharacter;
        }
        else
        {
            targetDest.z += distanceFromCharacter;
        }

        transform.DOMove(targetDest, transitionDuration);
        transform.DORotate(new Vector3(0, 0, 0), transitionDuration).OnComplete(MenuAppear);
    }

    private void MenuAppear()
    {
        MenuObject.SetActive(true);
    }

}
