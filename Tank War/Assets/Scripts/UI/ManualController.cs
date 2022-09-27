using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ManualController : MonoBehaviour
{
    public static ManualController instance;

    [SerializeField]
    GameObject manualPanel;
    [SerializeField]
    List<GameObject> tankButtonList;
    [SerializeField]
    GameObject tankDescriptionPanel;
    [SerializeField]
    List<GameObject> tankImageList;

    //set these 2 to duplicate OnStart() so that no need for text update, just set active when u need
    [SerializeField]
    GameObject tankName;
    [SerializeField]
    GameObject tankDescription;

    List<GameObject> tankNameList = new List<GameObject>();
    List<GameObject> tankDescriptionList = new List<GameObject>();
 
    void Start()
    {
        OnStart();
    }

    public void OnStart()
    {
        tankDescriptionPanel.SetActive(false);
        if (tankName && tankDescription)
        {
            for (int i=0; i<tankButtonList.Count; i++)
            {
                GameObject nameObj = Instantiate(tankName, tankDescriptionPanel.transform);
                Text name = nameObj.GetComponent<Text>();
                name.text = tankNameMap[i];
                nameObj.SetActive(false);
                tankNameList.Add(nameObj);

                GameObject desObj = Instantiate(tankDescription, tankDescriptionPanel.transform);
                Text des = desObj.GetComponent<Text>();
                des.text = tankDescriptionMap[i];
                desObj.SetActive(false);
                tankDescriptionList.Add(desObj);

                tankImageList[i].SetActive(false);
            }
            tankName.SetActive(false);
            tankDescription.SetActive(false);
        }
        else { Debug.Log("Objects to duplicate not found!"); }
    }

    public void OnTankManualButtonClicked()
    {
        tankDescriptionPanel.SetActive(true);
        for (int i=0; i<tankNameList.Count; i++)
        {
            tankImageList[i].SetActive(false);
            tankNameList[i].SetActive(false);
            tankDescriptionList[i].SetActive(false);
        }

        for (int i = 0; i < tankButtonList.Count; i++)
        {
            if (EventSystem.current.currentSelectedGameObject.name.Equals(tankButtonList[i].name))
            {
                tankImageList[i].SetActive(true);
                tankNameList[i].SetActive(true);
                tankDescriptionList[i].SetActive(true);
                break;
            }
        }
    }

    public void OnBackFromTankManualButtonClicked()
    {
        tankDescriptionPanel.SetActive(false);
        manualPanel.SetActive(true);
    }

    static Dictionary<int, string> tankNameMap = new Dictionary<int, string>
    {
        {0, "RANGER" },
        {1, "PEACE BREAKER" },
        {2, "SIEGE BREAKER" },
        {3, "SCALE BREAKER" },
        {4, "BATTERING RAM" }
    };

    static Dictionary<int, string> tankDescriptionMap = new Dictionary<int, string>
    {
        {0, "The good old trusty Ranger Tank. Boasting an all-rounded performance, " +
            "this model has been the staple of our armored force. Mass-produced and easy to repair," +
            "this heavy tank has seen action in all types of scenarios without fail to prove its combat effectiveness.\n" +
            "Hull: Heavy\n" +
            "Weapon: Standard AT shell\n" +
            "Combat Modules: None." },

        {1, "An universal tank built for disruption and destruction. " +
            "Designed to re-capture & exceed the balanced efficiency, " +
            "the Peace Breaker turns out surprisingly better than we ever hoped for. " +
            "Thanks to the recent breakthroughs in composite armor and trajectory calculating system, it possesses firepower " +
            "rivalled by an artilery barrage without losing the integrity of the armored hull. " +
            "The only downside is its cost and unbelievable complexity.\n" +
            "Hull: Universal\n" +
            "Weapon: Double barrel AT shells\n" +
            "Combat Modules:\n" +
            "- EMP Bomb: Fire an EMP bombshell that explodes on contact, disable enemies in the explosion radius for 5s.\n" +
            "- All-out barrage: Bombard a set area for 3s. " +
                "The barrage can't be cancelled and the tank can't fire during its activation. Manual targeting\n" },

        {2, "A modified version of a rejected design, the Siege Breaker defines the function of a tank on modern battlefield. " +
            "Able to withstand heavy fire and even multiple missile hits, " +
            "this tank is meant to lead the charge under intense crossfire. " +
            "The energy shield and plasma cannon offer even more tactical advantage to its already impressive strength.\n" +
            "Hull: Universal\n" +
            "Weapon: Double barrel AT shells\n" +
            "Combat Modules:\n" +
            "- Energy Shield: Create a barrier in front of the tank, lasts for 9s. Immediate activation.\n" +
            "- Plasma Cannon: Release the energy from the engine and shield generator to fire a plasma-infused shell." },

        {3, "The Scale Breaker is more of a tank hunter. " +
            "Equipped with the biggest caliber of anti-tank cannon, it can penetrate any hull type with lethal accuracy. " +
            "Its armor however doesn't match others in Breaker prototype series, " +
            "as R&D had to reduce weight for its shell accelerating gun & missile battery in exchange for slightly higher speed. " +
            "Recommended engage tactic is to avoid direct confrontation, " +
            "strike from a distance as the reloading takes considerable amount of time.\n" +
            "Hull: Universal\n" +
            "Weapon: ACCEL AT gun with improved high velocity HEAT shell\n" +
            "Combat Modules:\n" +
            "- Quad Missiles: Fire 4 guided missiles towards the set target. " +
            "The missiles will deactivate if target is destroyed. Manual targeting.\n" +
            "- Reload Overdrive: Push the reload system to its limit, reduce time needed per reload to only 1s. " +
            "Immediate activation." },

        {4, "Comes from the same rejected design as the Siege Breaker, this heavy tank development take a diffirent direction. " +
            "" }
    };
}
