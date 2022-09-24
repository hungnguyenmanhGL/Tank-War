using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttack : MonoBehaviour
{
    public enum fire { NORM, REPEAT}
    [SerializeField]
    protected fire fireMode;

    [SerializeField]
    protected AutoMovement moveComp;

    protected bool readyToFire = true;
    protected bool turretRotatedAtTarget = false;

    protected GameObject target;
    protected float distanceToTarget;

    [SerializeField]
    protected GameObject turret;
    [SerializeField]
    protected List<Transform> firingPointList;

    [SerializeField]
    protected GameObject shellPrefab;
    [SerializeField]
    protected Bullet shellComp;

    [SerializeField]
    protected float turretRotateSpeed = 4f;
    [SerializeField]
    protected float reloadTime = 5f;
    protected float firingForce = 0.03f;

    [SerializeField]
    protected float firingRange;
    protected float effectiveRange = 20f;

    protected Coroutine currentReloadCoroutine;

    //for repeat firing
    protected const int numOfShotToRepeat = 3;
    protected int numOfShotRepeated = 0;
    protected float timeBetweenRepeat = 0.5f;

    void Start()
    {
        
    }

    protected IEnumerator SetReadyToFire(float time)
    {
        readyToFire = false;
        yield return new WaitForSeconds(time);
        readyToFire = true;
        currentReloadCoroutine = null;
    }

    virtual protected void GetNeededComponent()
    {
        if (!moveComp) moveComp = GetComponent<AutoMovement>();
        if (shellComp) firingRange = shellComp.GetMaxRange();
        else
        {
            shellComp = GetComponent<Bullet>();
            if (!shellComp)
            {
                Debug.Log("No bullet component found on this");
                firingRange = 35f;
            }
            else firingRange = shellComp.GetMaxRange();
        }
    }

    public void ResetUponKilled()
    {
        currentReloadCoroutine = null;
        target = null;
        readyToFire = true;
        turretRotatedAtTarget = false;
        numOfShotRepeated = 0;
    }

    protected void RotateTurretAtTarget()
    {
        if (target && target.activeInHierarchy)
        {
            Vector2 dirToTarget = target.transform.position - turret.transform.position;
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
            turret.transform.rotation = Quaternion.Lerp(turret.transform.rotation, transform.rotation, turretRotateSpeed * Time.deltaTime);
        }
    }

    protected void Attack()
    {
        if (!readyToFire) return;

        if (target && target.activeInHierarchy)
        {
            if (distanceToTarget <= firingRange && turretRotatedAtTarget)
            {
                foreach (Transform i in firingPointList) GlobalVar.FireShell(shellPrefab.name, i, firingForce);

                currentReloadCoroutine = StartCoroutine(SetReadyToFire(reloadTime));
            }
        }
    }
    protected void RepeatAttack()
    {
        if (!readyToFire) return;
        if (target && target.activeInHierarchy)
        {
            if (distanceToTarget <= firingRange && turretRotatedAtTarget)
            {
                foreach (Transform i in firingPointList) GlobalVar.FireShell(shellPrefab.name, i, firingForce);

                numOfShotRepeated++;
                if (numOfShotRepeated < numOfShotToRepeat)
                    currentReloadCoroutine = StartCoroutine(SetReadyToFire(timeBetweenRepeat));
                if (numOfShotRepeated == numOfShotToRepeat)
                {
                    numOfShotRepeated = 0;
                    currentReloadCoroutine = StartCoroutine(SetReadyToFire(reloadTime));
                }
            }
        }
    }

    virtual protected void SetRandomTarget()
    {

    }
    virtual protected void SetTargetWithoutMoveComp()
    {
       
    }
}
