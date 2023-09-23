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
}
