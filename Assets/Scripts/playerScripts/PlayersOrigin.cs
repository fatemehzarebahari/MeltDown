using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class PlayersOrigin : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] protected int _isAlive = 1;
    [SerializeField] private Collider collider;
    [SerializeField] private Slider lifeSlider;
    [SerializeField] private Renderer renderer;
    
    [SerializeField] Color invisibleColor = Color.clear;
    [SerializeField] float blinkInterval = 0.1f; 
    [SerializeField] float blinkDuration = 2f; 

    private Color visibleColor; 
    
    private Coroutine blinkCoroutine;
    protected bool _inGme;
    protected bool isAnimating = false;
    
    private Rigidbody rb;
    private int lifeBarExtention;

    private void Awake()
    {
        Game.instance.AddPlayer(this);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        lifeBarExtention = _isAlive;
        visibleColor = renderer.material.color;
        lifeSlider.transform.localRotation = Quaternion.identity;

        if (!_inGme)
        {
            collider.isTrigger = true;
            lifeSlider.gameObject.SetActive(false);
        }
        
    }
 
  private IEnumerator BlinkCoroutine()
     {
         float startTime = Time.time;
         float elapsedTime = 0f;
 
         while (elapsedTime < blinkDuration)
         {
             renderer.material.color = (elapsedTime / blinkInterval) % 2 < 1 ? visibleColor : invisibleColor;
             yield return new WaitForSeconds(blinkInterval);
             elapsedTime = Time.time - startTime;
         }
 
         renderer.material.color = visibleColor;
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
                    // animator.SetTrigger("Fall");
                    // here deactivate  animator current animation
                    rb.isKinematic = false;
                    _isAlive = 0;
                    Destroy(lifeSlider.gameObject);
                    Game.instance.RemovePlayer(this);
                    
                }
                else
                {
                    Physics.IgnoreCollision(collider, other.collider);
                    if(lifeSlider != null)
                        lifeSlider.value = (float)_isAlive / lifeBarExtention;
                    blinkCoroutine = StartCoroutine(BlinkCoroutine());
                }
            }
        }

    }
    

    protected void Jump()
    {
        if(_isAlive > 0)
            animator.SetTrigger("Jump");
        isAnimating = true;
        StartCoroutine(WaitForAnimationFinish());
    }

    protected void Crunch()
    {
        if(_isAlive > 0)
            animator.SetTrigger("Crunch");
        isAnimating = true;
        StartCoroutine(WaitForAnimationFinish());
    }
    protected IEnumerator WaitForAnimationFinish()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        isAnimating = false;
        animator.SetTrigger("BackToIdle");
    }
    public void SetInGame(bool inGame)
    {
        _inGme = inGame;
    }

    public bool GetPlayerStatus()
    {
        if (_isAlive <= 0)
            return false;
        return true;
    }


}
