using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

//Destroy n enemy target - do not let n target escape
public class InterceptionObjectiveController : ObjectiveController
{
    // wave spawn controller here
    [SerializeField]
    Level2Controller targetSpawnController;

    static StringBuilder firstObj = new StringBuilder();
    static StringBuilder secObj = new StringBuilder();

    const string firstObjOpen = "Destroy enemy ";
    const string secObjOpen = "Do not let more than ";

    [SerializeField]
    string targetType = "convoy(s)";

    int numOfTargetTotal;
    int numOfTargetDestroyed = 0;
    int numOfTargetEscaped = 0;
    [SerializeField]
    int maxNumOfTargetEscaped = 1;

    void Start()
    {
        OnStartEachLevel();
        UpdateObjectiveStat();

        if (!player) gameOver = true;

        //DontDestroyOnLoad(this.gameObject);
    }

    protected override void OnStartEachLevel()
    {
        base.OnStartEachLevel();

        numOfTargetDestroyed = 0;
        numOfTargetEscaped = 0;
        if (!targetSpawnController) Debug.LogError("No target controller found");
        else
        {
            numOfTargetTotal = targetSpawnController.numOfConvoyTotal;
            maxNumOfTargetEscaped = targetSpawnController.maxNumOfConvoyEscaped;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckWinLoseCondition();

        CheckToLoadMissionReportStat();

        if (targetSpawnController.numOfConvoyDestroyed != numOfTargetDestroyed || 
            targetSpawnController.numOfConvoyEscaped != numOfTargetEscaped) { UpdateObjectiveStat(); }
    }

    protected override void UpdateObjectiveStat()
    {
        base.UpdateObjectiveStat();
        numOfTargetDestroyed = targetSpawnController.numOfConvoyDestroyed;
        numOfTargetEscaped = targetSpawnController.numOfConvoyEscaped;

        firstObj.Clear();
        firstObj.Append(firstObjOpen);
        firstObj.Append(targetType + "(s) " + numOfTargetDestroyed + "/" + numOfTargetTotal + "\n");

        secObj.Clear();
        secObj.Append(secObjOpen + maxNumOfTargetEscaped + " " + targetType + "(s)" + " escape " + numOfTargetEscaped + "/" + maxNumOfTargetEscaped);

        firstObj.Append(secObj);

        objectiveText.text = firstObj.ToString();
    }

    protected override void CheckWinLoseCondition()
    {
        base.CheckWinLoseCondition();

        //for testing
        if (win || gameOver) resultDecided = true;

        if (!resultDecided)
        {
            if (!player.activeInHierarchy || numOfTargetEscaped > maxNumOfTargetEscaped)
            {
                gameOver = true;
                base.OnLevelFinished(false);
                return;
            }

            if (targetSpawnController.finished)
            {
                win = true;
                base.OnLevelFinished(true);
                return;
            }
        }
        if (resultDecided)
        {
            endTime = Time.time;
        }
    }

}
