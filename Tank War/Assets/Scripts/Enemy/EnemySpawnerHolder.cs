using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemySpawnerHolder : MonoBehaviour
{
    public static EnemySpawnerHolder instance;

    [SerializeField]
    List<Spawner> syncSpawnerList;

    [SerializeField]
    List<Spawner> asyncSpawnerList;

    [SerializeField]
    Text nextWaveWarning;

    float syncSpawnerLastSpawnTime;

    [SerializeField]
    float timeBetweenSpawn = 1f;
    [SerializeField]
    float timeBetweenSpawnWave = 60f;

    // Use this for initialization
    void Start()
    {
        OnStart();
    }

    void OnStart()
    {
        foreach (Spawner spawner in syncSpawnerList)
        {
            spawner.enabled = false;
            spawner.SynchronizedSpawnTimeForSpawnGroup(timeBetweenSpawn, timeBetweenSpawnWave);
        }

        foreach(Spawner respawner in asyncSpawnerList)
        {
            respawner.enabled = false;
        }
    }

    public void ActivateSynchronizedSpawnerList(float waitTime)
    {
        foreach(Spawner spawner in syncSpawnerList)
        {
            spawner.enabled = true;
            spawner.ActivateSpawnerAfterTime(waitTime);
        }
    }

    public void ActivateAsyncSpawnerList()
    {
        foreach(Spawner respawner in asyncSpawnerList)
        {
            respawner.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
