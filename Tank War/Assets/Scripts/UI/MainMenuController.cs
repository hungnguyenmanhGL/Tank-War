using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    GameObject sharedBackButton;

    [SerializeField]
    List<GameObject> levelButtons;
    [SerializeField]
    List<GameObject> chooseTankButtonList;

    GameObject currentPanel;

    int level = -1;
    const int levelNumAllowedToChooseTank = 4;

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
        currentPanel = levelSelectPanel;
    }

    public void GetTankSelectPanel()
    {
        tankSelectPanel.SetActive(true);
        sharedBackButton.SetActive(false);
    }

    public void OnLevelButtonClicked()
    {
        if (!levelLoading)
        {
            //levelLoading = true;
            string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
 
            for (int i = 0; i < levelButtons.Count; i++)
            {
                if (levelButtons[i].name == name) level = i + 1;
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
            string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
            for (int i = 0; i < chooseTankButtonList.Count; i++)
            {
                if (chooseTankButtonList[i].name == name)
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
    
    public void OnSharedBackButtonClicked()
    {
        if (levelLoading) return;
        currentPanel.SetActive(false);
        sharedBackButton.SetActive(false);
        mainMenuPanel.SetActive(true);
        currentPanel = mainMenuPanel;
    }
    public void OnExitButtonClicked()
    {
        Application.Quit();
    }
    public void ReturnToLevelSelectPanel()
    {
        if (levelLoading) return;
        levelLoading = false;
        tankSelectPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
        currentPanel = levelSelectPanel;
        sharedBackButton.SetActive(true);
    }
}
