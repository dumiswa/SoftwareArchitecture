using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class implemented to move the banlnote prefab to the RectTransform that contains money UI
public class Banknote : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 5f;
    [HideInInspector] public int moneyAmount;
    private RectTransform targetRectTransform;
    private bool isReadyToGo = false;

    void Awake()
    {
        targetRectTransform = GameObject.FindGameObjectWithTag("Money text").GetComponent<RectTransform>();
        Invoke("ReadyToGo", 0.8f);
    }

    void Update()
    {
        if (!isReadyToGo) return;

        if (targetRectTransform != null)
        {
            Vector3 targetPosition = targetRectTransform.position;
            Vector3 currentPosition = transform.position;

            transform.position = Vector3.Lerp(currentPosition, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(currentPosition, targetPosition) <= 0.05f)
            {
                ReachedTheEnd();
            }
        }
    }

    //Increase the total amount of money when reached the RectTransform that contains money UI
    private void ReachedTheEnd()
    {
        GameManager.Instance.OnMoneyIncrease(moneyAmount);
        Destroy(gameObject);
    }

    private void ReadyToGo()
    {
        isReadyToGo = true;
    }
}
