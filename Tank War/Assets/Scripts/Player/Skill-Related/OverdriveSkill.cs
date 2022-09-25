using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverdriveSkill : EffectSkill
{
    [SerializeField]
    ShootBullet affectedAtkComp;
    [SerializeField]
    List<Skill> affectedSkillList;


    float reloadTimeReduceMultiplier = 0.4f;
    void Start()
    {
        OnStart();
    }

    public override void OnStart()
    {
        base.OnStart();
        skillType = type.NO_CURSOR;
        skillName = GlobalVar.skill.SYS_OVERDRIVE;
        allowOtherSkill = true;
        activeTime = 10f;
        cooldown = 30f;
    }

    // Update is called once per frame
    void Update()
    {
        CheckCooldown();
    }

    //any skill that activated here can be activated twice if put ActivateSkill() (mouse0 call ActivateSkill() again)
    public override void ActivateSkillCursor()
    {
        base.ActivateSkill();
        affectedAtkComp.ReduceReloadTimeBySkill(reloadTimeReduceMultiplier, activeTime);
        skillDurationEnded = true;
    }

    public override void ActivateSkill()
    {
        
    }

    public override void DeactivateSkill()
    {
        
    }
}
