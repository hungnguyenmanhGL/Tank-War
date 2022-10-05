using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    protected List<ObjectiveController> objectiveControllerList;

    protected float levelStartTime;

    protected float timeToStartCutscene = 2f;
    protected bool startCutsceneActivated = false;
    protected bool levelFinishedLoading = false;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    virtual protected void ActivateStartCutscene(string[] startCutsceneDialog)
    {
        if (!startCutsceneActivated && Time.time - levelStartTime >= timeToStartCutscene)
        {
            List<string> dialog = new List<string>();
            dialog.AddRange(startCutsceneDialog);

            startCutsceneActivated = true;
            UIController.instance.GetCutsceneCanvas(dialog);
        }
    }

    virtual protected void ActivateCutsceneWithText(string[] dialogArr)
    {
        List<string> dialog = new List<string>();
        dialog.AddRange(dialogArr);

        startCutsceneActivated = true;
        UIController.instance.GetCutsceneCanvas(dialog);
    }

    protected IEnumerator GetLevelFinishedScreen(bool win)
    {
        levelFinishedLoading = true;
        yield return new WaitForSeconds(2f);
        UIController.instance.GetLevelFinishedCanvas(win);
        Time.timeScale = 0;
    }

    virtual protected void CheckObjectiveStatus()
    {
        int objectiveCompletedCount = 0;
        foreach (ObjectiveController i in objectiveControllerList)
        {
            if (i.resultDecided && i.gameOver && !levelFinishedLoading)
            {
                StartCoroutine(GetLevelFinishedScreen(false));
                return;
            }
            if (i.resultDecided && i.win) { objectiveCompletedCount++; }
        }
        if (objectiveCompletedCount == objectiveControllerList.Count && !levelFinishedLoading)
        {
            StartCoroutine(GetLevelFinishedScreen(true));
        }
    }
}
