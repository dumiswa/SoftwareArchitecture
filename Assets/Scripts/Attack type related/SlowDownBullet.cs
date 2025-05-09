using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SlowDownBullet class to move the bullet prefab and slow down the enemy, extends base class "Projectile"
public class SlowDownBullet : Projectile
{
    [SerializeField] private float speed;
    [SerializeField] private float affectionTime;
    [SerializeField] private float slowDownValue;

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        MoveTowardsTarget(speed);
    }

    //Overrides the base class method in order to slow down the enemy
    protected override void HitTarget()
    {
        Destroy(gameObject);
        target.GetComponent<Enemy>().SlowDown(affectionTime, slowDownValue);
    }
}
