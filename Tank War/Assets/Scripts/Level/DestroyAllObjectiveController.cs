using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DestroyAllObjectiveController : ObjectiveController
{
    [SerializeField]
    List<GameObject> targetToDestroy;

    //for mission where kill count (target or enemy doesnt spawn immediately)
    public bool showKillCount = true;
    public bool winByKillCount = false;
    [SerializeField]
    int requiredKillCount;
    int currentKillCount;

    StringBuilder sb;
    void Start()
    {
        //DontDestroyOnLoad(this.gameObject);

        if (!winByKillCount) { requiredKillCount = targetToDestroy.Count; }
        UpdateObjectiveStat();

        OnStartEachLevel();

        if (!player) gameOver = true;
    }

    // Update is called once per frame
    void Update()
    {
        CheckWinLoseCondition();
        RemoveDestroyedTarget();

        CheckToLoadMissionReportStat();
    }

    protected override void CheckWinLoseCondition()
    {
        base.CheckWinLoseCondition();

        if (!resultDecided)
        {
            if (!winByKillCount && targetToDestroy.Count == 0)
            {
                win = true;
                base.OnLevelFinished(true);
                return;
            }
            if (winByKillCount && currentKillCount == requiredKillCount)
            {
                win = true;
                base.OnLevelFinished(true);
                return;
            }
            if (player && !player.activeInHierarchy) { 
                gameOver = true; 
                base.OnLevelFinished(false);
                return;
            }
        }

        //for testing 
        if (win || gameOver) resultDecided = true;

        if (resultDecided)
        {
            endTime = Time.time;
        }
    }

    void RemoveDestroyedTarget()
    {
        for (int i=0; i<targetToDestroy.Count; i++)
        {
            if (!targetToDestroy[i] || 
                (targetToDestroy[i] && !targetToDestroy[i].activeInHierarchy))
            {
                targetToDestroy.RemoveAt(i);
                currentKillCount++;
                UpdateObjectiveStat();
            }
        }
    }

    protected override void UpdateObjectiveStat()
    {
        base.UpdateObjectiveStat();
        if (objectiveText)
        {
            StringBuilder sb = new StringBuilder(objectiveText.text);
            sb.Clear();
            sb.Append(primaryObjective);
            if (showKillCount)
            {
                sb.Append(currentKillCount);
                sb.Append("/");
                sb.Append(requiredKillCount);
                objectiveText.text = sb.ToString();
            }
            else objectiveText.text = sb.ToString();
        }
         
    }

}
