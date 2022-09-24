using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreLevelDataController : MonoBehaviour
{
    public static PreLevelDataController instance;

    [SerializeField]
    List<GameObject> tankSetList;

    Vector3 playerStartPoint;

    int tankIndex = -1;
    public bool holdPlayer = false;
    public GameObject tankSet;
    public GameObject player;
    public Tank hull;
    public PlayerMovement playerMoveComp;
    public ShootBullet playerAtkComp;
    public SuperTankSkillController playerSkillController;
    public Repair playerRepairComp;

    void Start()
    {
        DontDestroyOnLoad(this);
        if (!instance) instance = this;
        else Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateTankSetForLevel(int level)
    {
        if (tankIndex >= tankSetList.Count || tankIndex < 0) { Debug.Log("Wrong tank set index"); return; }
        else
        {
            DestroyLastValue();
            playerStartPoint = GlobalVar.playerSpawnMap[level];
            holdPlayer = true;
            tankSet = GameObject.Instantiate(tankSetList[tankIndex], playerStartPoint, Quaternion.AngleAxis(0,Vector3.forward));
            DontDestroyOnLoad(tankSet);
            foreach (Transform child in tankSet.transform)
            {
                if (child.gameObject.CompareTag(GlobalVar.pTag)) player = child.gameObject;
            }
            //Debug.Log(player.name);
            //get all needed component for scripts in level (they can get it from here)
            hull = player.GetComponent<Tank>();
            playerAtkComp = player.GetComponentInChildren<ShootBullet>();
            playerMoveComp = player.GetComponent<PlayerMovement>();
            playerSkillController = player.GetComponent<SuperTankSkillController>();
            playerRepairComp = player.GetComponent<Repair>();
        }
    }

    public void DestroyLastValue()
    {
        holdPlayer = false;
        Destroy(tankSet);
        Destroy(player);
        Destroy(hull);
        Destroy(playerAtkComp);
        Destroy(playerMoveComp);
        Destroy(playerSkillController);
        Destroy(playerRepairComp);
    }

    public void SetTankIndex(int index)
    {
        tankIndex = index;
    }
}
