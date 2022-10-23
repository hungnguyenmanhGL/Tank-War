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

    private void Update()
    {
        UpdateScale();   
    }

    protected override void UpdateScale()
    {
        switch (flightState)
        {
            default:
            case 0:
                transform.localScale += Vector3.one * scaleChangeMultiplier * Time.deltaTime;
                if (transform.localScale.x >= maxScale) { flightState = 1; }
                break;
            case 1:
                transform.localScale -= Vector3.one * scaleChangeMultiplier * Time.deltaTime;
                if (transform.localScale.x <= normalScale) flightState = 2;
                break;
            case 2:
                break;
        }
    }

    IEnumerator WaitTillActivate()
    {
        yield return new WaitForSeconds(timeBeforeActivate);
        Explode();
    }
}
