using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repair : MonoBehaviour
{
    // Start is called before the first frame update
    
    public int repairChargeCount = 1;
    public int repairHP = 50;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Repair Kit"))
        {
            collision.gameObject.SetActive(false);
            repairChargeCount++;
        }
    }

    //repair function for key R & PC platform
    //public void repairHull()
    //{
    //    if (repairCount > 0)
    //    {
    //        if (hull.HP + repairHP > hull.maxHP) hull.HP = hull.maxHP;
    //        else hull.HP += repairHP;

    //        repairCount--;
    //    }
    //    else Debug.Log("No repair kit left.");
    //}
}
