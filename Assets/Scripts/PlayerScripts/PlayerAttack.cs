using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackDelay;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Projectile[] fireBalls;
    
    private float attackTimer = Mathf.Infinity;
    private Animator anim;
    private Player _player;
    
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
        _player = GetComponent<Player>();
        
    }

  void Update()
{
    attackTimer += Time.deltaTime;
    if (Input.GetKey(KeyCode.F) && attackTimer > attackDelay && _player.CanPerformAttack())
    {
        Attack();
    }
}


    private void Attack()
    {
        anim.SetTrigger("attack");
        Debug.Log("ATTACK");
        attackTimer = 0.0f;

        int index = FindFireball();
        fireBalls[index].transform.position = firePoint.position;
        fireBalls[index].SetDirection(Mathf.Sign(transform.localScale.x));
        
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireBalls.Length; i++)
        {
            if (!fireBalls[i].gameObject.activeInHierarchy)
            {
                return i;
            }
        }

        return 0;
    }
}
