using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//base class for objective controllers, decide win lose condition and handle loading mission end report logic 
public class ObjectiveController : MonoBehaviour
{
    [SerializeField]
    protected bool win = false;
    protected bool gameOver = false;
    protected bool resultDecided = false;

    public AsyncOperation loadMissionReportOperation;
    protected bool missionReportLoaded = false;

    [SerializeField]
    protected int level;
    protected int missionKillCount = 0;
    protected float startTime, endTime;

    [SerializeField]
    protected string primaryObjective;
    [SerializeField]
    protected Text objectiveText;
    [SerializeField]
    protected GameObject player;
    

    void Start()
    {
        //DontDestroyOnLoad(this);
    }

    virtual protected void OnStartEachLevel()
    {
        win = false;
        gameOver = false;
        resultDecided = false;
        startTime = Time.time;

        if (!player)
        {
            player = GameObject.FindGameObjectWithTag(GlobalVar.pTag);
            if (!player) Debug.Log("No player found");
        }
        
    }

    void Update()
    {
        //CheckToLoadMissionReportStat();
    }

    //depend on the mission objective
    virtual protected void CheckWinLoseCondition() { }

    virtual protected void UpdateObjectiveStat() { }

    virtual protected void DisableSingleTons()
    {
        EnemyPool.instance.enabled = false;
    }

    #region level finished and next button click

    //for next button once the level is finished
    public void OnLevelFinishedNextButtonClicked()
    {
        Time.timeScale = 1;

        missionKillCount = EnemyPool.instance.killCount;
        EnemyPool.instance.enabled = false;
        //load mission report
        loadMissionReportOperation = SceneManager.LoadSceneAsync("Mission Summary");
        if (LoadingScreenController.instance)
        {
            LoadingScreenController.instance.SetLoadingOperation(loadMissionReportOperation);
            LoadingScreenController.instance.GetLoadingScreenForLevel(-1);
        }
    }

    //for update loop, called once when next button clicked
    protected void CheckToLoadMissionReportStat()
    {
        if (loadMissionReportOperation != null && loadMissionReportOperation.isDone && !missionReportLoaded)
        {
            //Debug.Log("Mission Report Scene Loaded");
            loadMissionReportOperation = null;
            missionReportLoaded = true;

            if (SummaryHolder.instance)
            {
                SummaryHolder.instance.SetMissionReportStat(win, level, missionKillCount, endTime - startTime);
            }
            //Signal the loading screen that data load progress done -> click to continue
            LoadingScreenController.instance.SetDataLoadDone();
            //Destroy manually
            Destroy(this.gameObject);
            Destroy(this);
        }
    }

    IEnumerator ShowLevelFinishedCanvasAfterTime(bool win)
    {
        yield return new WaitForSeconds(3f);
        UIController.instance.GetLevelFinishedCanvas(win);
        Time.timeScale = 0;
    }

    virtual protected void OnLevelFinished(bool win)
    {
        if (win)
        {
            objectiveText.color = Color.green;
        }
        else
        {
            objectiveText.color = Color.red;
            
        }
        resultDecided = true;
        //UIController.instance.GetLevelFinishedCanvas(win);
        StartCoroutine(ShowLevelFinishedCanvasAfterTime(win));
    }

    #endregion
}
