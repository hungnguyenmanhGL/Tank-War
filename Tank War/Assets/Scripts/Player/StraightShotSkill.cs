using System.Collections;
using UnityEngine;

//Skill where player shoot a special projectile
//Skill cursor will be the bullet game object for this type of skill
public class StraightShotSkill : Skill
{
    [SerializeField]
    GameObject projectile;
    [SerializeField]
    Rigidbody2D shellBody;
    [SerializeField]
    Transform firingPoint;

    float firingForce = 0.04f;

    void Start()
    {
        skillType = type.HAS_CURSOR;
        skillCursor.SetActive(false);
        lockTurret = false;
        skillDurationEnded = true;
        activeTime = 3f;
        //cooldown = 10f;
    }

    IEnumerator DisableSkillCursor()
    {
        yield return new WaitForSeconds(activeTime);
        skillCursor.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckCooldown();
        CursorFollowTurret();
    }

    public override void ActivateSkillCursor()
    {
        if (skillCursor) skillCursor.SetActive(true);
    }

    public override void ActivateSkill()
    {
        base.ActivateSkill();
        skillCursor.SetActive(false);
        //Debug.Log("EMP Grenade Fired");
        GlobalVar.GetFiringFlash(firingPoint);

        projectile.SetActive(true);
        projectile.transform.position = firingPoint.position;
        projectile.transform.rotation = firingPoint.rotation;

        shellBody.AddForce(firingForce * firingPoint.up, ForceMode2D.Impulse);
        //Disable the bullet after active time runs out
        StartCoroutine(DisableSkillRelatedObject(projectile, 4f));
        //the skill duration ends when the shot is fired
        skillDurationEnded = true;
    }

    public override void DeactivateSkill()
    {
        skillCursor.SetActive(false);
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
}
