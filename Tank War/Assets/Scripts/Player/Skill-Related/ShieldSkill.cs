using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSkill : Skill
{
    [SerializeField]
    GameObject shield;

    [SerializeField]
    Transform shieldPoint;
    
    void Start()
    {
        skillType = type.NO_CURSOR;
        skillName = GlobalVar.skill.SHIELD;
        lockTurret = false;
        allowOtherSkill = true;

        activeTime = 9f;
        cooldown = 25f;
    }

    void Update()
    {
        CheckCooldown();
        if (needUpdate) UpdateSkillEffect();
    }

    //this skill is cursorless -> activate skill when skill button/hotkey clicked right away
    public override void ActivateSkillCursor()
    {
        base.ActivateSkill();
        needUpdate = true;
        shield.SetActive(true);
        StartCoroutine(DisableSkillRelatedObject(shield, activeTime));
        skillDurationEnded = true;
    }

    public override void ActivateSkill()
    {
        
    }

    public override void UpdateSkillEffect()
    {
        shield.transform.position = shieldPoint.position;
        shield.transform.rotation = shieldPoint.rotation;
    }

    public override void DeactivateSkill()
    {
        
    }
}
