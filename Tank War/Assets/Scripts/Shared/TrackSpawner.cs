using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSpawner : MonoBehaviour
{
    private Vector3 lastPos;
    public Transform leftTrack;
    public Transform rightTrack;
    public float distanceToNextTrackMark = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;   
    }

    // Update is called once per frame
    void Update()
    {
        float distanceDriven = Vector2.Distance(transform.position, lastPos);
        spawnTrackMark(distanceDriven);
    }

    //spawn track mark after predetermined distance
    void spawnTrackMark(float distanceDriven)
    {
        if (distanceDriven >= distanceToNextTrackMark)
        {
            lastPos = transform.position;
            string markName = "Track Mark";
            GameObject leftMark = ObjectPool.instance.GetObjectForType(markName, false);
            GameObject rightMark = ObjectPool.instance.GetObjectForType(markName, false);
            leftMark.transform.position = leftTrack.position;
            leftMark.transform.rotation = leftTrack.rotation;
            rightMark.transform.position = rightTrack.position;
            rightMark.transform.rotation = rightTrack.rotation;
        }
    }
}
