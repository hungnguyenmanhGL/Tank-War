using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    GameObject player;
    Tank hull;
    ShootBullet turret;
    Repair repairComp;
    Color useColorIndicator = Color.yellow;

    [SerializeField]
    GameObject cutsceneCanvas;
    [SerializeField]
    Text cutsceneText;
    List<string> cutsceneDialog = new List<string>();

    [SerializeField]
    GameObject levelFinishedCanvas;
    [SerializeField]
    Text missionResult;
    [SerializeField]
    GameObject levelFinshedNextButton;

    [SerializeField]
    GameObject ingameMenu;
    bool loadingMainMenu = false;

    [SerializeField]
    Canvas shellChangeCanvas;
    public List<GameObject> shellButtons;
    public List<Text> shellText;

    [SerializeField]
    Canvas skillCanvas;
    //[SerializeField]
    //List<GameObject> skillButtons;
    //[SerializeField]
    //List<Text> skillText;

    [SerializeField]
    GameObject pauseButton;

    [SerializeField]
    Text ingameDialog;

    public bool skillCanvasNeeded = false;
    public bool shellCanvasNeeded = true;

    // Start is called before the first frame update
    void Start()
    {
        if (!instance) instance = this;
        else Destroy(this);

        ingameMenu.SetActive(false);

        levelFinishedCanvas.SetActive(false);

        if (shellChangeCanvas && shellCanvasNeeded)
        {
            shellChangeCanvas.gameObject.SetActive(true);
            for (int i=0; i<shellButtons.Count; i++)
            {
                shellButtons[i].SetActive(true);
                shellText[i].gameObject.SetActive(true);
            }
        }
        else { shellChangeCanvas.gameObject.SetActive(false); }

        if (skillCanvasNeeded & skillCanvas)
        {
            skillCanvas.gameObject.SetActive(true);
        }
        else skillCanvas.gameObject.SetActive(false);

        if (PreLevelDataController.instance && PreLevelDataController.instance.holdPlayer)
        {
            player = PreLevelDataController.instance.player;
            hull = PreLevelDataController.instance.hull;
            turret = PreLevelDataController.instance.playerAtkComp;
            repairComp = PreLevelDataController.instance.playerRepairComp;
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player");
            hull = player.GetComponent<Tank>();
            turret = player.GetComponentInChildren<ShootBullet>();
            repairComp = player.GetComponent<Repair>();
        }
        if (!player)
        {
            Debug.LogError("No player found on " + gameObject.name);
        }

        shellText[0].color = useColorIndicator;
    }


    IEnumerator DisableUIComp(GameObject ui, float time)
    {
        yield return new WaitForSeconds(time);
        ui.SetActive(false);
    }

    public void GetLevelFinishedCanvas(bool win)
    {
        if (!win) missionResult.text = "MISSION FAILURE";
        if (pauseButton) pauseButton.SetActive(false);
        levelFinishedCanvas.SetActive(true);
        if (levelFinshedNextButton) levelFinshedNextButton.SetActive(true);
    }

    #region in-level cutscene and change cutscene text
    public void GetCutsceneCanvas(List<string> dialog)
    {
        cutsceneCanvas.SetActive(true);
        
        Time.timeScale = 0;
        LoadCutsceneText(dialog);
        //load first text
        OnCutsceneNextButtonClicked();
    }
    public void HideCutsceneCanvas()
    {
        Time.timeScale = 1;
        cutsceneDialog.Clear();
        cutsceneCanvas.SetActive(false);
    }
    public void LoadCutsceneText(List<string> dialog)
    {
        cutsceneDialog.Clear();
        cutsceneDialog.AddRange(dialog);
    }
    public void OnCutsceneNextButtonClicked()
    {
        if (cutsceneDialog.Count == 0)
        {
            HideCutsceneCanvas();
            return;
        }

        StringBuilder sb = new StringBuilder(cutsceneText.text);
        sb.Clear();
        sb.Append(cutsceneDialog[0]);
        cutsceneText.text = sb.ToString();
        cutsceneDialog.RemoveAt(0);
    }


    public void OnCutsceneSkipButtonClicked()
    {
        HideCutsceneCanvas();
    }
    #endregion

    public void SetIngameDialog(string text, float time)
    {
        //StringBuilder sb = new StringBuilder(ingameDialog.text);
        //sb.Clear();
        //sb.Append(text);
        if (text == null) { }
        else { ingameDialog.text = text; }

        ingameDialog.gameObject.SetActive(true);
        ingameDialog.enabled = true;
        StartCoroutine(DisableUIComp(ingameDialog.gameObject, time));
    }

    public void onShellButtonClicked()
    {
        string clickObj = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        Debug.Log(clickObj);
        int shellType = 0;
        if (clickObj == "EMP")
        {
            shellType = 1;
        }
        if (clickObj == "CONV")
        {
            shellType = 2;
        }
        //Debug.Log(shellType);

        foreach (Text t in shellText) t.color = Color.white;
        shellText[shellType].color = useColorIndicator;
        turret.changeShell(shellType);
    }

    //need work
    public void onRepairButtonClicked()
    {
        int repairHp = repairComp.repairHP;
        if (hull.HP >= hull.maxHP) return;

        if (repairComp.repairChargeCount > 0)
        {
            if (hull.HP + repairHp >= hull.maxHP) hull.HP = hull.maxHP;
            else hull.HP += repairHp;

            repairComp.repairChargeCount--;
        }
    }

    public void OnPauseButtonClicked()
    {
        Time.timeScale = 0;
        ingameMenu.SetActive(true);
    }
    public void OnAbortButtonClicked()
    {
        if (!loadingMainMenu && LoadingScreenController.instance)
        {
            loadingMainMenu = true;
            Time.timeScale = 1;

            
            LoadingScreenController.instance.GetLoadingScreenForLevel(-1);
            LoadingScreenController.instance.SetLoadingOperation(SceneManager.LoadSceneAsync("Main Menu"));
            LoadingScreenController.instance.SetDataLoadDone();
            //Destroy last used tank set
            PreLevelDataController.instance.DestroyLastValue();
            //else { SceneManager.LoadScene("Main Menu"); }
        }
    }
    public void OnContinueButtonClicked()
    {
        if (!loadingMainMenu)
        {
            Time.timeScale = 1;
            ingameMenu.SetActive(false);
        }
    }
}
