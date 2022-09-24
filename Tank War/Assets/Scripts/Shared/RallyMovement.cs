using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RallyMovement : MonoBehaviour
{
    Transform rallyPoint;

    [SerializeField]
    MonoBehaviour moveComp;
    //EnemyMovement eComp;
    //AllyMovement aComp;

    [SerializeField]
    float speed = 3f;
    [HideInInspector]
    public bool remainAtRallyPoint = false;
    bool reachedRallyPoint = false;

    void Start()
    {
        //if (moveComp)
        //{
        //    eComp = moveComp as EnemyMovement;
        //    aComp = moveComp as AllyMovement;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (!rallyPoint) this.enabled = false;

        CheckReachedRallyPoint();
        if (!reachedRallyPoint) MoveToRallyPoint(); 
    }

    void CheckReachedRallyPoint()
    {
        if (Vector2.Distance(transform.position, rallyPoint.position) <= 0.5f) reachedRallyPoint = true;

        if (reachedRallyPoint)
        {
            if (gameObject.CompareTag(GlobalVar.eTag)) gameObject.layer = GlobalVar.eTankLayer;
            if (gameObject.CompareTag(GlobalVar.allyTag)) gameObject.layer = GlobalVar.aTankLayer;
            if (!remainAtRallyPoint && moveComp) moveComp.enabled = true;
            reachedRallyPoint = false;
            this.enabled = false;
        }
    }

    public void SetRallyPoint(Transform i, bool stayAtRallyPoint)
    {
        if (gameObject.CompareTag(GlobalVar.eTag))
        {
            gameObject.layer = GlobalVar.eIgnoreLowBlockLayer;
        }
        if (gameObject.CompareTag(GlobalVar.allyTag))
        {
            gameObject.layer = GlobalVar.aIgnoreLowBlockLayer;
        }

        rallyPoint = i;
        remainAtRallyPoint = stayAtRallyPoint;
        reachedRallyPoint = false;
        if (moveComp) moveComp.enabled = false;
    }
    
    public void MoveToRallyPoint()
    {
        Vector3 dirToRally = (rallyPoint.position - transform.position).normalized;
        Vector2 pos = transform.position;
        pos.x += Time.deltaTime * speed * dirToRally.x;
        pos.y += Time.deltaTime * speed * dirToRally.y;
        transform.position = pos;
    }
}
