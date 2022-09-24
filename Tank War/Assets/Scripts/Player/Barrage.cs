using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Continous barrage that deals damage every n second (damage per tick) while enemy is in AOE
//Put this on the barrage game object
public class Barrage : MonoBehaviour
{
    CircleCollider2D barrageAOE;

    //list of explosion for effect(doesnt do anything)
    [SerializeField]
    List<GameObject> explosions;
    List<GameObject> activeExplosions = new List<GameObject>();
    //must be < explosions.Count
    const int maxExplosionActive = 4;

    List<GameObject> enemyInside = new List<GameObject>();


    int dmgPerTick = 50;

    //how long between tick
    float timeTillNextDamage = 0.5f;
    float timeSinceLastDamage = 0; 

    void Start()
    {
        //barrageAOE = GetComponent<CircleCollider2D>();
    }

    private void OnEnable()
    {
        timeSinceLastDamage = 0;
    }

    private void OnDisable()
    {
        explosions.AddRange(activeExplosions);
        foreach (GameObject explosion in explosions) explosion.SetActive(false);
        activeExplosions.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        ReactivateExplosionEffect();

        float frameTime = Time.time;
        if (frameTime - timeSinceLastDamage >= timeTillNextDamage)
        {
            timeSinceLastDamage = frameTime;
            for(int i=0; i<enemyInside.Count; i++)
            {
                Tank hull = enemyInside[i].GetComponent<Tank>();
                if (hull != null)
                {
                    hull.HP -= dmgPerTick;
                }
            }
        }
    }

    void ReactivateExplosionEffect()
    {
        int activeNum = activeExplosions.Count;
        if (activeNum < maxExplosionActive)
        {
            int rand = Random.Range(0, explosions.Count - 1);
            GameObject e = explosions[rand];
            e.SetActive(true);
            activeExplosions.Add(e);
            explosions.Remove(e);
        }

        for (int i=0; i<activeExplosions.Count; i++)
        {
            if (!activeExplosions[i].activeInHierarchy)
            {
                explosions.Add(activeExplosions[i]);
                activeExplosions.RemoveAt(i);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(GlobalVar.eTag))
        {
            enemyInside.Add(collision.gameObject);
            //Debug.Log(collision.gameObject.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(GlobalVar.eTag))
        {
            enemyInside.Remove(collision.gameObject);
        }
    }

    IEnumerator WaitBeforeReactivateExplosion()
    {
        yield return new WaitForSeconds(0.2f);

    }
}
