using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//handle the spawn, escape of target in interception mission
public class Level2Controller : MonoBehaviour
{
    [HideInInspector]
    public int numOfConvoyTotal = 6;

    private bool activateNextWave = true;
    private List<EscortMovement> activeConvoy;

    private bool dialogLoaded = false;
    private List<string> dialog;

    bool countingDown = false;
    float countDownStart = -1f;
    float countDownEnd;
    float countDown;

    public int numOfConvoyEscaped = 0;
    public int numOfConvoyDestroyed = 0;
    //edit this based on your level
    public int maxNumOfConvoyEscaped = 0;
    public int index = 0;
    public int waveNum = 0;

    public List<EscortMovement> convoy;
    public int[] numOfConvoyToActivate = { 1, 2, 2, 1};
    public float breakTillNextWave = 20f;

    public bool finished = false;

    //point to active convoys
    public List<GameObject> convoyPointer;
    //[SerializeField]
    //List<RectTransform> pointerRectTransform;
    
    //show in scene dialog
    public Text uiDialog;
    //count down to next convoy wave;
    public Text countDownTxt;


    void Start()
    {
        initDialog();
 
        numOfConvoyTotal = convoy.Count;

        for (int i = 0; i < convoyPointer.Count; i++)
        {
            convoyPointer[i].SetActive(false);
            //pointerRectTransform.Add(convoyPointer[i].GetComponent<RectTransform>());
        }

        activeConvoy = new List<EscortMovement>();
        for (int i=0; i<convoy.Count; i++)
        {
            Debug.Log("Disable escort Comp " + (i + 1));
            convoy[i].enabled = false;
        }
     
    }

    void initDialog()
    {
        uiDialog.enabled = false;

        string first = "The first convoy will arrive by the bottom lane, ETA 20 seconds. Ready the ambush.";
        string second = "The next 2 convoys have already been alerted of your attack. " +
            "They are passing through the upper lanes together, probably to divert your attention." +
            "The top convoy, with no escort, is moving at unsual speed.";
        string third = "2 heavily escorted packages incoming from top lanes.";
        string fourth = "Looks like they run out of whatever they juiced the previous convoys with. " +
            "This last one coming in slow, but protected by an entire heavy tank squad. Stay on guard.";

        dialog = new List<string>();
        dialog.Add(first);
        dialog.Add(second);
        dialog.Add(third);
        dialog.Add(fourth);
    }

    void loadDialog()
    {
        if (waveNum < dialog.Count)
        {
            //check if dialog text already loaded
            if (!dialogLoaded)
            {
                uiDialog.text = "HQ: " + dialog[waveNum];
                uiDialog.gameObject.SetActive(true);
                uiDialog.enabled = true;
                dialogLoaded = true;
            }
        }
    }

    //IF ACTIVE CONVOY EMPTY -> ACTIVATE NEXT WAVE -> PUSH INACTIVE NEXT WAVE TO ACTIVE CONVOY LIST 
    //WAIT FOR BREAK -> ACTIVATE NEXT WAVE
    IEnumerator breakBeforeNextWave()
    {
        activateNextWave = false;
        if (waveNum >= numOfConvoyToActivate.Length)
        {
            yield break;
        }
        for (int i = 0; i < numOfConvoyToActivate[waveNum]; i++)
        {
            Debug.Log("Add convoy " + index + " to ready");
            activeConvoy.Add(convoy[index]);
            index++;
        }
        loadDialog();
        waveNum++;

        //show countdown text 
        if (countDownStart < 0f) countDownStart = Time.time;
        countingDown = true;
        countDownTxt.enabled = true;

        yield return new WaitForSeconds(breakTillNextWave);
        //disable countdown text
        dialogLoaded = false;
        countingDown = false;
        countDownTxt.enabled = false;
        countDownStart = -1f;

        uiDialog.enabled = false;

        for (int i = 0; i < activeConvoy.Count; i++) 
        {
            activeConvoy[i].enabled = true;
            EnemyPool.instance.activeEnemies.Add(activeConvoy[i].escortTarget);
            EnemyPool.instance.activeEnemies.AddRange(activeConvoy[i].escortForce);
        }
        Debug.Log("Enable convoy " + index);
    }

    // Update is called once per frame
    void Update()
    {
        checkToActivateNextWave();
        if (activateNextWave) StartCoroutine(breakBeforeNextWave());

        if (countingDown)
        {
            countDownEnd = Time.time;
            countDown = breakTillNextWave - (countDownEnd - countDownStart);
            countDownTxt.text = "Next convoy ETA: " + (int)countDown + "s";
        }

        pointToActiveConvoy();
        updateConvoyStatus();
        checkCurrentLevelCondition();
    }

