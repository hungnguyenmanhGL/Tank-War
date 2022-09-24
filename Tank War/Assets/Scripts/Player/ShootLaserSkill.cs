using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootLaserSkill : Skill
{
    [SerializeField]
    GameObject laser;
    [SerializeField]
    Transform firingPoint;
    
    float maxRange = 35f;

    void Start()
    {
        skillType = type.HAS_CURSOR;
        skillName = GlobalVar.skill.SHOOT_LASER;
        lockTurret = true;
        activeTime = 1f;
        cooldown = 5f;
    }


    // Update is called once per frame
    void Update()
    {
        CheckCooldown();

        CursorFollowTurret();
    }

    public override void ActivateSkillCursor()
    {
        if (skillCursor)
        {
            skillCursor.SetActive(true);
        }
    
        skillDurationEnded = true;
        //Debug.Log("Laser ready!");
    }
    public override void ActivateSkill()
    {
        base.ActivateSkill();
        skillCursor.SetActive(false);

        laser.transform.position = firingPoint.transform.position;
        laser.transform.rotation = firingPoint.transform.rotation;
        laser.SetActive(true);
        StartCoroutine(DisableSkillRelatedObject(laser, activeTime));
    }

    public override void UpdateSkillEffect()
    {
        
    }

    public override void CursorFollowTurret()
    {
        base.CursorFollowTurret();
        if (skillCursor && skillCursor.activeInHierarchy)
        {
            skillCursor.transform.position = firingPoint.transform.position;
            skillCursor.transform.rotation = firingPoint.transform.rotation;
        }
    }

    public override void DeactivateSkill()
    {
        skillCursor.SetActive(false);
    }
}
