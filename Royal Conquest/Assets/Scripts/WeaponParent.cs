using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    [SerializeField] SpriteRenderer characterRenderer, weaponRenderer;
    [SerializeField] Vector2 targetDirection;
    [SerializeField] Vector2 baseDirection;
    [SerializeField] Animator animator;
    [SerializeField] float delay = .4f;
    [SerializeField] Transform circleOrigin;
    [SerializeField] float radius = 0.5f;
    bool attackBlocked = false;

    void Update()
    {
        Vector2 direction;
        if(targetDirection == null)
        {
            direction = baseDirection;
            this.transform.right = -direction; 
        }
        else
        {
            direction = targetDirection;
            this.transform.right = direction; 
        }
    }

    public Vector2 getTargetDirection()
    {
        return targetDirection;
    }

    public void setTargetDirection(Vector2 td)
    {
        targetDirection = td;
    }

    public void Attack()
    {
        if(attackBlocked)
            return;
        animator.SetTrigger("Attack");
        attackBlocked = true;
        StartCoroutine(DelayAttack());
    }

    IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position, radius);
    }
}
