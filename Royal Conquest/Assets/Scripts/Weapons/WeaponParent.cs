using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    [SerializeField] SpriteRenderer characterRenderer, weaponRenderer;
    [SerializeField] Vector2 targetDirection;
    [SerializeField] Vector2 baseDirection;
    public Animator animator;
    [SerializeField] float delay = .4f;
    [SerializeField] Transform circleOrigin;
    [SerializeField] float radius = 0.5f;
    public bool attackBlocked = false;
    public bool isAttacking = false;
    public GameObject targetObject;
    [SerializeField] GameObject me;
    [SerializeField] bool isSplashDamager = false;

    public void ResetIsAttacking()
    {
        isAttacking = false;
    }

    void Update()
    {
        HandleAim();
    }

    public void HandleAim()
    {
        if(isAttacking)
            return;
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

        Vector2 scale = transform.localScale;
        if(direction.x < 0)
        {
            scale.y = -1;
        }
        if(direction.x > 0)
        {
            scale.y = 1;
        }
        transform.localScale = scale;
    }

    public Vector2 getTargetDirection()
    {
        return targetDirection;
    }

    public void setTargetDirection(Vector2 td)
    {
        targetDirection = td;
    }

    public bool getIsAttacking()
    {
        return isAttacking;
    }

    public void setIsAttacking(bool ia)
    {
        isAttacking = ia;
    }

    public virtual void Attack()
    {
        isAttacking = true;
        if(attackBlocked)
            return;
        animator.SetTrigger("Attack");
        StartCoroutine(DelayAttack());
    }

    public IEnumerator DelayAttack()
    {
        //Debug.Log("Delaying");
        attackBlocked = true;
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
        //Debug.Log("Delay over");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position, radius);
    }

    public void DetectColliders()
    {
        //Debug.Log("Detecting colliders");
        foreach(Collider2D collider in Physics2D.OverlapCircleAll(circleOrigin.position, radius))
        {
            //Debug.Log("Hit a dude " + collider.gameObject.name);
            if(collider.gameObject == me)
                continue;
            if(collider.isTrigger == false)
            {
                //Debug.Log("Hit a dude " + collider.gameObject.name);
                
                if(!isSplashDamager && collider.gameObject == targetObject)
                {
                    collider.gameObject.GetComponent<Entity>().TakeDamage(me.GetComponent<Entity>().getDamageDealt(), me.GetComponent<Entity>().getKnockback(), gameObject.transform.position);
                }
                else if(isSplashDamager)
                {
                    if(collider.gameObject.tag == "Unit" || collider.gameObject.tag == "Building" || collider.gameObject.tag == "Player")
                        if(collider.gameObject.GetComponent<Entity>().getTeam() != me.GetComponent<Entity>().getTeam())
                            collider.gameObject.GetComponent<Entity>().TakeDamage(me.GetComponent<Entity>().getDamageDealt(), me.GetComponent<Entity>().getKnockback(), gameObject.transform.position);
                }
            }
        }
    }

    public void setTargetObject(GameObject to)
    {
        targetObject = to;
    }
}
