using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject[] arrows;
    private float cooldownTimer;

    private void Attack()
    {
        cooldownTimer = 0;
        arrows[FindArrow()].transform.position = spawnPoint.position;
        arrows[FindArrow()].GetComponent<EnemyProjectile>().ActivateProjectile();
    }

    private int FindArrow()
    {
        int index = 0;
        foreach (var arrow in arrows)
        {
            if (!arrow.activeInHierarchy)
            {
                return index;
            }

            index++;
        }

        return 0;
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer >= attackCooldown)
        {
            Attack();
        }
    }
}
