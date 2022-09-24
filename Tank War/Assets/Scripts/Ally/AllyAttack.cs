using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyAttack : AutoAttack
{
    void Start()
    {
        GetNeededComponent();
    }

    void Update()
    {
        if (target) distanceToTarget = Vector2.Distance(target.transform.position, transform.position);

        if (moveComp && moveComp.enabled) { target = moveComp.target; }
        else SetTargetWithoutMoveComp();

        RotateTurretAtTarget();
        if (fireMode == fire.NORM) Attack();
        if (fireMode == fire.REPEAT) RepeatAttack();
    }

    protected override void SetRandomTarget()
    {
        if (EnemyPool.instance.activeEnemies.Count > 0)
        {
            int index = Random.Range(0, EnemyPool.instance.activeEnemies.Count);
            target = EnemyPool.instance.activeEnemies[index];
        }

        if (EnemyPool.instance.activeEnemies.Count == 0)
        {
            target = null;
            return;
        }
    }

    //if object is stationary -> no movement component -> set target yourself
    protected override void SetTargetWithoutMoveComp()
    {
        Debug.Log("Self Target log");
        if (target) distanceToTarget = Vector2.Distance(target.transform.position, transform.position);

        if (!target || (target && !target.activeInHierarchy)) { SetRandomTarget(); }
        if ((target && target.activeInHierarchy))
        {
            if (distanceToTarget > effectiveRange)
            {
                GameObject tempTarget = null;
                float minDisToThis = 999f;
                foreach (GameObject potentialTarget in EnemyPool.instance.activeEnemies)
                {
                    float dis = Vector2.Distance(transform.position, potentialTarget.transform.position);
                    if (dis < minDisToThis)
                    {
                        tempTarget = potentialTarget;
                        minDisToThis = dis;
                    }
                }
                target = tempTarget;
            }
        }
    }
}
