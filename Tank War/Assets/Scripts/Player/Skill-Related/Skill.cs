using System.Collections;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public enum type { HAS_CURSOR, NO_CURSOR};
    public type skillType;
    //use to set text bubble matching text
    public GlobalVar.skill skillName;

    public GameObject skillCursor;
    
    //this is a property, not status variable -> DO NOT CHANGE IN MIDDLE OF SKILL
    [HideInInspector]
    public bool lockTurret = false;
    //has not been used yet
    [HideInInspector]
    public bool allowOtherSkill = false;
    //cooldown check done -> this == true
    [HideInInspector]
    public bool ready = true;
    //this == true to allow to switch to other skill after one is activated
    [HideInInspector]
    public bool skillDurationEnded = true;
    //if a skill need update for its effects, change this to true then revert when skill done
    [HideInInspector]
    public bool needUpdate = false;

    [HideInInspector]
    public float activeTime;
    //[HideInInspector]
    public float cooldown;

    protected float lastUsedTime;
    [HideInInspector]
    public float timeTillReady;
    
    void Start()
    {

    }

    public virtual void OnStart()
    {
        ready = true;
        skillDurationEnded = true;
        needUpdate = false;
    }

    //called before every skill activation to check if skill effect ended or not
    public virtual IEnumerator checkActiveTime()
    {
        skillDurationEnded = false;
        yield return new WaitForSeconds(activeTime);
        //Debug.Log("Skill ended!");
        skillDurationEnded = true;
        needUpdate = false;
    }

    public IEnumerator DisableSkillRelatedObject(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }

    public void CheckCooldown()
    {
        if (!ready)
        {
            timeTillReady = cooldown - (Time.time - lastUsedTime);
            if (timeTillReady <= 0f) ready = true;
            //else { Debug.Log("Time till ready:" + timeTillReady); }
        }
    }

    //this is check before SuperTankSkillController activate,
    //override in skill that require more than player input(like target lock,...)
    virtual public bool CheckAllowToActivate()
    {
        return true;
    }

    virtual public void ActivateSkillCursor() { }

    public void CursorFollowMouse()
    {
        if (skillCursor && skillCursor.activeInHierarchy)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 pos = Camera.main.ScreenToWorldPoint(mousePos);
            pos.z = 0;
            skillCursor.transform.position = pos;
        }
    }
    virtual public void CursorFollowTurret()
    {
     
    }

    virtual public void ActivateSkill() 
    {
        ready = false;
        lastUsedTime = Time.time;
        StartCoroutine(checkActiveTime());
    }

    virtual public void UpdateSkillEffect() { }

    virtual public void DeactivateSkill() { }
}
