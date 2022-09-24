using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryTurret : MonoBehaviour
{
    public enum Fire { NORM, REPEAT };
    [SerializeField]
    protected Fire fireMode = Fire.REPEAT;

    GameObject player;
    [SerializeField]
    protected GameObject turret;

    [SerializeField]
    protected GameObject shellPrefab;

    [SerializeField]
    protected List<Transform> firingPoints;

    protected bool readyToFire = true;
    protected bool turretRotatedAtTarget = false;

    [SerializeField]
    protected int numOfShotToRepeat = 3;
    protected int shotRepeated = 0;
    protected float timeBetweenRepeatedShot = 0.5f;

    protected float turretRotateSpeed = 6f;
    [SerializeField]
    protected float range = 30f;
    protected float firingForce = 0.03f;
    [SerializeField]
    protected float reloadTime = 6f;
    protected float distanceToTarget;
    protected float distanceToPlayer;
    //if dis to player < this -> switch to player
    protected float distanceToSwitchTargetToPlayer = 10f;

    protected List<GameObject> targetList = new List<GameObject>();
    protected List<GameObject> targetInRange = new List<GameObject>();
    protected GameObject target;


    void Start()
    {
        SetTargetList();
    }

    protected void SetTargetList()
    {
        player = GameObject.FindGameObjectWithTag(GlobalVar.pTag);
        if (gameObject.CompareTag(GlobalVar.eTag))
        {
            targetList = AllyHolder.instance.allyList;
        }
        if (gameObject.CompareTag(GlobalVar.allyTag))
        {
            targetList = EnemyPool.instance.activeEnemies;
        }
    }

    protected IEnumerator SetReadyToFire(float time)
    {
        readyToFire = false;
        yield return new WaitForSeconds(time);
        readyToFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        SetTarget();
        RotateTurretAtTarget();
        if (fireMode == Fire.NORM) { Attack(); }
        if (fireMode == Fire.REPEAT) { RepeatAttack(); }
        
    }

    virtual protected void SetTarget()
    {
        distanceToPlayer = Vector2.Distance(player.transform.position, transform.position);
        if (target) distanceToTarget = Vector2.Distance(target.transform.position, transform.position);

        //if enemy <= range -> add to target list
        for (int i=0; i<targetList.Count; i++)
        {
            if (Vector2.Distance(targetList[i].transform.position, transform.position) <= range) targetInRange.Add(targetList[i]);
        }
        if (distanceToPlayer <= range) targetInRange.Add(player);

        //remove destroyed and out of range target
        for (int i=0; i<targetInRange.Count; i++)
        {
            if (!target || !targetList[i].activeInHierarchy 
                || Vector2.Distance(targetInRange[i].transform.position, transform.position) > range) targetInRange.RemoveAt(i);
        }

        if (!target || (target && !target.activeInHierarchy) || (target && distanceToTarget > range)) 
        {
            if (targetInRange.Count > 0)
            {
                int index = Random.Range(0, targetInRange.Count - 1);
                target = targetInRange[index];
            }
            else target = null;
        }
        //if (distanceToTarget <= range)
        //{
        //    if (distanceToPlayer <= distanceToSwitchTargetToPlayer && player.activeInHierarchy) target = player;
        //}
    }

    virtual protected void Attack()//and rotate turret accordingly
    {
        if (!readyToFire) return;
        if (target && target.activeInHierarchy)
        {
            if (readyToFire)
            {
                foreach(Transform firingPoint in firingPoints)
                {
                    FireShell(firingPoint);
                }
                StartCoroutine(SetReadyToFire(reloadTime));
            }
        }
    }

    virtual protected void RepeatAttack()
    {
        if (!readyToFire) return;

        if (target && target.activeInHierarchy)
        {
            foreach (Transform i in firingPoints)
            {
                FireShell(i);
            }
            //number of shell in a repeat attack
            shotRepeated++;
            if (shotRepeated < numOfShotToRepeat)
            {
                StartCoroutine(SetReadyToFire(timeBetweenRepeatedShot));
            }
            if (shotRepeated == numOfShotToRepeat)
            {
                shotRepeated = 0;
                StartCoroutine(SetReadyToFire(reloadTime));
            }
        }
    }

    protected void FireShell(Transform firingPoint)
    {
        GlobalVar.GetFiringFlash(firingPoint);

        GameObject shell = GlobalVar.GetObject(shellPrefab.name, firingPoint);
        Rigidbody2D shellBody = shell.GetComponent<Rigidbody2D>();
        shellBody.AddForce(firingPoint.up * firingForce, ForceMode2D.Impulse);
    }

    virtual protected void RotateTurretAtTarget()
    {
        if (target && target.activeInHierarchy)
        {
            Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(dirToTarget.y, dirToTarget.x) * Mathf.Rad2Deg - 90;
            if (Quaternion.Angle(Quaternion.Euler(0, 0, angle), turret.transform.rotation) <= 0.5f)
            {
                turretRotatedAtTarget = true;
            }
            else
            {
                turretRotatedAtTarget = false;
                Quaternion rotateTurretTo = Quaternion.Euler(0, 0, angle);
                turret.transform.rotation = Quaternion.Lerp(turret.transform.rotation, rotateTurretTo, turretRotateSpeed * Time.deltaTime);
            }
        }
        
        if (!target || !target.activeInHierarchy)
        {
            
        }
    }
}
