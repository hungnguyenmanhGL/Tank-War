using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//handle skill activation logic
public class SuperTankSkillController : MonoBehaviour
{
    [SerializeField]
    GameObject turret;
    [SerializeField]
    Turret turretComp;
    [SerializeField]
    ShootBullet atkComp;
    Quaternion turretRotationAtSkill;

    public List<Skill> skills;
    Skill currentActiveSkill = null;

    bool turretLock = false;
    bool waitingToActivate = false;
    bool skillOnGoing = false;
    [HideInInspector]
    public int skillNum = -1;

    public bool testing = false;

    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!skillOnGoing)
        {
            //for test purpose, testing==true, moved this to SkillUIController class
            if (testing)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    SetSkillReady(1);
                }
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    SetSkillReady(0);
                }
            }

            if (Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject() && waitingToActivate)
            {
                ActivateSkill();
            }
        }

        if (skillOnGoing)
        {
            CheckCurrentActiveSkill();
            //if (currentActiveSkill) currentActiveSkill.UpdateSkillEffect();
        }
    }

    private void LateUpdate()
    {
        if (turretLock)
        {
            GetDirectionToSkillCursor();
            turret.transform.rotation = turretRotationAtSkill;
        }
    }

    public void SetSkillReady(int skillNumber)
    {
        if (!skillOnGoing)
        {
            //no shooting when set skill
            atkComp.enabled = false;
            //click same skill button again to unready skill activation;
            if (skillNum == skillNumber)
            {
                waitingToActivate = false;
                UnreadySkill();
                return;
            }
            //if skill num presses # current skill num -> unready current skill
            if (skillNum != skillNumber && skillNum != -1)
            {
                waitingToActivate = false;
                UnreadySkill();
                //UnreadySkill() set atkComp active again so re-deactivate below
                atkComp.enabled = false;
            }
            //check latest skill num eligibility
            this.skillNum = skillNumber;
            if (skillNumber >= skills.Count || skillNum < 0)
            {
                Debug.Log("Out of skill range. Check your number(0-2)?");
                atkComp.enabled = true;
                return;
            }
            if (!skills[skillNum])
            {
                Debug.LogError("Skill slot " + skillNum + " is null. Forgot to add skill component?");
            }

            waitingToActivate = true;
            currentActiveSkill = skills[skillNum];

            if (!currentActiveSkill.ready)
            {
                Debug.Log("Skill " + skillNum + " isn't ready. Till ready: " + currentActiveSkill.timeTillReady);
                UnreadySkill();
            }
            //eligible -> activate cursor (for cursor-less skill -> activate in this function)
            else
            {
                currentActiveSkill.ActivateSkillCursor();
            }
        }
    }

    public void ActivateSkill()
    {
        if (currentActiveSkill == null)
        {
            Debug.Log("No active skill chosen.");
            return;
        }

        if (currentActiveSkill.lockTurret)
        {
            turretLock = true;
            LockTurretRotationAtSkillStart();
        }
        currentActiveSkill.ActivateSkill();
        waitingToActivate = false;
        skillOnGoing = true;
    }

    public void UnreadySkill()
    {
        atkComp.enabled = true;
        //if added due to SkillUIController buttons >< hotkeys
        if (currentActiveSkill) currentActiveSkill.DeactivateSkill();
        currentActiveSkill = null;
        skillNum = -1;
    }

    public void CheckCurrentActiveSkill()
    {
        if (currentActiveSkill.skillDurationEnded)
        {
            skillOnGoing = false;
            turretLock = false;
            LockTurretRotationAtSkillStart();
            UnreadySkill();
        }
    }

    void GetDirectionToSkillCursor()
    {
        if (currentActiveSkill && currentActiveSkill.skillType == Skill.type.HAS_CURSOR)
        {
            Vector3 dirToCursor = (currentActiveSkill.skillCursor.transform.position - turret.transform.position).normalized;
            dirToCursor.z = 0;
            float angle = Mathf.Atan2(dirToCursor.y, dirToCursor.x) * Mathf.Rad2Deg - 90;
            turretRotationAtSkill = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    //lock Turret rotation at when skill start and deactivate when skill end (for skill with lock turret property)
    void LockTurretRotationAtSkillStart()
    {
        if (turretLock)
        {
            atkComp.enabled = false;
        }
        else
        {
            atkComp.enabled = true;
        }
    }

    public int GetCurrentSkillNameEnum()
    {
        if (!currentActiveSkill) return -1;
        else
        {
            return (int)currentActiveSkill.skillName;
        }
    }
}
