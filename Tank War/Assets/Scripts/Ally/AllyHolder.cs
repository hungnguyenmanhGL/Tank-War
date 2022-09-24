using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyHolder : MonoBehaviour
{
    public static AllyHolder instance;
    public List<GameObject> allyList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if (!instance) instance = this;
        else Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i=0; i<allyList.Count; i++)
        {
            if (!allyList[i] || !allyList[i].activeInHierarchy) allyList.RemoveAt(i);
        }
    }
}
