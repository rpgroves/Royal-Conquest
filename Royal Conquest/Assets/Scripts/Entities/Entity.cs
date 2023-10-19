using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour
{
    public float entityHealth = 10.0f;
    [SerializeField] float entityMaxHealth = 100.0f;
    [SerializeField] float healingPerSecond = 5.0f;
    [SerializeField] float healingCooldown = 5.0f;
    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] string team = "";
    [SerializeField] float damageDealt = 15.0f;
    [SerializeField] float entityKnockback = 1.0f;
    [SerializeField] Animator myAnimator;
    [SerializeField] SpriteRenderer mySprite;
    [SerializeField] float turnRedDelay = .3f;
    public UnityEvent OnDeath;
    public float timeSinceLastHeal = 0;
    Vector3 myDirection;
    public CircleCollider2D myRangeCollider;
    public bool isEntityInControl = true;
    public GameObject healthBarPrefab;

    void Start()
    {
        myRangeCollider = gameObject.GetComponent<CircleCollider2D>();
        HealthBar healthBar = Instantiate(healthBarPrefab, GameObject.Find("Canvas").transform).GetComponent<HealthBar>();
        healthBar.CreateBar(this);
    }

    void Update()
    {
        if(!isEntityInControl)
            return;
        HandleMovement();
        HandleHealing();
    }

    public void HandleMovement()
    {
        if(myDirection != null)
        {
            Vector3 delta = myDirection.normalized * moveSpeed * Time.deltaTime;
            transform.position += delta;

            Vector3 v = new Vector3(1, 1, 1);
            myDirection.Normalize();
            if(myDirection.x > 0)
                v.x = -1;

            mySprite.gameObject.transform.localScale = v;

            if(myAnimator == null)
                return;

            if(myDirection.x == 0 && myDirection.y == 0)
            {
                myAnimator.SetBool("isRunning", false);
                return;
            }
            
            myAnimator.SetBool("isRunning", true);
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

    public virtual void TakeDamage(float damage, float knockback, Vector3 enemyPosition)
    {
        Vector2 knockbackDirection = (Vector2)(enemyPosition - gameObject.transform.position);
        gameObject.GetComponent<Rigidbody2D>().AddForce(-knockbackDirection * knockback);
        timeSinceLastHeal = 0;
        entityHealth -= damage;
        StartCoroutine(TurnSpriteRed());
        if(entityHealth <= 0)
            HandleEntityDeath();
    }

    public IEnumerator TurnSpriteRed()
    {
        //Debug.Log("red color");
        mySprite.color = Color.red;
        yield return new WaitForSeconds(turnRedDelay);
        mySprite.color = Color.white;
        //Debug.Log("white color");
    }

    public void Heal(float heal)
    {
        entityHealth += heal;
        if(entityHealth >= entityMaxHealth)
            entityHealth = entityMaxHealth;
    }

    public void HandleEntityDeath()
    {
        //Debug.Log("Death!");
        Destroy(this.gameObject);
        OnDeath?.Invoke();
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

    public float getDamageDealt()
    {
        return damageDealt;
    }

    public void setEntityInControl(bool ieic)
    {
        isEntityInControl = ieic;
    }

    public float getKnockback()
    {
        return entityKnockback;
    }
}
