using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public GameObject spawnPoint; // where the projectile will spawn
    public InputActionReference attack;

    public bool isAttacking;
    public bool shotProjectile;
    public bool isCharging;

    public float chargeTime = 1f; // time it takes to charge an attack

    private ObjectPool objPool;
    private ShootProjectile shootProjectile;

    private float timer = 0f;
    private float currentChargeTime; // time attack has been charging up

    void Start()
    {
        objPool = GetComponent<ObjectPool>();
        shootProjectile = new ShootProjectile(objPool);
        isAttacking = false;
        currentChargeTime = 0;
    }

    private void Update()
    {
        // if player is pressing the attack button then true
        isAttacking = attack.action.ReadValue<float>() > 0.5f ? true : false;

        // if player is holding the attack button charge up an attack then fire projectile
        if (isAttacking)
        {
            if (!isCharging)
            {
                isCharging = true;
                currentChargeTime = 0f;
            }

            currentChargeTime += Time.deltaTime;

            if (currentChargeTime >= chargeTime)
            {
                StartCoroutine(Attack());
                isCharging = false;
            }
        }
        else
        {
            isCharging = false;
            currentChargeTime = 0f;
        }
    }

    public IEnumerator Attack()
    {
        shotProjectile = true;
        
        yield return null;

        shotProjectile = false;

        shootProjectile.Shoot(spawnPoint);
    }
}
