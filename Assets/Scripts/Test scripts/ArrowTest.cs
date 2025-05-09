using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTest : Projectile
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
        target.GetComponent<EnemyTest>().SlowDown(affectionTime, slowDownValue);
    }
}
