using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPExplosion : GrenadeExplosion
{
    float empTime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        EMP = true;
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
            if (hull) hull.StartCoroutine(hull.hitByEMP(empTime));
       }
    }
}
