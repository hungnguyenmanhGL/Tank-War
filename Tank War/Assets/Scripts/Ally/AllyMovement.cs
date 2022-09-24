using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyMovement : AutoMovement
{
    public float maxDistanceToPlayer = 8f;

    [SerializeField]
    private Rigidbody2D body;

    void Start()
    {
        if (!body) body = gameObject.GetComponent<Rigidbody2D>();
        FindPlayer();
        SetRallyLayerByTag();
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyPool.instance.activeEnemies.Count == 0) noTargetLeft = true;
        else noTargetLeft = false;
        if (!noTargetLeft) { SetTargetToEngage(); }

        if (target) distanceToTarget = Vector2.Distance(transform.position, target.transform.position);
        if (player) distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);

        Debug.DrawLine(transform.position, target.transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GlobalVar.obsBlockShellTag))
        {
            reachedDes = true;
            collisionStartTime = Time.time;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GlobalVar.obsBlockShellTag))
        {
            //if collision stays too long
            collisionDuration = Time.time - collisionStartTime;
            if (collisionDuration > lastPosCachedDuration)
            {
                SetNewDesManually(new Vector3(-destination.x, -destination.y, 0));
            }
        }
    }

    private void FixedUpdate()
    {
        RotateTowardDes();

        if (!canMove) return;

        if (currentAction == action.RALLY)
        {
            CheckReachedDes();
            MoveToDes();
            return;
        }

        if (noTargetLeft && distanceToPlayer > maxDistanceToPlayer)
        {
            CheckReachedDes();
            SetNewDesNearPlayer();
            MoveToDes();
        } 

        if (!noTargetLeft)
        {
            if (distanceToTarget > engageRange && reachedDes)
            {
                CheckReachedDes();
                SetNewEngageDes();
                MoveToDes();
            }
            if (distanceToTarget > engageRange && !reachedDes)
            {
                CheckReachedDes();
                MoveToDes();
            }
            if (distanceToTarget <= engageRange)
            {
                CheckReachedDes();
                SetNewRandomDes();
                MoveToDes();
            }
        }
    }

    protected override void SetRandomTargetToEngage()
    {
        if (EnemyPool.instance.activeEnemies.Count > 0)
        {
            int index = Random.Range(0, EnemyPool.instance.activeEnemies.Count);
            target = EnemyPool.instance.activeEnemies[index];
        }

        if (EnemyPool.instance.activeEnemies.Count == 0)
        {
            target = null;
            noTargetLeft = true;
            return;
        }
    }

    protected override void SetTargetToEngage()
    {
        if (!target || (target && !target.activeInHierarchy)) { SetRandomTargetToEngage(); }
        if (target && target.activeInHierarchy)
        {
            if (distanceToTarget > effectiveEngageRange)
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

    protected void SetNewDesNearPlayer()
    {
        if (reachedDes)
        {
            float randX = Random.Range(minRandomDistance, maxRandomDistance);
            float randY = Random.Range(minRandomDistance, maxRandomDistance);
            float dirX = Random.Range(-1f, 1f);
            float dirY = Random.Range(-1f, 1f);
            if (dirX <= 0) randX = -randX;
            if (dirY <= 0) randY = -randY;

            destination = new Vector3(player.transform.position.x + randX, player.transform.position.y + randY, 0);
            reachedDes = false;
            bodyRotated = false;
        }
    }

    protected override void CheckReachedDes()
    {
        base.CheckReachedDes();
    }
}
