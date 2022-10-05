using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SummaryHolder : MonoBehaviour
{
    public static SummaryHolder instance;

    static string[] missionEndReportArr =
    {
        "Well done! Despite the unaccounted presence of a heavy armored squad, you successfully completed the mission. " +
        "Now that you are behind the enemy frontline, opportunity to disrupt their supply chain will arise soon. " +
        "Stay ready for your next mission.",

        "Our assumption was correct. " +
        "The convoys were transporting experimental weapons along with multiple prototype designs. " +
        "This is a crucial contribution to our war effort. " +
        "Not only did we stopped the package from reaching its destination, we also gain valuable insight of their technology. " +
        "The R&D department will ensure that none of your retrieved spoil go to waste.",

        "We have successfully broken through enemy frontline. " +
        "We must maintain this momentum to advance further before they can reorganize. " +
        "Even then, expect fierce resistance the deeper we are in their territory." +
        "There is no other way now. Forward!",

        "The bridge is under our control. However your unit are ordered to return for maintenace. [DECODED]: We just received new intel report. " +
        "There are evidences suggesting enemy movement from their bases up north while we were occupied by their force. " +
        "This is inconclusive and we can't afford to risk all our controlled area. But ignoring the signs even the slightest would be disastrous. " +
        "After witnessing your combat prowess with the new prototype, HQ believes your unit presence would be sufficient in case of such emergency. ",

        "That would have been a disaster if it weren't for you. Unfortunately we don't have time to rest as another enemy movement was sighted. " +
        "The fact that they aren't willing to even mask anymore it is worrysome. " +
        "HQ was certain that they still had some cards up their sleeves before this but now... We had better brace for whatever thrown at us." 
    };

    [HideInInspector]
    public List<string> briefs;

    public string successResult = "MISSION ACCOMPLISHED";
    public string failResult = "MISSION FAILED";

    public Text result;
    public Text killCount;
    public Text timeRequired;
    public Text summaryBrief;

    bool sceneLoading = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!instance) instance = this;
        else { Destroy(gameObject); Destroy(this); }

        sceneLoading = false;

        AddToBriefList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onNextButtonClicked()
    {
        if (!sceneLoading)
        {
            sceneLoading = true;
            SceneManager.LoadScene("Main Menu");
        }
    }

    void AddToBriefList()
    {
        briefs = new List<string>();
        briefs.AddRange(missionEndReportArr);
    }

    public void SetMissionReportStat(bool win, int level, int kill, float timeSpent)
    {
        killCount.text = "KILL COUNT: " + kill;
        if (!win)
        {
            result.text = failResult;
            summaryBrief.text = GlobalVar.missionFailQuote;
        }
        else 
        { 
            result.text = successResult;
            if (level - 1 < 0 || level - 1 >= briefs.Count) summaryBrief.text += "N/A";
            else
                summaryBrief.text += briefs[level - 1]; 

            string time = GlobalVar.convertTime(timeSpent);
            Debug.Log(time);
            timeRequired.text += time;
        }
    }

    void SetBriefText(string s) { summaryBrief.text = s; }
}
