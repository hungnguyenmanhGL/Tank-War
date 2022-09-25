using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rain-from-above skill, deal damage per tick
//The cursor will be used to indicate the AOE and also the game object to handle barrage logic (check Barrage class)
public class BarrageSkill : Skill
{
    Barrage barrageComp;
    
    //firing effect, creating the illusion of bombardment
    [SerializeField]
    List<Transform> firingPoints;
    List<GameObject> firingFlashes = new List<GameObject>();

    const int maxActiveFlash = 2;
    int lastFiringPoint = -1;
    int firingFlashActive = 0;

    void Start()
    {
        skillType = type.HAS_CURSOR;
        activeTime = 3f;
        cooldown = 30f;
        lockTurret = true;

        barrageComp = skillCursor.GetComponent<Barrage>();
        if (barrageComp) barrageComp.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckCooldown();
        if (needUpdate) UpdateSkillEffect();
        if (skillDurationEnded) CursorFollowMouse();
    }

    public override void ActivateSkillCursor() 
    { 
        if (!skillCursor.activeInHierarchy)
        {
            skillCursor.SetActive(true);
        }
        skillDurationEnded = true;
    }

    public override void ActivateSkill()
    {
        base.ActivateSkill();

        //Debug.Log("Barrage activated!");
        barrageComp.enabled = true;
        needUpdate = true;
    }

    public override void UpdateSkillEffect()
    {
        firingFlashActive = firingFlashes.Count;

        if (firingFlashActive < maxActiveFlash)
        {
            int index = Random.Range(0, firingPoints.Count-1);
            while (index == lastFiringPoint) index = Random.Range(0, firingPoints.Count - 1);

            lastFiringPoint = index;
            GameObject flash = GlobalVar.GetFiringFlash(firingPoints[index]);
            //flash.transform.parent = firingPoints[index];
            firingFlashes.Add(flash);
        }

        for (int i=0; i<firingFlashes.Count; i++)
        {
            if (!firingFlashes[i].activeInHierarchy) {
                firingFlashes[i].transform.parent = null;
                firingFlashes.RemoveAt(i); 
            }
        }
    }

    public override void DeactivateSkill()
    {
        needUpdate = false;
        barrageComp.enabled = false;
        skillCursor.SetActive(false);

        //clear the effect variables
        firingFlashes.Clear();
        firingFlashActive = 0;
        lastFiringPoint = -1; 
    }
}
