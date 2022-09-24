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

        "Despite our victory, the enemy defense has served its purpose, buying enough time for them to evacuate their command center. " +
        "Your effort was not in vain though, as they couldn't destroy all documents containing classified info. " +
        "Looking through those files, HQ is certain that the enemy still have some cards up their sleeves. " +
        "Now that they are backed into a corner, we better brace for whatever are thrown at us."
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
            if (level - 1 < 0 || level - 1 >= briefs.Count) summaryBrief.text += "\n" + "N/A";
            else
                summaryBrief.text += "\n" + briefs[level - 1]; 

            string time = GlobalVar.convertTime(timeSpent);
            Debug.Log(time);
            timeRequired.text += time;
        }
    }

    void SetBriefText(string s) { summaryBrief.text = s; }
}
