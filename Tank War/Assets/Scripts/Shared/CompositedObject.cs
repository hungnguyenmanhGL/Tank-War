using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CompositedObject : MonoBehaviour
{
    [SerializeField]
    List<GameObject> compList;

    //if the object is a Tank itself -> no need to check to disable
    [SerializeField]
    bool isTank = false;
    [SerializeField]
    bool activateInSequence = false;

    void Start()
    {
        for (int i=0; i<compList.Count; i++)
        {
            if (!compList[i]) compList.RemoveAt(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckKilled();
    }

    void CheckKilled()
    {
        if (!isTank)
        {
            foreach (GameObject comp in compList)
            {
                if (comp.activeInHierarchy) return;
            }
            gameObject.SetActive(false);
        }
    }
}
