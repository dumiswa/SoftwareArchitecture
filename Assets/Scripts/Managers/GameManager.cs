using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Class that handles the money amount, the game speed, and the state of the game(Won/Lost)
//It also notifies its observers about the game state(Won/Lost)
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private float timeScaleMultiplier = 2f;

    public Action onGameWon;
    public Action onGameLost;

    public bool isGameSpedUp = false;
    public int totalMoney;
    public int amountOfEnemiesThatCanEnter;
    private int amountOfEnemiesThatHaveEntered = 0;

    //This class is used as a singleton in order for other classes to be able to reach this class
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    //This method is called form the Enemy class whenever the enemy has entered the final point
    //This method is responsible for the Loss condition
    public void OnEnemyEnter()
    {
        amountOfEnemiesThatHaveEntered++;
        UIEventHandler.Instance.UpdateEnteredEnemiesText(amountOfEnemiesThatHaveEntered, amountOfEnemiesThatCanEnter);

        if (amountOfEnemiesThatHaveEntered == amountOfEnemiesThatCanEnter)
        {
            OnGameLost();
        }
    }

    //This method is called form the Banknote class to increase the money amount
    public void OnMoneyIncrease(int _moneyAmount)
    {
        totalMoney += _moneyAmount;
        UIEventHandler.Instance.UpdateMoneyText();
    }

    //This method restarts the game
    public void RestartTheGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    //This method either speeds up the game or brings its speed value to 1
    public void SetTheSpeed()
    {
        isGameSpedUp = !isGameSpedUp;
        Time.timeScale = isGameSpedUp ? timeScaleMultiplier : 1;
    }

    //Called when the game is lost to notify others classes
    private void OnGameLost()
    {
        onGameLost?.Invoke();
    }

    //Called when the game is won to notify others classes
    public void OnGameWon()
    {
        onGameWon?.Invoke();
    }

}
