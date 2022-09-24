using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUpdate : MonoBehaviour
{
    GameObject player;
    ShootBullet turret;
    Repair repairComp;


    int shellType;
    public Text shellCountTxt;
    
    // Start is called before the first frame update
    void Start()
    {
        if (PreLevelDataController.instance && PreLevelDataController.instance.holdPlayer)
        {
            player = PreLevelDataController.instance.player;
            turret = PreLevelDataController.instance.playerAtkComp;
            repairComp = PreLevelDataController.instance.playerRepairComp;
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player");
            turret = player.GetComponentInChildren<ShootBullet>();
            repairComp = player.GetComponent<Repair>();
        }

        shellType = 0;
        if (shellCountTxt.name == "EMP") shellType = 1;
        if (shellCountTxt.name == "CONV") shellType = 2;

        if (shellCountTxt.name == "REPAIR") shellType = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (shellType >= 0) shellCountTxt.text = shellCountTxt.name + " " + turret.shellCount[shellType].ToString();
        else
        {
            shellCountTxt.text = repairComp.repairChargeCount.ToString();
        }
    }
}
