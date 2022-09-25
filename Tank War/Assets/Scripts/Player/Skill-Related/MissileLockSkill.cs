using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLockSkill : Skill
{
    [SerializeField]
    LockTargetCursor lockTargetComp;
    [SerializeField]
    GameObject missilePrefab;
    [SerializeField]
    List<Transform> firingPointList;
    [SerializeField]
    int numOfShot = 4;

    GameObject currentTarget = null;
    int numOfShotFired = 0;
    float breakBetweenShot = 0.5f;
    bool fire = false;

    void Start()
    {
        skillType = type.HAS_CURSOR;
        skillName = GlobalVar.skill.MISSILE_LOCK;
        fire = true;
        ready = true;
        numOfShotFired = 0;
    }

    IEnumerator BreakBetweenShot()
    {
        fire = false;
        yield return new WaitForSeconds(breakBetweenShot);
        fire = true;
    }

    void Update()
    {
        CheckCooldown();
        if (needUpdate) UpdateSkillEffect();
        if (skillCursor && skillCursor.activeInHierarchy) { CursorFollowMouse(); }
    }

    //target acquired here
    public override bool CheckAllowToActivate()
    {
        currentTarget = lockTargetComp.ActivateTargetLock();
        if (!currentTarget) return false;
        if (!fire) { Debug.Log("!fire"); return false; }
        return true;
    }

    public override void ActivateSkillCursor()
    {
        skillCursor.SetActive(true);
        lockTargetComp.enabled = true;
    }

    public override void ActivateSkill()
    {
        ready = false;
        lastUsedTime = Time.time;
        skillCursor.SetActive(false);
        skillDurationEnded = true;
        needUpdate = true;
    }

    public override void UpdateSkillEffect()
    {
        if (!fire) return;
        GameObject missile = ObjectPool.instance.GetObjectForType(missilePrefab.name, false);
        missile.transform.position = firingPointList[numOfShotFired].position;
        missile.transform.rotation = firingPointList[numOfShotFired].rotation;
        GuidedMissile lockComp = missile.GetComponent<GuidedMissile>();
        lockComp.target = currentTarget;
        //Debug.Log(numOfShotFired);
        numOfShotFired++;
        if (numOfShotFired < numOfShot)
        {
            StartCoroutine(BreakBetweenShot());
            return;
        }
        if (numOfShotFired == numOfShot)
        {
            numOfShotFired = 0;
            currentTarget = null;
            needUpdate = false;
            return;
        }
    }

    public override void DeactivateSkill()
    {
        skillCursor.SetActive(false);
    }
}
