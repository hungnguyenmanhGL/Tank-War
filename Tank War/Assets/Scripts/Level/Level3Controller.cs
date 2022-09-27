using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Level3Controller : LevelController
{
    static string[] cutsceneDialog =
    {
        "Operator: Let me give you a quick run-down of your new armaments. " +
            "You're probably familiar with the battle tank you have used up till now. " +
            "This brand new prototype does a little more than that.",
        "Operator: Beside the normal turret system, every universal tank like this comes up with 2 auxilary modules." +
            "Their functions may vary and it would depend on your operating squad to make the best out of them." +
            "Remember that each module requires time to cooldown after every use. That will be all."
        //"Operator: EMP shell is exactly what its name suggests. Use them to temporarily disable your enemy.\n" +
        //    "CONV shell is still an experimental ammunition, however, its test result is good enough. " +
        //    "On hit, the shell seizes control of the target vehicle and transfers it to a remote team. " +
        //    "However the shell isn't complete and only works on regular enemy hull types. That's all."
    };

    void Start()
    {
        levelStartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        ActivateStartCutscene(cutsceneDialog);
    }
}
