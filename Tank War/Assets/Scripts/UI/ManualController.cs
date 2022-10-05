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
        manualPanel.SetActive(false);
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
        {0, "RANGER TANK T34" },
        {1, "PEACE BREAKER T1" },
        {2, "SIEGE BREAKER" },
        {3, "SCALE BREAKER" },
        {4, "WINSTON MK IV" },
        {5, "\"IMP\" M4" },
        {6, "\"ORCE\" MED5"},
        {7, "\"RHINO\" MARK I" },
        {8, "\"SCORPION\" SUPER TANK" },
        {9, "\"PEASHOOTER\" MARK II"  }
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
            "The Peace Breaker is incredibly promising despite being the first attempt to re-capture the Ranger Tank's balanced performance. " +
            "Thanks to the recent breakthroughs in composite armor and trajectory calculating system, it possesses firepower " +
            "rivalled by an artilery barrage without losing the integrity of the armored hull. " +
            "The only downside is its cost and unbelievable complexity.\n" +
            "Hull: Universal\n" +
            "Weapon: Double barrel AT shells\n" +
            "Combat Modules:\n" +
            "- EMP Bomb: Fire an EMP bombshell that explodes on contact, disable enemies in the explosion radius for 5s. Manual targeting.\n" +
            "- All-out barrage: Bombard a set area for 3s. " +
                "The barrage can't be cancelled and the tank can't fire during its activation. Manual targeting\n" },

        {2, "A modified version of a rejected design, the Siege Breaker defines the function of a tank on modern battlefield. " +
            "Able to withstand heavy fire and even multiple missile hits, " +
            "this tank is meant to lead the charge under intense crossfire. " +
            "The energy shield and plasma cannon offer even more tactical advantage to its already impressive strength.\n" +
            "Hull: Universal\n" +
            "Weapon: Double barrel AT shells\n" +
            "Combat Modules:\n" +
            "- Energy Shield: Create a barrier in front of the tank, lasts for 9s or destroyed. Immediate activation.\n" +
            "- Plasma Cannon: Release the energy from the engine and shield generator to fire a plasma-infused shell. " +
            "Can destroy enemy shells. Manual targeting." },

        {3, "The Scale Breaker is more of a tank hunter. " +
            "Armed with the high caliber anti-tank cannon, it can penetrate any hull type with lethal accuracy. " +
            "Its armor however doesn't match others in Breaker prototype series, " +
            "as weight has to be reduced for its shell accelerating gun & missile battery in exchange for higher speed. " +
            "Recommended engage tactic is to avoid direct confrontation, " +
            "strike from a distance as the reloading takes considerable amount of time.\n" +
            "Hull: Universal\n" +
            "Weapon: ACCEL AT gun with high velocity HEAT shell\n" +
            "Combat Modules:\n" +
            "- Quad Missiles: Fire 4 guided missiles towards the set target. " +
            "The missiles will deactivate themselves if target is destroyed. Manual targeting.\n" +
            "- Reload Overdrive: Push the reload system to its limit, reduce time per reload to less than 1s. The effect lasts for 15s. " +
            "Immediate activation." },

        {4, "Nicknamed \"Battering Ram\", " +
            "this heavy tank is the predecessor of the Siege Breaker, with front-focused armor and smaller gun reducing the cost & difficulty of mass-production. " +
            "The glaring weakness remains though is inadequate firepower compared to tanks of the same hull. " +
            "Therefore the army decided to install remote control on these models, " +
            "sending them in as cannon fodder. This tactic makes the most use out of its thick front armor and relatively low cost.\n" +
            "Hull: Heavy\n" +
            "Weapon: Standard AT shell\n" +
            "Combat Modules: None." },

        {5, "An outdated model brought back to action, this tank is refit with remote control and bigger gun to actually do some damage. " +
            "They always operate in squad, serving as scouts or disruptors. Only pose as a threat in large numbers.\n" +
            "Hull: Light\n" +
            "Weapon: Small-caliber AT shell\n" +
            "Combat Modules: None." },

        {6, "Another obsolete model. Better protection but still nothing against standard AT round. Operate similarly like the Imp.\n" +
            "Hull: Medium\n" +
            "Weapon: Small-caliber AT shell\n" +
            "Combat Modules: None." },

        {7, "The main rival of the Ranger, the Rhino has a heavy hull, thicker armor, fires standard AT round. " +
            "Speed and manuverability isn't its strength, so avoid outnumbered fight as its turret can put some dent on even universal hull.\n" +
            "Hull: Heavy\n" +
            "Weapon: Standard AT shell\n" +
            "Combat Modules: None." },

        {8, "The first iteration of the super tank series, this tank took the battefield by storm. " +
            "Even now a concentrated number of this model can be a menace to any of our operation, " +
            "meaning destroying them should be a priority. The double-barrel turret and auto missile battery won't make it easy either." +
            "This tank, like other universal models, shares the manufacture problem without specialized assembly line.\n" +
            "Hull: Universal\n" +
            "Weapon: Double barrel AT shell\n" +
            "Combat Modules: Auto-lock missile battery\n" +
            "Combat Tips: Shoot down the missiles, you can't outrun them" },

        {9, "A later version of the Rhino with serveral improvements, including repeat firing mechanism and better hull. " +
            "R&D is interested how such complexity fits in seemingly unchanged turret but capturing an intact model remains a challenge. " +
            "It is noted that its reload time is almost the same as the original models, making it a high priority target.\n" +
            "Hull: Heavy\n" +
            "Weapon: Standard AT shells. Repeating AT gun.\n" +
            "Combat Modules: None." }
    };
}
