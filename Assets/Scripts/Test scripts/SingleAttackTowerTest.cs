using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAttackTowerTest : MonoBehaviour
{
    [SerializeField] private WeaponScriptableObject weaponScriptableObject;
    [SerializeField] private Transform smallPartToRotate;
    [SerializeField] private Transform bigPartToRotate;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private Transform bullet;
    private Transform target;
    private float attackTimer = 0f;

    private float range;
    private float attackInterval;
    private int damage;


    //Set the values according to scriptable object class attached
    private void Awake()
    {
        range = weaponScriptableObject.range;
        attackInterval = weaponScriptableObject.attackInterval;
        damage = weaponScriptableObject.damage;
    }

    //Subscribing to the GameManager actions
    //Updates the closest enemy every 0.5 seconds
    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0, 0.5f);
    }

    void Update()
    {
        if (target == null) return;
        RotateTowardsTheTarget();
        Shoot();
    }

    //Instantiates a bullet prefab based on the attack interval value
    private void Shoot()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            Transform projectile = Instantiate(bullet, firePoint.position, firePoint.rotation);

            if (projectile != null)
            {
                projectile.GetComponent<Projectile>().SetTheValues(target, damage);
            }

            attackTimer = attackInterval;
        }

    }

    //Rotates its parts towards the closest enemy
    private void RotateTowardsTheTarget()
    {
        Vector3 directionToRotate = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(directionToRotate);

        Vector3 bigPartRotation = Quaternion.Lerp(bigPartToRotate.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
        bigPartToRotate.rotation = Quaternion.Euler(0, bigPartRotation.y, 0);

        Vector3 smallPartRotation = Quaternion.Lerp(smallPartToRotate.rotation, lookRotation, Time.deltaTime * rotationSpeed).eulerAngles;
        smallPartToRotate.localRotation = Quaternion.Euler(smallPartRotation.x, smallPartToRotate.localRotation.y, 0);
    }

    //Update the closest enemy transform
    private void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(weaponScriptableObject.enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }
}
