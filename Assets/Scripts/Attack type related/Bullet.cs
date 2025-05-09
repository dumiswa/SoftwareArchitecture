using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Bullet class to move the bullet prefab and damage the enemy, extends base class "Projectile"
public class Bullet : Projectile
{
    [SerializeField] private float speed;

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        MoveTowardsTarget(speed);
    }

    //Overrides the base class method in order to damage the enemy
    protected override void HitTarget()
    {
        Destroy(gameObject);
        target.GetComponent<Enemy>().TakeDamage(damage);
    }
}
