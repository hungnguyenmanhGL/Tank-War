using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//create and hold the player gameobject before level loading 
//script in level can get player components from here if not null
public class PreLevelDataController : MonoBehaviour
{
    public static PreLevelDataController instance;

    [SerializeField]
    List<GameObject> tankSetList;

    Vector3 playerStartPoint;

    int tankIndex = -1;
    [HideInInspector]
    public bool holdPlayer = false;
    public GameObject tankSet;
    public GameObject player;
    public Tank hull;
    public Turret turret;
    public PlayerMovement playerMoveComp;
    public ShootBullet playerAtkComp;
    public Collider2D playerCollider;
    public SuperTankSkillController playerSkillController;
    public Repair playerRepairComp;

    public bool cameraFollowPlayer = true;

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
            playerStartPoint = GlobalVar.playerSpawnMap[level];
            float angleToSpawnPlayer = playerStartPoint.z;
            playerStartPoint.z = 0;
            holdPlayer = true;
            tankSet = GameObject.Instantiate(tankSetList[tankIndex], 
                playerStartPoint, Quaternion.AngleAxis(angleToSpawnPlayer,Vector3.forward));
            DontDestroyOnLoad(tankSet);
            foreach (Transform child in tankSet.transform)
            {
                if (child.gameObject.CompareTag(GlobalVar.pTag)) player = child.gameObject;
            }
            //Debug.Log(player.name);
            //get all needed component for scripts in level (they can get it from here)
            GetPlayerComponents();
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

    public void DisablePlayerComp()
    {
        playerMoveComp.enabled = false;
        turret.enabled = false;
        playerAtkComp.enabled = false;
        playerSkillController.enabled = false;
        playerRepairComp.enabled = false;
    }
    IEnumerator DeactivatePlayerInCutScene(float time)
    {
        playerCollider.enabled = false;
        playerMoveComp.enabled = false;
        playerAtkComp.enabled = false;
        playerSkillController.enabled = false;
        yield return new WaitForSeconds(time);
        playerCollider.enabled = true;
        playerMoveComp.enabled = true;
        playerAtkComp.enabled = true;
        playerSkillController.enabled = true;
    }
    public void DeactivatePlayerInCutscene(float time)
    {
        if (tankSet)
        {
            StartCoroutine(DeactivatePlayerInCutScene(time));
        }
    }

    void GetPlayerComponents()
    {
        hull = player.GetComponent<Tank>();
        turret = player.GetComponentInChildren<Turret>();
        playerAtkComp = player.GetComponentInChildren<ShootBullet>();
        playerMoveComp = player.GetComponent<PlayerMovement>();
        playerCollider = player.GetComponent<Collider2D>();
        playerSkillController = player.GetComponent<SuperTankSkillController>();
        playerRepairComp = player.GetComponent<Repair>();
    }

    public void ChangeCameraMode()
    {
        //Debug.Log("Camera changed");
        cameraFollowPlayer = !cameraFollowPlayer;
    }

    public void SetTankIndex(int index)
    {
        tankIndex = index;
    }
}
