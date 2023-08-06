using UnityEngine;

public class Bar : MonoBehaviour
{
    [SerializeField] Transform destroyer;

    public Transform GetDestroyerCenter()
    {
        return destroyer.transform;
    }

    public float GetDestroyerSpeed()
    {
        return destroyer.GetComponent<Destroyer>().GetSpeed();
    }
}
