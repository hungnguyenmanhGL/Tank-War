using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupTrigger : MonoBehaviour
{
    bool activated = false;
    public List<GameObject> force;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < force.Count; i++)
        {
            force[i].GetComponent<EnemyMovement>().enabled = false;
            force[i].GetComponent<EnemyAttack>().enabled = false;
            force[i].GetComponent<Collider2D>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(GlobalVar.pTag)
            || collision.gameObject.CompareTag(GlobalVar.allyTag)
            || collision.gameObject.CompareTag(GlobalVar.shellTag))
        {
            if (!activated)
            {
                //Debug.Log("Player activated " + gameObject.name);
                for (int i = 0; i < force.Count; i++)
                {
                    //force[i].SetActive(true);
                    force[i].GetComponent<EnemyMovement>().enabled = true;
                    force[i].GetComponent<EnemyAttack>().enabled = true;
                    force[i].GetComponent<Collider2D>().enabled = true;

                    EnemyPool.instance.activeEnemies.Add(force[i]);
                }
                gameObject.GetComponent<Collider2D>().enabled = false;
                activated = true;
            }
        }
    }
}
