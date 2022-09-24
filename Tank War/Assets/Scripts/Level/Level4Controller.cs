using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4Controller : LevelController
{
    [SerializeField]
    EnemySpawnerHolder eHolder;
    [SerializeField]
    EnemySpawnerHolder aHolder;

    [SerializeField]
    float syncSpawnerStartAt = 5f;
    [SerializeField]
    float asyncSpawnerStartAt = 10f;

    float allyLastSpawnAt;
    float allySpawnBreak = 60f;

    bool syncSpawnerActivated = false;
    bool asyncSpawnerActivated = false;

    static string[] startCutsceneDialog =
    {
        "HQ: The enemy regrouped faster than we thought. " +
            "They even managed to set up a considerable defense preventing us from entering their main command center. " +
            "As this is the only way, we have no other option. " +
            "Your primary targets will be to neutralize 6 defense turret holding us down this road. " +
            "Keep in mind they are holding a choke point, so advance with discreet. ",
        "Operator: HQ wants you to spearhead our assasult using our new tank prototype. " +
            "Reinforcement will be sent to support you, but the enemy will likely do the same so prepare for a tug-of-war situation. " +
            "Focus on the enemy turrets while our force distract enemy fire.",
        "Operator: Your new tank is designed for this kind of demolition. While losing access to shell changing mechanism, it packs more than " +
            "enough destructive firepower to compensate. Its auxilary weapon systems matches an artillery barrage, but require time to cooldown after use. " +
            "Same goes for the EMP-infused grenade launcher." +
            "That'll be all."
    };

    static string[] asyncSpawnActivatedCutsceneDialog =
    {
        "Operator: The enemy are sending in more than their armored force. " +
            "HQ confirmed that they are bringing in naval destroyers and modified trains, coming in from 2 sides. " +
            "Watch your flanks."
    };

    bool allyIncomingTextSet = false;
    static string allyIncomingText = "Operator: Reinforcement incoming";

    void Start()
    {
        levelStartTime = Time.time;
        allyLastSpawnAt = syncSpawnerStartAt;
    }

    // Update is called once per frame
    void Update()
    {
        ActivateStartCutscene(startCutsceneDialog);
        CheckToActivateEvent();

        if (Time.time - allyLastSpawnAt >= allySpawnBreak - 3f)
        {
            allyLastSpawnAt = Time.time;
            if (!allyIncomingTextSet)
            {
                allyIncomingTextSet = true;
                UIController.instance.SetIngameDialog(allyIncomingText, 3f);
            }
            else UIController.instance.SetIngameDialog(null, 3f);
        }
    }

    void StartSpawningSyncWave()
    {
        eHolder.enabled = true;
        aHolder.enabled = true;

        eHolder.ActivateSynchronizedSpawnerList(0);
        aHolder.ActivateSynchronizedSpawnerList(0);
    }

    void StartSpawningAsyncWave()
    {
        eHolder.ActivateAsyncSpawnerList();
    }

    void CheckToActivateEvent()
    {
        if (!syncSpawnerActivated && Time.time - levelStartTime >= syncSpawnerStartAt)
        {
            syncSpawnerActivated = true;
            StartSpawningSyncWave();
        }

        if (!asyncSpawnerActivated && Time.time - levelStartTime >= asyncSpawnerStartAt)
        {
            asyncSpawnerActivated = true;
            StartSpawningAsyncWave();
            ActivateCutsceneWithText(asyncSpawnActivatedCutsceneDialog);
            this.enabled = false;
        }
    }

    
}
