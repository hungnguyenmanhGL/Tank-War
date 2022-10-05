using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//type of ammo that explode upon reaches a predetermined destination
//the explosion game object should deal dmg instead of this
public class ExplodeAtTargetBullet : Bullet
{
    protected Vector3 explodeDes;

    void Start()
    {
        ammoType = GlobalVar.ammo.EXPLODE_ON_DES;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(transform.position, explodeDes);
        DisableAfterReachingMaxRange();
    }
    //if reach max range or reached set destination -> explode
    protected override void DisableAfterReachingMaxRange()
    {
        if ((Vector2.Distance(transform.position, explodeDes) <= 0.5f) || (Vector2.Distance(transform.position, startingPoint) >= maxRange))
        {
            Explode();
        }
    }

    protected void Explode()
    {
        GameObject e = ObjectPool.instance.GetObjectForType(explosion.name, false);
        e.transform.position = transform.position;
        gameObject.SetActive(false);
    }

    public void SetExplodeDestination(Vector3 pos)
    {
        explodeDes = pos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Explode();
    }
}
