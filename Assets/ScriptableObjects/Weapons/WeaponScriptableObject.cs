using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Weapon/Tower scriptable objects that has all the Weapon/Tower values
[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    public string weaponName;
    public string attackTypeDescription;

    public int damage;
    public float range;
    public float attackInterval;
    public int cost;

    public GameObject weaponPrefab;

    public Transform attackPrefab;
    public string enemyTag;

    public Sprite enemySprite;
    public float rangeToAdd;
    public float attackIntervalToSubstract;
    public int upgradeCost;
}
