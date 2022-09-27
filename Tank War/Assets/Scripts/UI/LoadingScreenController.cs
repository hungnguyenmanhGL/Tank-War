using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LoadingScreenController : MonoBehaviour
{
    public static LoadingScreenController instance;

    public AsyncOperation loadingOperation;

    [SerializeField]
    Animator animator;
    const float animTime = 1f;

    [SerializeField]
    GameObject loadingScreen;
    [SerializeField]
    Canvas loadingScreenCanvas;
    //Text for mission brief while loading level
    public Text brief;
    List<string> missionBrief = new List<string>();

    //Text indicating loading
    public Text loadText;
    string loading = "Loading...";
    string loaded = "Click to continue";

    [HideInInspector]
    public bool waitingForClick = false;
    [HideInInspector]
    public bool dataLoadDone = false;

    // Use this for initialization
    void Start()
    {
        if (!instance) instance = this;
        else
        {
            Destroy(this.gameObject);
            Destroy(this.loadingScreen);
            Destroy(this);
        }


        DontDestroyOnLoad(this);
        DontDestroyOnLoad(loadingScreen);

        loadingScreen.SetActive(false);
    }

    IEnumerator StartFadingAnimation(bool fadeIn)
    {
        if (fadeIn)
        {
            animator.SetTrigger("FadeIn");
            yield return new WaitForSeconds(animTime);
            if (loadingOperation != null) loadingOperation.allowSceneActivation = true;
        }
        else
        {
            //wait for too slow -> cause bug where loading screen doesn went up when the screen not hided before used again
            animator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(animTime);
            animator.SetTrigger("Exit");
            HideLoadingScreen();
        }
    }

    //Only work when loading operation not null
    void Update()
    {
        //Camera destroyed on new load -> set next scene main camera for replacement
        ChangeRenderCamera();

        WaitingForContinueClick();
        OnClickedToContinue();
    }

    public void SetLoadingOperation(AsyncOperation oper) { 
        loadingOperation = oper;  
    }

    //the original render camera is destroyed when load scene -> find new scene camera
    public void ChangeRenderCamera()
    {
        if (loadingScreenCanvas.worldCamera == null) 
            loadingScreenCanvas.worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    //called in the class that started scene loading - DOESNT CONTAIN LOADING LOGIC 
    public void GetLoadingScreenForLevel(int level)
    {
        //not going in this

        loadingScreen.SetActive(true);
        brief.text = briefTitle;
        if (level >= missionBriefArr.Length || level < 0)
        {
            brief.text = tips[Random.Range(0,tips.Length)];
        }
        else brief.text += missionBriefArr[level];
        loadText.text = loading;
    }

    //called right after SetLoadingOperation, the data load should be right after this  
    public void WaitingForContinueClick()
    {
        if (loadingOperation != null)
        {
            loadingOperation.allowSceneActivation = false;
            //loading operation is allowed activation after fading in anim completed in coroutine below
            StartCoroutine(StartFadingAnimation(true));
            if (loadingOperation.isDone && !waitingForClick)
            {
                //loadingOperation = null;
                waitingForClick = true;
                Time.timeScale = 0;
            }
        }
    }


    //called this when the data load is finished in class that handle the data loading operation
    public void SetDataLoadDone() { dataLoadDone = true; Debug.Log("Data loaded signal"); }

    public void OnClickedToContinue()
    {
        if (dataLoadDone) loadText.text = loaded;

        //if waiting for continue click and data load done -> allow continue click to be registered
        if (waitingForClick && dataLoadDone)
        {
            //Debug.Log("Waiting for click");
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("Clicked to continue");
                Time.timeScale = 1;

                StartCoroutine(StartFadingAnimation(false));
 
            }
        }
    }

    public void HideLoadingScreen()
    {
        loadingOperation = null;
        loadingScreen.SetActive(false);
        brief.text = "";
        loadText.text = loading;

        waitingForClick = false;
    }

    static string[] missionBriefArr =
    {
        "We have recently discovered a secret road opened by our enemy, thanks to our inside intel. " +
        "To avoid alerting us of the existence of such a path, " +
        "they have kept rather low presence near that area and only stationed a small number of guard. " +
        "We must take advantage of this to get behind their line and take them by surprise.",

        "Our scout reported sightings of multiple armed convoys transporting unknown cargo. " +
        "They are heading away from the frontline, heavily escorted, suggesting the package is more than just normal supply. " +
        "Your tank is the only unit capable of interception, given your current position. " +
        "Prepare an ambush and recover whatever you can.",

        "HQ has authorized a head-on armored assasult to break the gridlock between us and our enemy. " +
        "Delaying it means giving them more time to fortify their position, as they have already set up several " +
        "defensive infrastructure and auto-turrets across the battledfield." +
        "You will lead the charge of our second armored squad and support our already engaging forces. " +
        "Access to our latest developed weapons is granted, make good use of it. ",

        "To effectively remove the enemy's command chain, we must cut off their main command center and related facilities. " +
        "But first our priority is to capture the single bridge connecting to the main land, " +
        "and their static defense must be taken care of. " +
        "The bridge has yet to be barricaded meaning they plan to send their force to contest with us."
    };

    static string[] tips =
    {
        "Use WASD or Arrow keys to move, left-click to fire or activate your chosen skill. " +
            "You can also use Q & E hotkeys to select available skills.",

        "EMP - EMP \"Sparkling\" Type 1 shell is a recently fielded ammunition that explodes on contact, " +
            "releasing multiple EMP waves, temporarily disabling crucial battle systems. " +
            "Note by the dev team: Multiple hits of same shell type DOES NOT increase the duration.\n" +
            "CONV - Conversion \"Peace Priest\" Type S shell is an experimental weapon, " +
            "developed from the original EMP Type 1 shell. " +
            "While its detailed mechanism is classified by the AOE.Corp responsible for development, " +
            "the shell's main function is to seize and allow complete remote control of any vehicle it attached to.",

        "Super Tank - \"Peace Breaker\" Prototype Tank, designated MT77 is the latest combination of our and *acquired* technology. " +
           "Protected by the latest [REDACTED] composite hull, it is a moving fortress with multiple advanced firepower systems. " +
            "Designed to operate on diffirent terrains & situations, it can assume any roles on the battlefield." +
            "Due to the cost and complexity, only 1 prototype has been fielded up till now.",

        "Super Tank - \"Golden Ram\" V1 was a long discarded model, originally intended to be the first super tank. " +
            "Mainly borrowed from our opposition's design, the tank features an exoframe covered by thick layers of armor. " +
            "This, however, combined with a rather small hull & engine left no room for extra firepower due to overweight. " +
            "Thanks to our R&D team, changes are made for this juggernaut to finally see some action " +
            "without sacrificing any of its defense strength.",

        "Super Tank - \"Scale Break\" ",

          "Missile ignores obstacles and lock on until it reaches the target. " +
            "Shoot it down if you ever find yourself in its aim."
    };

    static string briefTitle = "Mission Brief:\n";
    
}
