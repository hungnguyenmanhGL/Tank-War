﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTextBubbleController : MonoBehaviour
{
    [SerializeField]
    GameObject textBubble;
    [SerializeField]
    Text comm;

    static string[] reload = { "Reloading!", "Shell loading!" };

    static string hit = "Hit!";

    static string[] gotHit = { "Taking fire!", "We're hit!", "Under attack!"};

    static string empBomb = "EMP bomb ready!";
    static string barrage = "Barrage ready!";

    Coroutine currentCoroutine;
    

    enum status { RELOAD, SKILL, HIT, GOT_HIT, NULL};
    status currentStatus;
    // Use this for initialization
    void Start()
    {
        if (!comm)
        {
            comm = textBubble.GetComponentInChildren<Text>();
            if (!comm)
            {
                Debug.LogError("No text found for text bubble");
                this.enabled = false;
            }
        }

        textBubble.SetActive(false);
    }

    IEnumerator DisableTextBubble(float time)
    {
        yield return new WaitForSeconds(time);
        comm.text = "";
        textBubble.SetActive(false);
        currentStatus = status.NULL;
        currentCoroutine = null;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void WhenReload(float time) 
    {
        if (currentStatus != status.SKILL)
        {
            currentStatus = status.RELOAD;
            comm.text = reload[Random.Range(0, reload.Length)];
            textBubble.SetActive(true);
            if (currentCoroutine != null) { StopCoroutine(currentCoroutine); }
            currentCoroutine = StartCoroutine(DisableTextBubble(time));
        }
    }

    public void WhenGotHit()
    {
        float activeTime = 1f;
        if (currentStatus != status.RELOAD && currentStatus != status.GOT_HIT && currentStatus != status.SKILL)
        {
            currentStatus = status.GOT_HIT;
            comm.text = gotHit[Random.Range(0, gotHit.Length)];
            textBubble.SetActive(true);
            if (currentCoroutine != null) { StopCoroutine(currentCoroutine); }
            currentCoroutine = StartCoroutine(DisableTextBubble(activeTime));
        }
    }

    public void WhenSkill(GlobalVar.skill skill)
    {
        float activeTime = 1f;
        currentStatus = status.SKILL;
        if (skill == GlobalVar.skill.EMP_BOMB)
        {
            comm.text = empBomb;
        }
        if (skill == GlobalVar.skill.BARRAGE) { comm.text = barrage; }

        textBubble.SetActive(true);
        if (currentCoroutine != null) { StopCoroutine(currentCoroutine); }
        currentCoroutine = StartCoroutine(DisableTextBubble(activeTime));
    }
}
