using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : AutoAttack
{
    GameObject player;
    float distanceToPlayer;

    void Start()
    {
        GetNeededComponent();
        if (PreLevelDataController.instance && PreLevelDataController.instance.holdPlayer) player = PreLevelDataController.instance.player;
        else player = GameObject.FindGameObjectWithTag(GlobalVar.pTag);
    }

    void Update()
    {
        if (target) distanceToTarget = Vector2.Distance(target.transform.position, transform.position);

        if (moveComp && moveComp.enabled)
        {
            target = moveComp.target;
        }
        else SetTargetWithoutMoveComp();

        RotateTurretAtTarget();
        if (fireMode == fire.NORM) Attack();
        if (fireMode == fire.REPEAT) RepeatAttack();
        if (fireMode == fire.SALVO) SalvoAttack();
    }

    protected override void SetRandomTarget()
    {
        if ((!AllyHolder.instance || AllyHolder.instance.allyList.Count == 0) && player && player.activeInHierarchy)
        {
            target = player;
            return;
        }

        if (AllyHolder.instance.allyList.Count > 0)
        {
            int index = Random.Range(0, AllyHolder.instance.allyList.Count);
            target = AllyHolder.instance.allyList[index];
        }

        if ((!AllyHolder.instance || AllyHolder.instance.allyList.Count == 0) && !player.activeInHierarchy)
        {
            target = null;
            return;
        }
    }

    //if object is stationary -> no movement component -> set target yourself
    protected override void SetTargetWithoutMoveComp()
    {
        if (player) distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);
        if (target) distanceToTarget = Vector2.Distance(target.transform.position, transform.position);

        if (!target || (target && !target.activeInHierarchy)) { SetRandomTarget(); }
        if ((target && target.activeInHierarchy))
        {
            if (distanceToTarget > effectiveRange)
            {
                GameObject tempTarget = null;
                float minDisToThis = 999f;
                foreach (GameObject potentialTarget in AllyHolder.instance.allyList)
                {
                    float dis = Vector2.Distance(transform.position, potentialTarget.transform.position);
                    if (dis < minDisToThis)
                    {
                        tempTarget = potentialTarget;
                        minDisToThis = dis;
                    }
                }
                if (distanceToPlayer < minDisToThis && player.activeInHierarchy) { tempTarget = player; }

                target = tempTarget;
            }
            if (distanceToPlayer < distanceToTarget && target != player && player.activeInHierarchy) target = player;
        }
    }
}
