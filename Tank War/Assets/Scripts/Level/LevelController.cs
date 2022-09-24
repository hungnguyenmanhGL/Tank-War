using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    protected float levelStartTime;

    protected float timeToStartCutscene = 2f;
    protected bool startCutsceneActivated = false;
    
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
}
