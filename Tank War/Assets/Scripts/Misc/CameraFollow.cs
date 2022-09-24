using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Transform player;
    Vector3 camPos;

    [SerializeField]
    bool boundLock = false;

    [SerializeField]
    float minX, maxX;

    [SerializeField]
    float minY, maxY;

    // Start is called before the first frame update
    void Start()
    {
        if (PreLevelDataController.instance && PreLevelDataController.instance.holdPlayer) player = PreLevelDataController.instance.player.transform;
        else player = GameObject.FindGameObjectWithTag(GlobalVar.pTag).transform;
    }

    void LateUpdate()
    {
        camPos = transform.position;
        camPos.x = player.position.x;
        camPos.y = player.position.y;

        if (boundLock) transform.position = KeepCamInBounds(camPos);
        else transform.position = camPos;
    }

    Vector3 KeepCamInBounds(Vector3 camPos)
    {
        if (camPos.x < minX)
            camPos.x = minX;

        if (camPos.x > maxX)
            camPos.x = maxX;

        if (camPos.y < minY) camPos.y = minY;
        if (camPos.y > maxY) camPos.y = maxY;

        return camPos;
        //Debug.Log(camPos);
    }
}
