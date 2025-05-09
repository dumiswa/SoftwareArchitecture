using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTest : Projectile
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
        target.GetComponent<EnemyTest>().TakeDamage(damage);
    }
}
