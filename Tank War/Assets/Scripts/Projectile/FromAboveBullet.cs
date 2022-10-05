using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//type of bullet that appears as warning on target location -> wait few seconds -> explodes
//may need to effect rocket falling animation as effect
public class FromAboveBullet : ExplodeAtTargetBullet
{
    [SerializeField]
    float timeBeforeActivate = 5f;

    void Start()
    {
        ammoType = GlobalVar.ammo.RAIN;
    }

    private void OnEnable()
    {
        StartCoroutine(WaitTillActivate());
    }

    IEnumerator WaitTillActivate()
    {
        yield return new WaitForSeconds(timeBeforeActivate);
        Explode();
    }
}
