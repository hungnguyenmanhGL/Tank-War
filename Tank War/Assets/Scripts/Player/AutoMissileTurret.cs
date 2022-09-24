using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AutoMissileTurret : MonoBehaviour
{
    [SerializeField]
    GameObject missilePrefab;
    GameObject target;

    [SerializeField]
    List<Transform> firingPoint;

    bool readyToFire = true;

    public float range = 25f;
    public float reloadTime = 8f;


    // Use this for initialization
    void Start()
    {

    }

    IEnumerator setReadyToFire()
    {
        readyToFire = false;
        yield return new WaitForSeconds(reloadTime);
        readyToFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        //
        if (!target || (target && !target.activeInHierarchy)) SetTarget();

        if (target && target.activeInHierarchy) AutoAttack();
    }

    void SetTarget()
    {
        List<Tank> enemyInRange = new List<Tank>();
        // add enemy to in range list
        foreach (GameObject e in EnemyPool.instance.activeEnemies)
        {
            if (Vector2.Distance(e.transform.position, transform.position) <= range) enemyInRange.Add(e.GetComponent<Tank>());
        }

        //priortize hull type
        GameObject temp = null;
        Tank.Hull maxHullType = Tank.Hull.Light;
        for(int i=0; i<enemyInRange.Count; i++)
        {
            if (enemyInRange[i].hullType > maxHullType) { 
                maxHullType = enemyInRange[i].hullType;
                temp = enemyInRange[i].gameObject;
            }
        }

        if (temp) target = temp;
    }

    void AutoAttack()
    {
        if (readyToFire)
        {
            foreach (Transform point in firingPoint)
            {
                GameObject missile = ObjectPool.instance.GetObjectForType(missilePrefab.name, false);
                missile.transform.position = point.position;
                missile.transform.rotation = point.rotation;

                missile.GetComponent<GuidedMissile>().target = this.target;
            }
            StartCoroutine(setReadyToFire());
        }
    }
}
