using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManagerTest : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform[] enemyPrefabs;

    [SerializeField] private int amountOfEnemiesFirstWave;

    [SerializeField] private float delayBetweenEachEnemy;

    private Transform[] spawnedEnemies;

    private bool startWaveSpawning = false;

    //Subscribing to the GameManager actions
    void Start()
    {
    }


    void Update()
    {
        StartWaveSpawning();
    }

    //This method handles the amount of enemies to be sapwned during the current wave
    //Starts the coroutine that spawns enemies
    private void StartWaveSpawning()
    {
        if (!startWaveSpawning)
        {
            startWaveSpawning = true;
            int amountOfEnemiesToBeSpawned = amountOfEnemiesFirstWave;
            spawnedEnemies = new Transform[amountOfEnemiesToBeSpawned];
            StartCoroutine(SpawnWave(amountOfEnemiesToBeSpawned));
        }
    }

    //Spawns enemies every "delayBetweenEachEnemy" seconds
    IEnumerator SpawnWave(int _amountOfEnemies)
    {
        for (int i = 0; i < _amountOfEnemies; i++)
        {
            SpawnEnemy(i);
            yield return new WaitForSeconds(delayBetweenEachEnemy);
        }
    }

    //Instantiate on of the enemy prefabs
    private void SpawnEnemy(int _index)
    {
        int whichPrefabToSpawn = Random.Range(0, enemyPrefabs.Length);
        spawnedEnemies[_index] = Instantiate(enemyPrefabs[whichPrefabToSpawn], spawnPoint.transform.position, Quaternion.identity);
    }

}
