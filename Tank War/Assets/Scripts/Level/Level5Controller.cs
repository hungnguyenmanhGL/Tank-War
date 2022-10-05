using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level5Controller : LevelController
{
    [SerializeField]
    GameObject battleshipBoss;
    [SerializeField]
    EnemyMovement bossMoveComp;
    [SerializeField]
    EnemyAttack bossBombardComp;
    [SerializeField]
    Transform bossRallyPoint;
    [SerializeField]
    List<Collider2D> bossColliderCompList;
    [SerializeField]
    List<EnemyAttack> bossAtkCompList;
    [SerializeField]
    EnemySpawnerHolder eHolder;

    float syncSpawnerStartAt = 1f;
    float bombardStartTime = 30f;
    float deactivateSpawnerTime = 93f;
    float activateBossTime = 95f;

    bool syncSpawnerActivated = false;
    bool bombardActivated = false;
    bool syncSpawnerDeactivated = false;    
    bool bossActivated = false;

    void Start()
    {
        levelStartTime = Time.time;
        bossBombardComp.enabled = false;
        ActivateBoss(false);
    }

    //deactivate boss comps: collider, atk, move till activate time
    void ActivateBoss(bool active)
    {
        if (active)
        {
            bossMoveComp.enabled = true;
            for (int i=0; i<bossColliderCompList.Count; i++)
            {
                bossColliderCompList[i].enabled = true;
                bossAtkCompList[i].enabled = true;
            }
        }
        else
        {
            bossMoveComp.enabled = false;
            for (int i=0;i<bossAtkCompList.Count; i++)
            {
                bossColliderCompList[i].enabled = false;
                bossAtkCompList[i].enabled = false;
            }
        }
    }

    IEnumerator ActivateBossAfterCutscene(float time)
    {
        //the boss must move for cutscene
        bossMoveComp.enabled = true;
        bossMoveComp.SetRallyDes(bossRallyPoint.position, true, 10);

        //move the camera for cutscene
        CameraFollow.instance.ActivateCameraCutscene("BossIn", time);
        bossBombardComp.enabled = false;
        yield return new WaitForSeconds(time);

        //get text then activate boss
        ActivateCutsceneWithText(bossFightBeginDialog);
        ActivateBoss(true);
    }

    // Update is called once per frame
    void Update()
    {
        ActivateStartCutscene(startCutsceneDialog);
        //start spawning , should be 3 waves before boss
        if (!syncSpawnerActivated && Time.time - levelStartTime >= syncSpawnerStartAt)
        {
            syncSpawnerActivated = true;
            eHolder.enabled = true;
            eHolder.ActivateSynchronizedSpawnerList(0.5f);
        }

        if (!bombardActivated && Time.time - levelStartTime >= bombardStartTime)
        {
            bombardActivated = true;
            bossBombardComp.enabled = true;
        }

        if (!syncSpawnerDeactivated && Time.time - levelStartTime >= deactivateSpawnerTime)
        {
            eHolder.DeactivateAllSpawners();
            syncSpawnerDeactivated = true;
        }

        //when time for boss -> stop spawner -> get boss to rally point
        if (!bossActivated && Time.time - levelStartTime >= activateBossTime && EnemyPool.instance.activeEnemies.Count == 0)
        {
            bossActivated = true;
            StartCoroutine(ActivateBossAfterCutscene(10));
            if (PreLevelDataController.instance != null)
            {
                PreLevelDataController.instance.DeactivatePlayerInCutscene(10f);
            }
        }

        CheckObjectiveStatus();
    }

    static string[] startCutsceneDialog =
    {
        "Operator: A considerable number of enemy units have successfully landed on this area. " +
            "They are swarming towards your position as we speak. " +
            "To make matter worse, there are also reports of a hostile battleship bombarding our coastal defense. " +
            "Reinforcement is already dispactched but they won't make it in time for the first few waves. ",
        "Operator: Our stationed artillery is the only available resource in your proximity. " +
            "It will provide crucial tatical advantages if you are to hold your ground. " +
            "Taking cover behind its wall but the turrets should remain online as long as possible. And keep your eyes out for enemy bombardment."
            
    };

    static string[] bossFightBeginDialog =
    {
        "Operator: How can a battleship get this close to shore ? What are they trying to... ? " +
            "Doesn't matter. We aren't prepared for this at all, we gotta pull back. " +
            "No, wait. New message from HQ. ...",
        "Operator: Bad news. Our force sent to aid you arrived just to get hit by a large scale EMP. " +
            "They are right in the range of that battleship barrage. We have to do something or they are dead meat. ",
        "Operator: Your only option right now is to neutralize that battleship. " +
            "Neutralize its combat capability, I repeat , not sink it. " +
            "I know how it sounds but most of its main armaments are useless in close range. " +
            "Your universal tank cannon can still do numbers on its armor. " +
            "Your only concern would be the 4 batteries on its front deck. " +
            "Inflict enough damage to chase it away.",
    };
}
