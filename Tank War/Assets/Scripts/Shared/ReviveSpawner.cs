using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Spawner that only starting cooldown after its ONLY spawn is killed, use for infinitely repeated enemy spawn
//DO NOT use this spawner for sync spawn group
public class ReviveSpawner : Spawner
{
    GameObject spawnToCheck;
    AutoMovement moveComp;

    bool needToRespawn = false;
    
    void Start()
    {
        doRandomSpawn = false;
        //timeBetweenSpawnWave = 90f;
        canStartSpawning = false;
        needToRespawn = false;

        //GetUnitFromPool();
    }

    //for some reason, this started faster than object pool start -> no object init -> cant get -> temp fix: put in update
    void GetUnitFromPool()
    {
        if (!spawnToCheck)
        {
            GameObject unit = ObjectPool.instance.GetObjectPermanently(spawnPrefabList[0].name);
            spawnToCheck = unit;
            moveComp = unit.GetComponent<AutoMovement>();
            if (moveComp) SetRallyPointForSpawn();
            unit.transform.position = transform.position;
            AddToFactionHolder(unit);
        }
    }
    
    void Update()
    {
        GetUnitFromPool();
        CheckIfNeedToRespawn();
        StartCountingDownToRespawn();
    }

    void CheckIfNeedToRespawn()
    {
        if (spawnToCheck && !spawnToCheck.activeInHierarchy && !needToRespawn)
        {
            //Debug.Log("Start spawning");
            needToRespawn = true;
            canStartSpawning = true;
        }
    }

    void StartCountingDownToRespawn()
    {
        if (canStartSpawning) { 
            lastSpawnTime = Time.time;
            canStartSpawning = false;
        }

        if (needToRespawn && Time.time - lastSpawnTime >= timeBetweenSpawnWave)
        {
            spawnToCheck.transform.position = transform.position;
            SetRallyPointForSpawn();
            spawnToCheck.SetActive(true);
            AddToFactionHolder(spawnToCheck);
            needToRespawn = false;
        }
    }

    void SetRallyPointForSpawn()
    {
        moveComp.enabled = true;
        moveComp.SetRallyDes(rallyPoint.position, true);
    }
}
