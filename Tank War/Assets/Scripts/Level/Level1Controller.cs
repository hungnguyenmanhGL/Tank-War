using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Controller : LevelController
{
    static string[] startCutsceneDialog =
    {
        "Operator: Head-up team. The enemy has dispatched a nearby small heavy armor squad to current position. " +
            "Seems like a patrol team, so most likely your unit can still take them by surprise. " +
            "This is unaccounted for, but your mission stands. Just ready for more firefight.",
        "Copy that."
    };
    void Start()
    {
        levelStartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        ActivateStartCutscene(startCutsceneDialog);
        //this.enabled = false;
    }
}
