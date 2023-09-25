using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    GameObject targetObject;
    float damageToDeal = 20;
    float knockback = 0.0f;
    [SerializeField] float moveSpeed = 5;
    [SerializeField] SpriteRenderer mySprite;
    Vector2 targetDirection;

    public void ProjectileSetup(float dmg, float kb)
    {
        damageToDeal = dmg;
        knockback = kb;
    }

    // Update is called once per frame
    void Update()
    {
        setTargetDirection((targetObject.transform.position - gameObject.transform.position).normalized);
        HandleMovement();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.isTrigger == true)
            return;
        if(other.TryGetComponent<Entity>(out Entity enemy))
        {
            if(enemy.gameObject == targetObject)
            {
                enemy.TakeDamage(damageToDeal, knockback, gameObject.transform.position);
                Destroy(this.gameObject);
            }
        }
    }

    void setTargetDirection(Vector2 td)
    {
        targetDirection = td;
    }

    public void setTargetObject(GameObject to)
    {
        targetObject = to;
        if(targetObject.TryGetComponent<Entity>(out Entity enemy))
            enemy.OnDeath.AddListener(EntityDied);
    }

    void EntityDied()
    {
        Destroy(this.gameObject);
    }

    void HandleMovement()
    {
        Vector3 delta = targetDirection.normalized * moveSpeed * Time.deltaTime;
        transform.position += delta;

        Vector3 v = new Vector3(1, 1, 1);
        targetDirection.Normalize();
        if(targetDirection.x > 0)
            v.x = -1;
        mySprite.gameObject.transform.localScale = v;

        Vector2 direction;
        direction = targetDirection;
        this.transform.right = direction; 
    }
}
