using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeaponParent : WeaponParent
{
    [SerializeField] GameObject ProjectilePrefab;

    void Update()
    {
        HandleAim();
    }

    public override void Attack()
    {
        isAttacking = true;
        if(attackBlocked)
            return;
        animator.SetTrigger("Attack");
        GameObject projectile = Instantiate(ProjectilePrefab, gameObject.transform);
        projectile.GetComponent<Projectile>().setTargetObject(targetObject);
        StartCoroutine(DelayAttack());
    }
}
