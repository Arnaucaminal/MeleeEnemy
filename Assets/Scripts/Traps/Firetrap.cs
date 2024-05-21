using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firetrap : EnemyDamage
{
    [Header("Firetrap Timers")] 
    [SerializeField] private float activationDelay;
    [SerializeField] private float activeTime;
    private Animator anim;
    private SpriteRenderer spriteRend;
private AudioSource audioSource;
    private bool triggered;
    private bool active;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (!triggered)
            {
                StartCoroutine(ActivateFirtrap());
            }

            if (active)
            {
                Debug.Log("Firetrap: OnTriggerEnter2D activat");
                base.OnTriggerEnter2D(other);
                
            }
        }
    }

    private IEnumerator ActivateFirtrap()
    {
        triggered = true;
        spriteRend.color = Color.red;
        yield return new WaitForSeconds(activationDelay);
        spriteRend.color = Color.white;
        active = true;
        anim.SetBool("activated", true);
        
        audioSource.Play();
        yield return new WaitForSeconds(activeTime);
        anim.SetBool("activated", false);
        active = false;
        triggered = false;
    }
}
