using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    [SerializeField] WeaponParent weaponParent;

    void Start()
    {
        myRangeCollider = gameObject.GetComponent<CircleCollider2D>();
        HealthBar healthBar = Instantiate(healthBarPrefab, GameObject.Find("Canvas").transform).GetComponent<HealthBar>();
        healthBar.CreateBar(this);
    }

    void Update()
    {
        HandleMovement();
        HandleHealing();
    }

    void OnMove(InputValue value)
    {
        //Debug.Log("Moving!");
        ChangeMoveVector(value.Get<Vector2>());
    }

    void OnAttack()
    {
        weaponParent.Attack();
    }
}