    void pointToActiveConvoy()
    {
        for (int i = 0; i < activeConvoy.Count; i++)
        {
            GameObject target = activeConvoy[i].escortTarget;
            if (target.activeInHierarchy && activeConvoy[i].enabled)
            {
                convoyPointer[i].SetActive(true);
                //get angle from camera to target
                Vector3 targetToPoint = target.transform.position;
                Vector3 fromPosition = Camera.main.transform.position;
                Vector3 dir = (targetToPoint - fromPosition).normalized;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

                //pointerRectTransform[i].rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                convoyPointer[i].transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                //get screen point of target the arrow pointing to
                Vector3 targetWorldToScreenPoint = Camera.main.WorldToScreenPoint(targetToPoint);
                bool targetOffScreen = targetWorldToScreenPoint.x < 0 || targetWorldToScreenPoint.x > Screen.width
                    || targetWorldToScreenPoint.y < 0 || targetWorldToScreenPoint.y > Screen.height;

                if (targetOffScreen)
                {
                    Vector3 cappedTargetScreenPosition = targetWorldToScreenPoint;
                    if (cappedTargetScreenPosition.x <= 0) cappedTargetScreenPosition.x = 0;
                    if (cappedTargetScreenPosition.x >= Screen.width) cappedTargetScreenPosition.x = Screen.width;
                    if (cappedTargetScreenPosition.y <= 0) cappedTargetScreenPosition.y = 0;
                    if (cappedTargetScreenPosition.y >= Screen.height) cappedTargetScreenPosition.x = Screen.height;

                    //replace main camera for ui camera if not using main camera for ui
                    Vector3 pointerWorldPosition = Camera.main.ScreenToWorldPoint(cappedTargetScreenPosition);
                    //pointerRectTransform[i].position = pointerWorldPosition;
                    //pointerRectTransform[i].localPosition = new Vector2(pointerWorldPosition.x, pointerWorldPosition.y);

                    convoyPointer[i].transform.position = pointerWorldPosition;
                    convoyPointer[i].transform.localPosition = new Vector2(pointerWorldPosition.x, pointerWorldPosition.y);
                }
                else
                {
                    convoyPointer[i].SetActive(false);
                }
            }
            else if (!target.activeInHierarchy || !activeConvoy[i].enabled)
            {
                convoyPointer[i].SetActive(false);
            }
        }
    }

    void checkToActivateNextWave()
    {
        if (activeConvoy.Count > 0) return;
        activateNextWave = true;
    }

    void updateConvoyStatus()
    {
        
        List<int> toBeRemoved = new List<int>();
        for (int i=0; i<activeConvoy.Count; i++)
        {
            if (activeConvoy[i].escaped)
            {
                numOfConvoyEscaped++;
                toBeRemoved.Add(i);
                
            }
            if (activeConvoy[i].destroyed)
            {
                numOfConvoyDestroyed++;
                toBeRemoved.Add(i);
            }
        }

        for (int i=0; i<toBeRemoved.Count; i++) activeConvoy.RemoveAt(toBeRemoved[i]);
    }

    void checkCurrentLevelCondition()
    {
        //if win or gameover -> no need to check anymore
        
        if (numOfConvoyEscaped > 1)
        {
            Debug.Log("Lost, escaped = " + numOfConvoyEscaped);
        }
        if (numOfConvoyDestroyed >= numOfConvoyTotal - maxNumOfConvoyEscaped 
            && activeConvoy.Count == 0 && index == convoy.Count)
        {
            finished = true;
        }
        
    }

    //public void getSummaryReport()
    //{
    //    //Time.timeScale = 1;
    //    SceneManager.LoadScene("Mission Summary");
    //    StartCoroutine(waitForSceneLoad());
    //}

    //public IEnumerator waitForSceneLoad()
    //{
    //    yield return new WaitForSeconds(1);
    //    GameObject canvas = GameObject.FindGameObjectWithTag("Report");
    //    SummaryHolder summaryHolder = canvas.GetComponent<SummaryHolder>();
    //    SummaryHolder report = summaryHolder;
    //    if (!win)
    //    {
    //        report.result.text = report.failResult;
    //        report.summaryBrief.text = "Mission failed! We'll get them next time";
    //    }
    //    if (win)
    //    {
    //        report.result.text = "Mission Accomplished";
    //        //text already in SummaryHolder class, index = level - 1
    //        report.summaryBrief.text += "\n" + report.briefs[1];
    //    }
    //    report.killCount.text = "KILL COUNT: " + EnemyPool.instance.killCount;
    //    EnemyPool.instance.enabled = false;
    //    string time = convertTime();
    //    Debug.Log(time);
    //    report.timeRequired.text += time;
        
    //    Destroy(this);
    //}

    //public string convertTime()
    //{
    //    string result = " ";
    //    float timeInSecond = endTime - startTime;
    //    if (timeInSecond > 3600)
    //    {
    //        int timeInHour = (int)timeInSecond / 3600;
    //        if (timeInHour < 10) result += "0" + timeInHour + ":";
    //        else result += timeInHour + ":";
    //        timeInSecond -= timeInHour * 3600;
    //    }
    //    else result += "0:";
        
    //    if (timeInSecond > 60)
    //    {
    //        int timeInMinute = (int)timeInSecond / 60;
    //        if (timeInMinute < 10) result += "0" + timeInMinute + ":";
    //        else result += timeInMinute + ":";
    //        timeInSecond -= timeInMinute * 60;
    //    }
    //    else result += "0:";

    //    if (timeInSecond < 10) result += "0" + (int)timeInSecond;
    //    else result += (int)timeInSecond;
    //    return result;
    //}
}
