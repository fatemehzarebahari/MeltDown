using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PlayersOrigin : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] protected int _isAlive = 1;

    protected bool _inGme;
    
    private Rigidbody rb;
    private int lifeBarExtention;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        lifeBarExtention = _isAlive;
    }

    protected void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Bar"))
        {
            if (_inGme)
            {
                _isAlive--;
                if (_isAlive <= 0)
                {
                    Vector3 direction = new Vector3(transform.position.x, -transform.position.y, transform.position.z);

                    // animator.SetTrigger("Fall");
                    // here deactivate  animator current animation
                    rb.isKinematic = false;
                    rb.AddForce(direction * 150);
                    _isAlive = 0;
                }
            }
            // else
            //     _isAlive--;
        }

        // if (other.transform.CompareTag("Ground"))
        // {
        //     animator.SetTrigger("Die");
        //
        // }

    }
    

    protected void Jump()
    {
        if(_isAlive > 0)
            animator.SetTrigger("Jump");
        StartCoroutine(WaitForAnimationFinish());

    }

    protected void Crunch()
    {
        if(_isAlive > 0)
            animator.SetTrigger("Crunch");
        StartCoroutine(WaitForAnimationFinish());


    }
    protected IEnumerator WaitForAnimationFinish()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        animator.SetTrigger("BackToIdle");

    }
    public void SetInGame(bool inGame)
    {
        _inGme = inGame;
    }


}
