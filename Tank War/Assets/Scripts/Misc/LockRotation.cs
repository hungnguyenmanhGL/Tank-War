using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    [SerializeField]
    GameObject objectToFollow;

    float yDis;
    Quaternion fixedRotation;
    // Start is called before the first frame update
    void Start()
    {
        fixedRotation = Quaternion.AngleAxis(0, Vector3.forward);
        yDis = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        transform.rotation = fixedRotation;
        transform.position = new Vector3(objectToFollow.transform.position.x, objectToFollow.transform.position.y + yDis, 0);
    }
}
