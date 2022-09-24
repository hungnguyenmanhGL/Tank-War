using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Handle skill chosen by buttons/hotkeys//
//Handle UI for cooldown time
public class SkillUIController : MonoBehaviour
{
    public static SkillUIController instance;

    [SerializeField]
    public SuperTankSkillController skillController;
   
    [SerializeField]
    List<GameObject> skillButtons;
    [SerializeField]
    List<Text> skillNames;
    [SerializeField]
    List<GameObject> cooldownBars;

    //each cooldown bar has 1 cooldown text as child
    [SerializeField]
    List<Text> cooldownTexts = new List<Text>();

    int currentSkillChosen = -1;
    bool skillChosen = false;

    void Start()
    {
        OnStart();
    }

    void OnStart()
    {
        if (PreLevelDataController.instance && PreLevelDataController.instance.holdPlayer) 
            skillController = PreLevelDataController.instance.playerSkillController;

        cooldownTexts = new List<Text>();
        for (int i=0; i<skillButtons.Count; i++)
        {
            cooldownBars[i].SetActive(false);
            cooldownTexts.Add(cooldownBars[i].GetComponentInChildren<Text>());
        }

        for (int i=0; i<skillController.skills.Count; i++)
        {
            skillNames[i].text = GlobalVar.skillNameMap[skillController.skills[i].skillName];
        }

        if (skillController.skills.Count > skillButtons.Count) { Debug.Log("Not enough skill button"); }
    }

    public void OnFirstSkillButtonClicked()
    {
        if (!cooldownBars[0].activeInHierarchy)
        {
            if (currentSkillChosen == 0) { currentSkillChosen = -1; }
            else { currentSkillChosen = 0; }

            skillController.SetSkillReady(currentSkillChosen);
            SetChosenSkillText(currentSkillChosen);
        }
    }
    public void OnSecondSkillButtonClicked()
    {
        if (!cooldownBars[1].activeInHierarchy)
        {
            if (currentSkillChosen == 1) { currentSkillChosen = -1; }
            else { currentSkillChosen = 1; }

            skillController.SetSkillReady(currentSkillChosen);
            SetChosenSkillText(currentSkillChosen);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) { OnFirstSkillButtonClicked(); }
        if (Input.GetKeyDown(KeyCode.E)) { OnSecondSkillButtonClicked(); }

        //if skill chosen and activate -> set skill name to white
        if (currentSkillChosen >= 0 && currentSkillChosen < skillButtons.Count && Input.GetKeyDown(KeyCode.Mouse0))
        {
            SetChosenSkillText(-1);
            currentSkillChosen = -1;
        }

        SetSkillCooldownBar();
    }

    void SetChosenSkillText(int skillNum)
    {
        if (skillNum >= 0 && skillNum < skillNames.Count)
        {
            foreach (Text t in skillNames) t.color = Color.white;
            //Debug.Log("Yellow");
            skillNames[skillNum].color = Color.yellow;
        }
        else { foreach (Text t in skillNames) t.color = Color.white; }
    }

    void SetSkillCooldownBar()
    {
        for (int i=0; i<cooldownBars.Count; i++)
        {
            Skill skill = skillController.skills[i];
            if (cooldownBars[i].activeInHierarchy)
            {
                cooldownTexts[i].text = ((int)skill.timeTillReady) + "s";
                if (skill.ready)
                {
                    cooldownBars[i].SetActive(false);
                }
            }
            if (!cooldownBars[i].activeInHierarchy)
            {
                if (!skill.ready)
                {
                    cooldownBars[i].SetActive(true);
                    cooldownTexts[i].gameObject.SetActive(true);
                    cooldownTexts[i].text = ((int)skill.timeTillReady) + "s";
                }
            }
        }
    }
}
