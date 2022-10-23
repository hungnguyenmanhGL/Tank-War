using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//type of ammo that explode upon reaches a predetermined destination
//the explosion game object should deal dmg instead of this
public class ExplodeAtTargetBullet : Bullet
{
    protected Vector3 explodeDes;
    protected Vector3 middlePos;
    //decide which state the bullet is in -> going up or down to change scale accordingly
    protected int flightState = 0;

    [SerializeField]
    float speed = 5f;

    [SerializeField]
    protected float normalScale = 5f;
    [SerializeField]
    protected float maxScale = 10f;
    [SerializeField]
    protected float scaleChangeMultiplier = 4f;

    void Start()
    {
        ammoType = GlobalVar.ammo.EXPLODE_ON_DES;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawLine(transform.position, explodeDes);
        MoveToSetDestionation();
        UpdateScale();
        DisableAfterReachingMaxRange();
    }

    protected void MoveToSetDestionation()
    {
        GlobalVar.MoveAndRotateTowardDes(explodeDes, transform, speed);
    }

    protected virtual void UpdateScale()
    {
        switch (flightState)
        {
            default:
            case 0://if going up -> scale up till max && check if reached middle. IF reached middle -> move to state 2 
                if (transform.localScale.x < maxScale) transform.localScale += Vector3.one * scaleChangeMultiplier * Time.deltaTime;
                if (Vector2.Distance(transform.position, middlePos) <= 0.3f) { flightState = 1; }
                break;
            case 1://going down -> down scale till min
                if (transform.localScale.x > normalScale) transform.localScale -= Vector3.one * scaleChangeMultiplier * Time.deltaTime;
                break;
            case 2:
                break;
        }
    }
    //if reach max range or reached set destination -> explode
    protected override void DisableAfterReachingMaxRange()
    {
        if ((Vector2.Distance(transform.position, explodeDes) <= 0.5f) || (Vector2.Distance(transform.position, startingPoint) >= maxRange))
        {
            flightState = 2;
            Explode();
        }
    }

    protected void Explode()
    {
        GameObject e = ObjectPool.instance.GetObjectForType(explosion.name, false);
        e.transform.position = transform.position;
        transform.localScale = normalScale * Vector3.one;
        flightState = 0;
        gameObject.SetActive(false);
    }

    public void SetExplodeDestination(Vector3 pos)
    {
        explodeDes = pos;
        middlePos = Vector3.Lerp(startingPoint, explodeDes, 0.5f);
        //Debug.Log(middlePos);
    }

}
