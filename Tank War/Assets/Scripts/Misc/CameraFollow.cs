using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;

    Transform player;
    Vector3 camPos;

    [SerializeField]
    Animator cameraAnimator;

    [SerializeField]
    bool boundLock = false;
    [SerializeField]
    bool lockOnPlayer = true;

    bool inCutscene = false;
    float lastTimeScale = -1;

    [SerializeField]
    float minX, maxX;

    [SerializeField]
    float minY, maxY;

    // Start is called before the first frame update
    void Start()
    {
        if (!instance) instance = this;
        else Destroy(this);

        if (PreLevelDataController.instance && PreLevelDataController.instance.holdPlayer) player = PreLevelDataController.instance.player.transform;
        else player = GameObject.FindGameObjectWithTag(GlobalVar.pTag).transform;
    }

    void LateUpdate()
    {
        if (player && lockOnPlayer)
        {
            camPos = transform.position;
            camPos.x = player.position.x;
            camPos.y = player.position.y;
        }
        if (boundLock) camPos = KeepCamInBounds(camPos);

        if (lockOnPlayer) transform.position = camPos;

        if (inCutscene) Time.timeScale = 1;
    }

    public void ActivateCameraCutscene(string trigger, float time)
    {
        StartCoroutine(WaitForAnimation(time));
        cameraAnimator.SetTrigger(trigger);
    }
    IEnumerator WaitForAnimation(float time)
    {
        lastTimeScale = Time.timeScale;

        UIController.instance.gameObject.SetActive(false);
        inCutscene = true;
        boundLock = false;
        lockOnPlayer = false;
        yield return new WaitForSeconds(time);
        UIController.instance.gameObject.SetActive(true);
        inCutscene = false;
        boundLock = true;
        lockOnPlayer = true;
        cameraAnimator.SetTrigger("Exit");

        Time.timeScale = lastTimeScale;
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
