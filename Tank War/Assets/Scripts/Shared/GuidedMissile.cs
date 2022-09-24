using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedMissile : MonoBehaviour
{
    //[HideInInspector]
    public GameObject target;
    private Tank targetHull;

    public int dmg = 60;
    public float speed = 5f;
    public float liveTime = 10f;
    // Start is called before the first frame update
    void Start()
    {
        if (!target) gameObject.SetActive(false);
        else targetHull = target.GetComponent<Tank>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target.activeInHierarchy)
        {
            //CheckReachedTarget();
            GlobalVar.MoveAndRotateTowardDes(target.transform, transform, speed);

        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject e = ObjectPool.instance.GetObjectForType("Explosion", false);
        e.transform.position = this.transform.position;
    }
}
