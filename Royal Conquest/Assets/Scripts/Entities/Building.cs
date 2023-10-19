using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Entity
{
    void Start()
    {
        HealthBar healthBar = Instantiate(healthBarPrefab, GameObject.Find("Canvas").transform).GetComponent<HealthBar>();
        healthBar.CreateBar(this);
    }

    void Update()
    {
        HandleHealing();
    }

    public override void TakeDamage(float damage, float knockback, Vector3 enemyPosition)
    {
        timeSinceLastHeal = 0;
        entityHealth -= damage;
        StartCoroutine(TurnSpriteRed());
        if(entityHealth <= 0)
            HandleEntityDeath();
    }
}
