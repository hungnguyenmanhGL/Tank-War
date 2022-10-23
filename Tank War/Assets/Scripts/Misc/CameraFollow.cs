using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;

    Transform player;
    Vector3 mousePos;
    Vector3 middlePos;
    Vector3 camPos;


    [SerializeField]
    Animator cameraAnimator;

    [SerializeField]
    bool boundLock = false;
    [SerializeField]
    bool lockOnPlayer = true;
    [SerializeField]
    bool followMouseDirection = false;

    //used for cutscene
    Vector3 lastPosBeforeCutscene;
    bool[] cameraStartState = { false, false };
    bool inCutscene = false;
    float lastTimeScale = -1;

    [SerializeField]
    float maxDistanceFromPlayer = 10f;
    //avoid jittering from little cursor movement
    float maxDistanceFromCursor = 2f;
    float damping = 8f;

    [SerializeField]
    float minX, maxX;

    [SerializeField]
    float minY, maxY;

    // Start is called before the first frame update
    void Start()
    {
        if (!instance) instance = this;
        else Destroy(this);

        if (PreLevelDataController.instance) SetCameraMode();
        cameraStartState[0] = lockOnPlayer;
        cameraStartState[1] = followMouseDirection;

        if (PreLevelDataController.instance && PreLevelDataController.instance.holdPlayer) player = PreLevelDataController.instance.player.transform;
        else player = GameObject.FindGameObjectWithTag(GlobalVar.pTag).transform;
    }

    void LateUpdate()
    {
        //if set to follow player
        if (player && lockOnPlayer)
        {
            camPos = player.position;
            camPos.z = transform.position.z;
            if (boundLock) camPos = KeepCamInBounds(camPos);
            transform.position = camPos;
        }
        //if set to follow mouse
        if (player && followMouseDirection)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            middlePos = Vector3.Lerp(player.position, mousePos, 0.5f);

            Vector3 deltaPos = (Vector2)middlePos - (Vector2)player.position;
            if (deltaPos.magnitude > maxDistanceFromPlayer)
            {
                // clamp it to max distance
                deltaPos = Vector3.ClampMagnitude(deltaPos, maxDistanceFromPlayer);
                middlePos = player.position + deltaPos;
            }
            middlePos.z = transform.position.z;
            if (boundLock) middlePos = KeepCamInBounds(middlePos);
            if (Vector2.Distance(transform.position, middlePos) > maxDistanceFromCursor)
                transform.position = Vector3.MoveTowards(transform.position, middlePos, Time.deltaTime * damping);
        }

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
        lastPosBeforeCutscene = transform.position;
        cameraAnimator.enabled = true;

        UIController.instance.gameObject.SetActive(false);
        inCutscene = true;
        boundLock = false;
        lockOnPlayer = false;
        followMouseDirection = false;
        yield return new WaitForSeconds(time);
        UIController.instance.gameObject.SetActive(true);
        inCutscene = false;
        boundLock = true;
        lockOnPlayer = cameraStartState[0];
        followMouseDirection = cameraStartState[1];
        cameraAnimator.SetTrigger("Exit");

        cameraAnimator.enabled = false;
        Time.timeScale = lastTimeScale;
        transform.position = lastPosBeforeCutscene;
    }

    public void SetCameraMode()
    {
        if (PreLevelDataController.instance && !PreLevelDataController.instance.cameraFollowPlayer)
        {
            lockOnPlayer = false;
            followMouseDirection = true;
        }
        if (PreLevelDataController.instance && PreLevelDataController.instance.cameraFollowPlayer)
        {
            lockOnPlayer = true;
            followMouseDirection = false;
        }
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
