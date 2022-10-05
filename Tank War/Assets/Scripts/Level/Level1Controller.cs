using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Controller : LevelController
{
    static string[] startCutsceneDialog =
    {
        "Operator: Head-up. The enemy has dispatched a nearby small heavy armor squad to current position. " +
            "Seems like a patrol team, so most likely your unit can still take them by surprise. " +
            "This is unaccounted for, but your mission stands. Take them out fast and avoid getting surrounded.",
        "Operator: Note that you have to destroy all enemy units so they cannot alert others."
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
        CheckObjectiveStatus();
    }
}
