using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private bool hit;
    private Animator anim;
    private BoxCollider2D collider;
    private float direction;
    private float lifeTime;
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        
    }

    void Update()
    {
        if (hit)
        {
            return;
        }

        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0 ,0);

        lifeTime += Time.deltaTime;
        if (lifeTime > 5)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
{
    hit = true;
    collider.enabled = false;
    anim.SetTrigger("explode");
    if (other.CompareTag("Enemy")) 
    {
      
        other.GetComponent<EnemyController>().TakeDamage();
   
    }        
}


    public void SetDirection(float direction)
    {
        lifeTime = 0.0f;
        this.direction = direction;
        gameObject.SetActive(true);
        collider.enabled = true;
        hit = false;

        float localScaleX =transform.localScale.x;
        if ( Mathf.Sign(localScaleX) != direction)
        {
            transform.localScale = new Vector3(-localScaleX, transform.localScale.y, transform.localScale.z);
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}