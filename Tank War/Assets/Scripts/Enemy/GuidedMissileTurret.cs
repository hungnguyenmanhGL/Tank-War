using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedMissileTurret : MonoBehaviour
{
    private EnemyMovement hull;
    private GameObject target;
    [SerializeField]
    GameObject missilePrefab;

    float distanceToTarget;

    bool readyToFire = true;
    public List<Transform> firingPoint;

    float reloadTime = 10f;
    float engageRange;
    // Start is called before the first frame update
    void Start()
    {
        hull = GetComponent<EnemyMovement>();
        if (hull)
        {
            target = hull.target;
            engageRange = hull.engageRange;
        }
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
        target = hull.target;

        if (target && target.activeInHierarchy)
        {
            Attack();
        }
    }

    void Attack()
    {
        if (target) distanceToTarget = Vector2.Distance(target.transform.position, transform.position);

        if (distanceToTarget <= engageRange)
        {
            if (readyToFire)
            {
                foreach (Transform i in firingPoint)
                {
                    GameObject missile = ObjectPool.instance.GetObjectForType(missilePrefab.name, false);
                    missile.transform.position = i.transform.position;
                    missile.transform.rotation = i.transform.rotation;

                    GuidedMissile guide = missile.GetComponent<GuidedMissile>();
                    guide.target = target;
                    
                }
                StartCoroutine(setReadyToFire());
            }
        }
    }
}
