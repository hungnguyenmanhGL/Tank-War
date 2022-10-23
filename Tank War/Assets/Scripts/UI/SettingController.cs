using System.Collections;
using System.Collections.Generic;
using System.Windows;
using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    public static SettingController instance;

    int defaultResolutionWidth = 1280; int defaultResolutionHeight = 800; 
    int defaultResolutionIndex = -1;
    int lastResolutionIndex = -1;
    int currentResolutionIndex = -1;
    List<Resolution> resolutionList;

    [SerializeField]
    GameObject resolutionSettingHolder;
    [SerializeField]
    Text defaultResolutionText;
    [SerializeField]
    List<Text> resolutionTextList = new List<Text>();

    [SerializeField]
    Toggle cameraModeToggle;
    [SerializeField]
    Toggle fullScreenToggle;
    

    void Start()
    {
        resolutionList = new List<Resolution>();
        for (int i=0; i<Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].width >= 800 && Screen.resolutions[i].width <= 1280)
            {
                resolutionList.Add(Screen.resolutions[i]);
            }
        }
        resolutionTextList.Clear();
        defaultResolutionText.gameObject.SetActive(false);

        for (int i=0; i<resolutionList.Count; i++)
        {
            Text option = Instantiate(defaultResolutionText,resolutionSettingHolder.transform);
            option.text = resolutionList[i].width + " x " + resolutionList[i].height + "@" + resolutionList[i].refreshRate;
            option.gameObject.SetActive(false);
            resolutionTextList.Add(option);

            if (resolutionList[i].width == defaultResolutionWidth && resolutionList[i].height == defaultResolutionHeight) defaultResolutionIndex = i;
        }
        lastResolutionIndex = defaultResolutionIndex;
        currentResolutionIndex = defaultResolutionIndex;
        resolutionTextList[defaultResolutionIndex].gameObject.SetActive(true);
        //if not in full screen mode -> keep same resolution
        if (Screen.fullScreenMode != FullScreenMode.ExclusiveFullScreen) ApplyResolution(currentResolutionIndex); 

        //set values for toggle options
        if (PreLevelDataController.instance)
        {
            cameraModeToggle.SetIsOnWithoutNotify(PreLevelDataController.instance.cameraFollowPlayer);
        }
        fullScreenToggle.SetIsOnWithoutNotify(Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen);

        Debug.Log(Screen.fullScreen);
        Debug.Log(Screen.fullScreenMode);
    }


    public void OnPreviousResolutionClicked()
    {
        currentResolutionIndex--;
        if (currentResolutionIndex < 0) currentResolutionIndex = 0;
        if (lastResolutionIndex >= 0 && lastResolutionIndex < resolutionList.Count) resolutionTextList[lastResolutionIndex].gameObject.SetActive(false);
        lastResolutionIndex = currentResolutionIndex;
        resolutionTextList[currentResolutionIndex].gameObject.SetActive(true);
        ApplyResolution(currentResolutionIndex); 
    }
    public void OnNextResolutionClicked()
    {
        currentResolutionIndex++;
        if (currentResolutionIndex > resolutionList.Count - 1) currentResolutionIndex = resolutionList.Count - 1;
        if (lastResolutionIndex >= 0 && lastResolutionIndex < resolutionList.Count) resolutionTextList[lastResolutionIndex].gameObject.SetActive(false);
        lastResolutionIndex = currentResolutionIndex;
        resolutionTextList[currentResolutionIndex].gameObject.SetActive(true);
        ApplyResolution(currentResolutionIndex);
    }
    public void ApplyResolution(int index)
    {
        Screen.SetResolution(resolutionList[index].width, resolutionList[index].height, Screen.fullScreen);
    }
    public void ToggleFullScreen()
    {
        if (Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        Debug.Log(Screen.fullScreen);
        Debug.Log(Screen.fullScreenMode);
        //Screen.SetResolution(resolutionList[currentResolutionIndex].width,
        //        resolutionList[currentResolutionIndex].height, Screen.fullScreenMode);
    }


    #region setting for camera
    public void ToggleCameraMode()
    {
        if (PreLevelDataController.instance) PreLevelDataController.instance.ChangeCameraMode();
    }
    #endregion

}
