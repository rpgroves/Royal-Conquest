using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject targetObject;
    [SerializeField] float damageToDeal = 20;
    [SerializeField] float moveSpeed = 5;
    [SerializeField] SpriteRenderer mySprite;
    Vector2 targetDirection;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(targetObject == null)
            Destroy(this.gameObject);
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
                enemy.TakeDamage(damageToDeal);
                Destroy(this.gameObject);
            }
        }
    }

    void setTargetDirection(Vector2 td)
    {
        targetDirection = td;
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
