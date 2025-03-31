using System;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    public WaveObject[] waves;

    public int currentWave;
    
    public float timeToNextWave;

    public bool canSpawnWaves = true;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        timeToNextWave = waves[0].timeToSpawn;
    }

    private void Update()
    {
        if (canSpawnWaves)
        {
            timeToNextWave -= Time.deltaTime;

            if (timeToNextWave <= 0)
            {
                Instantiate(waves[currentWave].theWave, transform.position, transform.rotation);

                if (currentWave < waves.Length - 1)
                {
                    currentWave++;
            
                    timeToNextWave = waves[currentWave].timeToSpawn;
                }
                else
                {
                    canSpawnWaves = false;
                }
            }
        }
    }
    
    public void ContinueSpawning()
    {
        if(currentWave <= waves.Length - 1 && timeToNextWave > 0)
        {
           canSpawnWaves = true;
        }
    
    }
}

[Serializable]
public class WaveObject
{
    public float timeToSpawn;
    public EnemyWave theWave;
}