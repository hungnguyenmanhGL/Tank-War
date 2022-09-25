using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSkill : Skill
{
    void Start()
    {
        
    }


    public override IEnumerator checkActiveTime()
    {
        skillDurationEnded = false;
        yield return new WaitForSeconds(activeTime);
        //Debug.Log("Skill ended!");
        skillDurationEnded = true;
        needUpdate = false;
        RemoveBuffEffect();
    }

    public virtual void RemoveBuffEffect()
    {

    }
}
