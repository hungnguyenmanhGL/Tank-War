using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    GameObject mainMenuPanel;
    [SerializeField]
    GameObject levelSelectPanel;
    //can only get to this from levelSelectPanel
    [SerializeField]
    GameObject tankSelectPanel;

    [SerializeField]
    GameObject manualPanel;
    [SerializeField]
    GameObject settingPanel;

    [SerializeField]
    GameObject sharedBackButton;

    [SerializeField]
    List<GameObject> levelButtons;
    [SerializeField]
    List<GameObject> chooseTankButtonList;

    GameObject lastPanel;
    GameObject currentPanel;

    int level = -1;
    const int levelNumAllowedToChooseTank = 3;

    //prevent multi click leads to multi load
    bool levelLoading = false;
    // Start is called before the first frame update
    void Start()
    {
        levelLoading = false;
        mainMenuPanel.SetActive(true);
        currentPanel = mainMenuPanel;
        levelSelectPanel.SetActive(false);
        sharedBackButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //CheckNextSceneLoaded();
        //OnClickedToContinue();
    }

    public void OnCampaignButtonClicked()
    {
        currentPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
        sharedBackButton.SetActive(true);
        lastPanel = mainMenuPanel;
        currentPanel = levelSelectPanel;
    }

    public void GetTankSelectPanel()
    {
        tankSelectPanel.SetActive(true);
        lastPanel = levelSelectPanel;
        currentPanel = tankSelectPanel;
        sharedBackButton.SetActive(false);
    }

    public void OnLevelButtonClicked()
    {
        if (!levelLoading)
        {
          
            for (int i = 0; i < levelButtons.Count; i++)
            {
                if (levelButtons[i].name.Equals(EventSystem.current.currentSelectedGameObject.name)) level = i + 1;
            }
            Debug.Log(level);
            if (level < levelNumAllowedToChooseTank)
            {
                levelLoading = true;
                LoadingScreenController.instance.GetLoadingScreenForLevel(level - 1);
                LoadingScreenController.instance.SetLoadingOperation(SceneManager.LoadSceneAsync("Level " + level));
                LoadingScreenController.instance.SetDataLoadDone();
                PreLevelDataController.instance.DestroyLastValue();
            }
            else
            {
                GetTankSelectPanel();
            }
        }
    }

    public void OnChooseTankButtonClicked()
    {
        if (!levelLoading)
        {
            for (int i = 0; i < chooseTankButtonList.Count; i++)
            {
                if (EventSystem.current.currentSelectedGameObject.name.Equals(chooseTankButtonList[i].name))
                {
                    PreLevelDataController.instance.SetTankIndex(i);
                }
            }
            levelLoading = true;
            LoadingScreenController.instance.GetLoadingScreenForLevel(level - 1);
            LoadingScreenController.instance.SetLoadingOperation(SceneManager.LoadSceneAsync("Level " + level));
            PreLevelDataController.instance.CreateTankSetForLevel(level);
            LoadingScreenController.instance.SetDataLoadDone();
        }
    }

    public void OnManualButtonClicked()
    {
        sharedBackButton.SetActive(true);
        currentPanel.SetActive(false);
        manualPanel.SetActive(true);
        lastPanel = mainMenuPanel;
        currentPanel = manualPanel;
    }

    public void OnSettingButtonClicked()
    {
        sharedBackButton.SetActive(true);
        currentPanel.SetActive(false);
        settingPanel.SetActive(true);
        lastPanel = mainMenuPanel;
        currentPanel = settingPanel;
    }
    
    public void OnSharedBackButtonClicked()
    {
        if (levelLoading) return;
        lastPanel = null;
        currentPanel.SetActive(false);
        sharedBackButton.SetActive(false);
        mainMenuPanel.SetActive(true);
        currentPanel = mainMenuPanel;
    }
    public void OnExitButtonClicked()
    {
        Application.Quit();
    }

    //if player return from tank select to level select
    public void ReturnToPreviousPanel()
    {
        if (levelLoading) return;
        levelLoading = false;

        lastPanel.SetActive(true);
        sharedBackButton.SetActive(true);
        currentPanel.SetActive(false);
        currentPanel = lastPanel;
        lastPanel = null;
    }
}
