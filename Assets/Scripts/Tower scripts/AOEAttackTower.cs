using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class used for AOE attack tower, extends the ITower
public class AOEAttackTower : MonoBehaviour, ITower
{

    [SerializeField] private WeaponScriptableObject weaponScriptableObject;
    [SerializeField] private Vector3 offset;
    [SerializeField] private GameObject areaPrefab;

    private GameObject area;
    private float attackTimer = 0f;

    private bool hasBeenBuild = false;
    private bool toStopFunctionality = false;

    private float range;
    private float attackInterval;
    private int damage;

    private SelectableTile attachedTile;

    private int level = 1;
    [SerializeField] private GameObject[] levelGameObjects;

    //Set the values according to scriptable object class attached
    void Awake()
    {
        range = weaponScriptableObject.range;
        attackInterval = weaponScriptableObject.attackInterval;
        damage = weaponScriptableObject.damage;
    }

    //Subscribing to the GameManager actions
    void Start()
    {
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
        if (!hasBeenBuild || toStopFunctionality) return;

        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            AttackEnemiesWithinRange();
            attackTimer = attackInterval;
        }
    }

    //Stop its functionality when the game is won/lost
    private void ToStopFunctionality()
    {
        toStopFunctionality = true;
    }

    //Gets the tile where this tower has been placed on and change its "isTaken" value to true
    //Instantiates the red area around the tower
    public void OnBuild(SelectableTile _tile)
    {
        hasBeenBuild = true;
        area = Instantiate(areaPrefab, transform.position + offset, Quaternion.identity);
        area.transform.localScale = new Vector3(range * 2, area.transform.localScale.y, range * 2);

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

        area.transform.localScale = new Vector3(range * 2, area.transform.localScale.y, range * 2);
    }

    public void Destroy()
    {
        if (attachedTile == null) return;

        attachedTile.isTaken = false;
        Destroy(area);
        Destroy(gameObject);
    }

    //Returns the current level of the tower
    public int GetTheCurrentLevel()
    {
        return level;
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
        Enemy enemyScript = enemy.GetComponent<Enemy>();

        if (enemyScript != null)
        {
            enemyScript.TakeDamage(damage);
        }
    }

}
