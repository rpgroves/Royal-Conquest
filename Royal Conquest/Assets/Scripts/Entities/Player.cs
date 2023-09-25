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
        HandleMouse();
    }

    void HandleMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        weaponParent.setTargetDirection(-(Camera.main.ScreenToWorldPoint(mousePos) - gameObject.transform.position).normalized);
        //weaponParent.setTargetDirection(-(currentTarget.transform.position - gameObject.transform.position).normalized);
        //Debug.Log(mousePos);
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
