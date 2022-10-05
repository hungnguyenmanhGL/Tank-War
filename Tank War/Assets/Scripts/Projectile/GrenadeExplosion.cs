using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//inherit from bullet need work
public class GrenadeExplosion : Bullet
{
    [SerializeField]
    protected bool haveDealDmg = false;

    protected List<int> affectedLayerList = new List<int>();
    protected List<Tank> tankInRadiusList = new List<Tank>();
    [SerializeField]
    protected float animTime = 0.4f;


    private void OnEnable()
    {
        StartCoroutine(DisableAfterAnim());
    }
    private void OnDisable()
    {
        haveDealDmg = false;
        tankInRadiusList.Clear();
    }

    IEnumerator DisableAfterAnim()
    {
        yield return new WaitForSeconds(animTime);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(GlobalVar.allyTag) || collision.gameObject.CompareTag(GlobalVar.eTag)
            || collision.gameObject.CompareTag(GlobalVar.pTag))
        {
            //Debug.Log(haveDealDmg);
            //only get tank component if havent dealt dmg
            if (!haveDealDmg)
            {
                Tank hull = collision.gameObject.GetComponent<Tank>();
                if (hull) tankInRadiusList.Add(hull);
            }
            //Debug.Log(tankInRadiusList.Count);
        }
    }

    void Update()
    {
        DealDamageAndEffect();
    }

    //this get called before triggerEnter2D for boss level 5
    protected virtual void DealDamageAndEffect()
    {
        if (!haveDealDmg && tankInRadiusList.Count > 0)
        {
            foreach (Tank hull in tankInRadiusList)
            {
                hull.TakeDamage(this.dmg);
            }
            haveDealDmg = true;
        }
    }
}
