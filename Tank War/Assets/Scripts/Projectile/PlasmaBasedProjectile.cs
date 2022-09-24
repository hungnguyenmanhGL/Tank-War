using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//same with bullet, but doesnt use collider but trigger
public class PlasmaBasedProjectile : Bullet
{
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(GlobalVar.eTag))
        {
            Tank hull = collision.gameObject.GetComponent<Tank>();
            if (hull)
            {
                hull.TakeDamage(dmg);
            }
        }
        if (collision.gameObject.CompareTag(GlobalVar.shellTag))
        {
            collision.gameObject.SetActive(false);
        }
    }
}
