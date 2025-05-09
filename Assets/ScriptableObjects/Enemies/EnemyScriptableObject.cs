using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enemy scriptable objects that has all the enemy values
[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    public int health;
    public float speed;
    public int carriedMoney;
}
