using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is used to control the wave functionality
public class WaveManager : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform[] enemyPrefabs;

    [SerializeField] private int waveAmount;

    [Range(1, 10)]
    [SerializeField] private int amountOfEnemiesAddedEachWave;

    [SerializeField] private int amountOfEnemiesFirstWave;

    [Range(10, 100)]
    [SerializeField] private int amountOfSecondsBetweenWaves;

    [SerializeField] private float delayBetweenEachEnemy;

    private float timer;
    private Transform[] spawnedEnemies;

    private bool timeIsRunning;
    private bool startWaveSpawning = false;
    private bool waveHasBeenSpawned = false;
    private bool toStopSpawning = false;

    private int currentWaveIndex = 0;

    //Subscribing to the GameManager actions
    void Start()
    {
        timer = amountOfSecondsBetweenWaves + 0.5f;
        timeIsRunning = true;

        SetTheWaveNumberText();

        GameManager.Instance.onGameWon += ToStopSpawning;
        GameManager.Instance.onGameLost += ToStopSpawning;
    }

    //Unsubscribing from the GameManager actions
    private void OnDisable()
    {
        GameManager.Instance.onGameWon -= ToStopSpawning;
        GameManager.Instance.onGameLost -= ToStopSpawning;
    }

    void Update()
    {
        if (toStopSpawning) { return; }

        if (timeIsRunning)
        {
            SetTheTimerText("Time until next wave: " + Mathf.Round(timer).ToString() + "s", false);

            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                OnTimerRunOut();
            }
        }
        else
        {
            StartWaveFunctionality();
        }
    }

    //Stop spawning the enemies
    private void ToStopSpawning()
    {
        toStopSpawning = true;
    }

    private void StartWaveFunctionality()
    {
        StartWaveSpawning();
        CheckIfEnemiesAreDestroyed();
    }

    //This method handles the amount of enemies to be sapwned during the current wave
    //Starts the coroutine that spawns enemies
    private void StartWaveSpawning()
    {
        if (!startWaveSpawning)
        {
            startWaveSpawning = true;
            int amountOfEnemiesToBeSpawned = amountOfEnemiesFirstWave + currentWaveIndex * amountOfEnemiesAddedEachWave;
            spawnedEnemies = new Transform[amountOfEnemiesToBeSpawned];
            SetTheTimerText("The wave is coming!", true);
            StartCoroutine(SpawnWave(amountOfEnemiesToBeSpawned));
        }
    }

    //Spawns enemies every "delayBetweenEachEnemy" seconds
    IEnumerator SpawnWave(int _amountOfEnemies)
    {
        for (int i = 0; i < _amountOfEnemies; i++)
        {
            if (!toStopSpawning)
            {
                SpawnEnemy(i);
                yield return new WaitForSeconds(delayBetweenEachEnemy);
            }
        }
        waveHasBeenSpawned = true;
    }

    //Instantiate on of the enemy prefabs
    private void SpawnEnemy(int _index)
    {
        int whichPrefabToSpawn = Random.Range(0, enemyPrefabs.Length);
        spawnedEnemies[_index] = Instantiate(enemyPrefabs[whichPrefabToSpawn], spawnPoint.transform.position, Quaternion.identity);
    }

    //Check if there're no enemies in the game field
    private void CheckIfEnemiesAreDestroyed()
    {
        if (waveHasBeenSpawned)
        {
            int amountOfNullReferences = 0;

            for (int i = 0; i < spawnedEnemies.Length; i++)
            {
                if (spawnedEnemies[i] == null)
                {
                    amountOfNullReferences++;
                }
            }

            if (amountOfNullReferences == spawnedEnemies.Length)
            {
                if (currentWaveIndex == waveAmount - 1)
                {
                    GameManager.Instance.OnGameWon();
                }
                else
                {
                    StartNewWave();
                }
            }
        }
    }

    //Start new wave
    private void StartNewWave()
    {
        currentWaveIndex++;
        SetTheWaveNumberText();

        timer = amountOfSecondsBetweenWaves + 0.5f;
        timeIsRunning = true;

        startWaveSpawning = false;
        waveHasBeenSpawned = false;
    }

    private void OnTimerRunOut()
    {
        timeIsRunning = false;
    }

    private void SetTheTimerText(string _text, bool _isWaveComing)
    {
        UIEventHandler.Instance.OnTimerSet(_text, _isWaveComing);
    }

    private void SetTheWaveNumberText()
    {
        UIEventHandler.Instance.OnWaveChanged(currentWaveIndex + 1, waveAmount);
    }


}
