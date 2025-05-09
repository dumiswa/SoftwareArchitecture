using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class for bullet types
public class Projectile : MonoBehaviour
{

    protected Transform target;
    protected int damage;

    public void SetTheValues(Transform _target, int _damage)
    {
        target = _target;
        damage = _damage;
    }

    //This method is called in subclasses to make them move towards the target
    protected void MoveTowardsTarget(float _speed)
    {
        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = _speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        // Calculate rotation to look at the target direction
        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);

        // Move the projectile
        transform.Translate(direction.normalized * distanceThisFrame, Space.World);

        // Set rotation to face the target direction
        transform.rotation = targetRotation;

    }

    //This method is overriden in subclasses to do their own logic
    protected virtual void HitTarget()
    {

    }
}
