using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : AutoMovement
{
    private bool focusOnPlayer = false;

    [SerializeField]
    private Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        FindPlayer();
        if (!player)
        {
            Debug.Log("No Player found");
        }
        SetRallyLayerByTag();
        //setRandomTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (AllyHolder.instance.allyList.Count == 0 && !player.activeInHierarchy) noTargetLeft = true;
        else noTargetLeft = false;
        if (!noTargetLeft) { SetTargetToEngage(); }

        if (target) distanceToTarget = Vector2.Distance(transform.position, target.transform.position);
        if (player) distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);

    }

    protected override void CheckReachedDes()
    {
        base.CheckReachedDes();
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

        //if distance to target > engaging distance && reached randomized des -> chase target
        if (distanceToTarget > engageRange && reachedDes)
        {
            CheckReachedDes();
            SetNewEngageDes();
            MoveToDes();
        }
        //if dis > engaging but randomized des not reached -> finish before chase target
        if (distanceToTarget > engageRange && !reachedDes)
        {
            CheckReachedDes();
            MoveToDes();
        }
        // if dis < engaging -> random new des and move to it
        if (distanceToTarget <= engageRange)
        {
            CheckReachedDes();
            SetNewRandomDes();
            MoveToDes();
        }
        //Debug.DrawLine(transform.position, setDestination);
        //the object can be pushed due to combined force of multiple bullet
        StopVelocityMovement();
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

    //if player has 0 allies -> target = player
    //if player has allies -> set target based on distance. if distance > engage range
    protected override void SetRandomTargetToEngage()
    {
        if (!focusOnPlayer)
        {
            if (AllyHolder.instance.allyList.Count == 0) target = player;

        }

        if (!AllyHolder.instance || AllyHolder.instance.allyList.Count == 0 || focusOnPlayer)
        {
            if (player && player.activeInHierarchy)
            {
                target = player;
                return;
            }
        }

        if (AllyHolder.instance.allyList.Count == 0 && player && player.activeInHierarchy) target = player;
        else if (AllyHolder.instance.allyList.Count > 0 && !focusOnPlayer)
        {
            int index = Random.Range(0, AllyHolder.instance.allyList.Count);
            target = AllyHolder.instance.allyList[index];
        }

        if (AllyHolder.instance.allyList.Count == 0 && !player.activeInHierarchy)
        {
            target = null;
            noTargetLeft = true;
            return;
        }
    }

    protected override void SetTargetToEngage()
    {
        if (!target || (target && !target.activeInHierarchy)) { SetRandomTargetToEngage(); }
        if ((target && target.activeInHierarchy))
        {
            if (distanceToTarget > effectiveEngageRange)
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
                if (distanceToPlayer < minDisToThis) { tempTarget = player; }//Debug.Log("Switch to player"); }

                target = tempTarget;
            }
            if (distanceToPlayer < distanceToTarget && target != player) target = player;
        }
    }

    void StopVelocityMovement()
    {
        if (body)
        {
            body.velocity = Vector2.zero;
            body.angularVelocity = 0;
        }
    }
}
