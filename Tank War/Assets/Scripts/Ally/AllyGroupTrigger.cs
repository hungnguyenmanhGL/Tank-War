using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyGroupTrigger : MonoBehaviour
{
    public List<GameObject> allyGroup;

    bool activated = false;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject tank in allyGroup)
        {
            AllyMovement moveComp = tank.GetComponent<AllyMovement>();
            AllyAttack atkComp = tank.GetComponent<AllyAttack>();
            Tank t = tank.GetComponent<Tank>();

            t.HP = 9999;
            moveComp.enabled = false;
            atkComp.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Ally"))
        {
            if (!activated)
            {
                foreach (GameObject tank in allyGroup)
                {
                    AllyMovement moveComp = tank.GetComponent<AllyMovement>();
                    AllyAttack atkComp = tank.GetComponent<AllyAttack>();
                    Tank t = tank.GetComponent<Tank>();

                    t.HP = t.maxHP;
                    moveComp.enabled = true;
                    atkComp.enabled = true;
                }
                if (AllyHolder.instance != null) AllyHolder.instance.allyList.AddRange(allyGroup);
                activated = true;
            }
            this.enabled = false;
        }
    }
}
