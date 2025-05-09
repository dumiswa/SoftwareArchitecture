using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEAttackTowerTest : MonoBehaviour
{
    [SerializeField] private WeaponScriptableObject weaponScriptableObject;
    [SerializeField] private Vector3 offset;
    [SerializeField] private GameObject areaPrefab;

    private GameObject area;
    private float attackTimer = 0f;

    private float range;
    private float attackInterval;
    private int damage;

    //Set the values according to scriptable object class attached
    void Awake()
    {
        range = weaponScriptableObject.range;
        attackInterval = weaponScriptableObject.attackInterval;
        damage = weaponScriptableObject.damage;
    }

    private void Start()
    {
        area = Instantiate(areaPrefab, transform.position + offset, Quaternion.identity);
        area.transform.localScale = new Vector3(range * 2, area.transform.localScale.y, range * 2);
    }

    void Update()
    {

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            AttackEnemiesWithinRange();
            attackTimer = attackInterval;
        }
    }

    //Finds enemies that are within the range
    private void AttackEnemiesWithinRange()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(weaponScriptableObject.enemyTag);

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy <= range)
            {
                DamageEnemy(enemy);
            }
        }
    }

    //Damages the enemies that are within the range
    private void DamageEnemy(GameObject enemy)
    {
        EnemyTest enemyScript = enemy.GetComponent<EnemyTest>();

        if (enemyScript != null)
        {
            enemyScript.TakeDamage(damage);
        }
    }
}
