using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTest : MonoBehaviour
{
    [SerializeField] private EnemyScriptableObject enemyScriptableObject;
    [SerializeField] private Slider healthBar;
    private int health;
    private float speed;
    private int currentWayPoint = 0;

    private Transform target;

    private bool toSlowDown = false;

    //Set the values from the scriptable object for this enemy
    void Awake()
    {
        health = enemyScriptableObject.health;
        speed = enemyScriptableObject.speed;

        healthBar.maxValue = health;
        UpdateHealthBar();
    }

    //Subscribe to the GameManager events and get the first waypoint
    private void Start()
    {
        if (Waypoints.Instance == null) return;
        target = Waypoints.Instance.points[0];
    }


    void Update()
    {
        if (Waypoints.Instance == null) return;
        Move();
    }

    //Move the enemy towards the next waypoint
    private void Move()
    {
        Vector3 direction = target.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) <= 0.1f)
        {
            SwitchToTheNextWaypoint();
        }
    }

    //Switch the waypoint if too close to the current one
    private void SwitchToTheNextWaypoint()
    {
        if (currentWayPoint >= Waypoints.Instance.points.Length - 1)
        {
            Destroy(gameObject);
            return;
        }

        currentWayPoint++;
        target = Waypoints.Instance.points[currentWayPoint];

    }

    //Method called from the SlowDownBullet class to slow down the enemy
    public void SlowDown(float _seconds, float _slowDownValue)
    {
        if (!toSlowDown)
        {
            toSlowDown = true;
            speed *= _slowDownValue;

            Invoke("SetTheOriginalSpeed", _seconds);
        }
    }

    //Set the original speed when the time of impact from the slow down bullet ends
    private void SetTheOriginalSpeed()
    {
        speed = enemyScriptableObject.speed;
        toSlowDown = false;
    }

    //Method called from the Bullet class to damage the enemy
    public void TakeDamage(int _damage)
    {
        health -= _damage;
        UpdateHealthBar();
        CheckIfOutOfHealth();
    }

    private void UpdateHealthBar()
    {
        healthBar.value = health;
    }

    private void CheckIfOutOfHealth()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

}
