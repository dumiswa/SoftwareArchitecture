using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used for both single attack/slow down attack towers, extends the ITower
public class ShootingAttackTower : MonoBehaviour, ITower
{
    [SerializeField] private WeaponScriptableObject weaponScriptableObject;
    [SerializeField] private Transform smallPartToRotate;
    [SerializeField] private Transform bigPartToRotate;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float rotationSpeed = 10f;
    private Transform target;
    private float attackTimer = 0f;

    private bool hasBeenBuild = false;
    private bool toStopFunctionality = false;

    private float range;
    private float attackInterval;
    private int damage;

    SelectableTile attachedTile;

    private int level = 1;
    [SerializeField] private GameObject[] levelGameObjects;

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

        GameManager.Instance.onGameWon += ToStopFunctionality;
        GameManager.Instance.onGameLost += ToStopFunctionality;
    }

    //Unsubscribing from the GameManager actions
    private void OnDisable()
    {
        GameManager.Instance.onGameWon -= ToStopFunctionality;
        GameManager.Instance.onGameLost -= ToStopFunctionality;
    }

    void Update()
    {
        if (target == null || !hasBeenBuild || toStopFunctionality) return;

        RotateTowardsTheTarget();
        Shoot();
    }

    //Stop its functionality when the game is won/lost
    private void ToStopFunctionality()
    {
        toStopFunctionality = true;
    }

    //Gets the tile where this tower has been placed on and change its "isTaken" value to true
    public void OnBuild(SelectableTile _tile)
    {
        hasBeenBuild = true;

        attachedTile = _tile;
        attachedTile.isTaken = true;
    }

    //Returns the weapon scriptable object attached to this class
    public WeaponScriptableObject GetWeaponScriptableObject()
    {
        return weaponScriptableObject;
    }

    //Returns the current range of this tower
    public float GetCurrentRange()
    {
        return range;
    }

    //Returns the current attack interval of this tower
    public float GetCurrentAttackInterval()
    {
        return attackInterval;
    }

    //Returns if the tower has been fully upgraded
    public bool IsFullyUpgraded()
    {
        if (level >= levelGameObjects.Length)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Upgrades its values
    public void Upgrade(float _range, float _attackInterval)
    {
        if (IsFullyUpgraded()) return;

        level++;

        for (int i = 0; i < levelGameObjects.Length; i++)
        {
            if (i == level - 1)
            {
                levelGameObjects[i].SetActive(true);
            }
            else
            {
                levelGameObjects[i].SetActive(false);
            }
        }

        range += _range;
        attackInterval -= _attackInterval;
    }

    public void Destroy()
    {
        if (attachedTile == null) return;

        attachedTile.isTaken = false;
        Destroy(gameObject);
    }

    //Returns the current level of the tower
    public int GetTheCurrentLevel()
    {
        return level;
    }

    //Instantiates a bullet prefab based on the attack interval value
    private void Shoot()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            Transform projectile = Instantiate(weaponScriptableObject.attackPrefab, firePoint.position, firePoint.rotation);

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
