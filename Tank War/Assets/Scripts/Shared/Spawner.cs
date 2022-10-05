using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base spawner class
//Spawner that spawns continuously in a cycle,  
public class Spawner : MonoBehaviour
{
    [SerializeField]
    protected List<GameObject> spawnPrefabList;
    
    protected int numOfSpawnedThisWave = 0;

    //use this if you want to random spawn
    [SerializeField]
    protected bool doRandomSpawn = false;
    protected int maxNumOfRandomSpawn = 3;
    protected int lastRandomSpawnIndex = -1;

    protected bool canStartSpawning = false;

    //if multiple spawns for a spawn wave
    [SerializeField]
    protected float timeBetweenSpawn = 3f;
    [SerializeField]
    protected float timeBetweenSpawnWave = 60f;
    protected float lastSpawnTime;

    //spawn move to this point first before doing anything else
    [SerializeField]
    protected Transform rallyPoint;

    // Use this for initialization
    void Start()
    {
        //canStartSpawning = true;
    }

    //the spawner wont spawn until you called ActivateSpawnerAfterTime in EnemySpawnerHolder after set amount of time
    private void OnEnable()
    {
        canStartSpawning = false;
    }

    protected IEnumerator BreakBeforeNextSpawn(float time)
    {
        canStartSpawning = false;
        yield return new WaitForSeconds(time);
        canStartSpawning = true;
    }

    //called this in spawn holder or controller if you wanna activate the spawner after some time
    //!DONT WORK on revive spawner
    public void ActivateSpawnerAfterTime(float time)
    {
        StartCoroutine(BreakBeforeNextSpawn(time));
    }

    public void SynchronizedSpawnTimeForSpawnGroup(float syncTimeBetweenSpawn, float syncTimeBetweenWave)
    {
        timeBetweenSpawn = syncTimeBetweenSpawn;
        timeBetweenSpawnWave = syncTimeBetweenWave;
    }

    // Update is called once per frame
    void Update()
    {
        if (doRandomSpawn)
        {
            SpawnRandom();
            return;
        }
        SpawnUnitInPrefabList();
    }

    //spawn each unit in prefab list once as a wave
    protected void SpawnUnitInPrefabList()
    {
        if (!canStartSpawning) return;

        if (numOfSpawnedThisWave == 0) lastSpawnTime = Time.time;

        GameObject unit = GlobalVar.GetObject(spawnPrefabList[numOfSpawnedThisWave].name, transform);
        AutoMovement moveComp = unit.GetComponent<AutoMovement>();
        if (moveComp)
        {
            moveComp.SetRallyDes(rallyPoint.position, false);
        }
        AddToFactionHolder(unit);
        
        numOfSpawnedThisWave++;
        if (numOfSpawnedThisWave < spawnPrefabList.Count)
        {
            StartCoroutine(BreakBeforeNextSpawn(timeBetweenSpawn));
        }
        if (numOfSpawnedThisWave == spawnPrefabList.Count)
        {
            StartCoroutine(BreakBeforeNextSpawn(timeBetweenSpawnWave));
            numOfSpawnedThisWave = 0;
        }
    }

    protected void AddToFactionHolder(GameObject g)
    {
        if (gameObject.CompareTag(GlobalVar.eTag)) EnemyPool.instance.activeEnemies.Add(g);
        if (gameObject.CompareTag(GlobalVar.allyTag)) AllyHolder.instance.allyList.Add(g);
    }

    virtual protected void ResetSpawner()
    {
        canStartSpawning = false;
    }

    virtual protected void SpawnRandom()
    {
        //if (!canStartSpawning) return;
        //if (numOfSpawnedThisWave == 0) lastSpawnTime = Time.time;

        //int i = Random.Range(0, spawnPrefabList.Count);
        //while (i == lastRandomSpawnIndex) i = Random.Range(0, spawnPrefabList.Count);
        //lastRandomSpawnIndex = i;

        //GameObject unit = GlobalVar.GetObject(spawnPrefabList[i].name, transform);
        //AutoMovement moveComp = unit.GetComponent<AutoMovement>();
        //if (moveComp)
        //{
        //    moveComp.SetRallyDes(rallyPoint.position, false);
        //}
        //AddToFactionHolder(unit);

        //numOfSpawnedThisWave++;

        //if (numOfSpawnedThisWave < spawnPrefabList.Count - 1)
        //{
        //    StartCoroutine(BreakBeforeNextSpawn(timeBetweenSpawn));
        //}
        //if (numOfSpawnedThisWave == spawnPrefabList.Count - 1)
        //{
        //    numOfSpawnedThisWave = 0;
        //    StartCoroutine(BreakBeforeNextSpawn(timeBetweenSpawnWave));
        //}
    }
}
