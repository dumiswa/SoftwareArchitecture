using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Enemy class that is attached to both enemy types
public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyScriptableObject enemyScriptableObject;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Transform banknotePrefab;
    private int health;
    private float speed;
    private int carriedMoney;
    private int currentWayPoint = 0;

    private Transform target;

    private bool toSlowDown = false;
    private bool toStop = false;

    //Set the values from the scriptable object for this enemy
    void Awake()
    {
        health = enemyScriptableObject.health;
        speed = enemyScriptableObject.speed;
        carriedMoney = enemyScriptableObject.carriedMoney;

        healthBar.maxValue = health;
        UpdateHealthBar();
    }

    //Subscribe to the GameManager events and get the first waypoint
    private void Start()
    {
        target = Waypoints.Instance.points[0];

        GameManager.Instance.onGameWon += ToStopTheEnemy;
        GameManager.Instance.onGameLost += ToStopTheEnemy;
    }

    //Unsubscribe from the GameManager events when destroyed
    private void OnDisable()
    {
        GameManager.Instance.onGameWon -= ToStopTheEnemy;
        GameManager.Instance.onGameLost -= ToStopTheEnemy;
    }

    void Update()
    {
        if (toStop) return;
        Move();
    }

    private void ToStopTheEnemy()
    {
        toStop = true;
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
            GameManager.Instance.OnEnemyEnter();
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
            SetTheBanknote();
        }
    }

    //This method instantiates the banknote at the point where the enemy died.
    //Then it transfroms this position to position applicable for the canvas.
    //Then it sets banknote as a child of the needed canvas
    private void SetTheBanknote()
    {
        Transform banknoteInstance = Instantiate(banknotePrefab);
        banknoteInstance.GetChild(0).GetComponent<TextMeshProUGUI>().text = "$" + carriedMoney;
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(transform.position);
        Canvas canvas = GameObject.FindGameObjectWithTag("Info canvas").GetComponent<Canvas>();
        
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        Vector2 canvasPos = new Vector2(
            (viewportPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f),
            (viewportPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)
        );

        banknoteInstance.GetComponent<RectTransform>().anchoredPosition = canvasPos;
        banknoteInstance.GetComponent<Banknote>().moneyAmount = carriedMoney;
        banknoteInstance.transform.SetParent(GameObject.FindGameObjectWithTag("Info canvas").transform, false);
    }

}
