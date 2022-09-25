using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockTargetCursor : MonoBehaviour
{
    public GameObject target;
    List<GameObject> targetList = new List<GameObject>();
    bool activated = false;

    void Start()
    {
        
    }

    //when clicked to lock target registered, call this function on class
    public GameObject ActivateTargetLock()
    {
        if (targetList.Count == 0) return null;
        float minDis = Vector2.Distance(targetList[0].transform.position, transform.position);
        target = targetList[0];
        foreach (GameObject hull in targetList)
        {
            if (Vector2.Distance(hull.transform.position, transform.position) < minDis)
            {
                target = hull;
            }
        }
        //Debug.Log(target.name);
        return target;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(GlobalVar.eTag))
        {
            Tank hull = collision.gameObject.GetComponent<Tank>();
            if (hull) targetList.Add(collision.gameObject);
        }
        //Debug.Log(targetList.Count);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(GlobalVar.eTag))
        {
            targetList.Remove(collision.gameObject);
        }
    }

    void ResetUponCancel()
    {
        target = null;
        targetList.Clear();
        activated = false;
    }
}
