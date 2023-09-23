using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] float entityHealth = 10.0f;
    [SerializeField] float entityMaxHealth = 100.0f;
    [SerializeField] float healingPerSecond = 5.0f;
    [SerializeField] float healingCooldown = 5.0f;
    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] string team = "";
    float timeSinceLastHeal = 0;
    Vector3 myDirection;
    CircleCollider2D myRangeCollider;
    [SerializeField] bool isEntityInControl = true;

    void Start()
    {
        myRangeCollider = gameObject.GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        HandleMovement();
        HandleHealing();
    }

    public void HandleMovement()
    {
        if(isEntityInControl && myDirection != null)
        {
            Vector3 delta = myDirection.normalized * moveSpeed * Time.deltaTime;
            transform.position += delta;
        
            /*if(myDirection.x == 0 && myDirection.y == 0)
            {
                myAnimator.SetBool("isRunning", false);
                myAnimator.SetBool("isIdle", true);
                return;
            }
            
            myAnimator.SetBool("isRunning", true);
            myAnimator.SetBool("isIdle", false);

            myDirection.Normalize();
            myAnimator.SetFloat("X Comp", myDirection.x);
            myAnimator.SetFloat("Y Comp", myDirection.y);*/
        }
    }

    public void HandleHealing()
    {
        timeSinceLastHeal += Time.deltaTime;
        if(timeSinceLastHeal >= healingCooldown && entityHealth < entityMaxHealth)
        {
            Heal(healingPerSecond * Time.deltaTime);
        }
    }

    public void ChangeMoveVector(Vector2 v)
    {
        myDirection = v;
    }

    public void TakeDamage(int damage)
    {
        timeSinceLastHeal = 0;
        entityHealth -= damage;
        if(entityHealth <= 0)
            HandleEntityDeath();
    }

    public void Heal(float heal)
    {
        entityHealth += heal;
        if(entityHealth >= entityMaxHealth)
            entityHealth = entityMaxHealth;
    }

    public void HandleEntityDeath()
    {
        Debug.Log("Death!");
    }

    public string getTeam()
    {
        return team;
    }

    public float getHealth()
    {
        return entityHealth;
    }

    public float getMaxHealth()
    {
        return entityMaxHealth;
    }
}
