using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarControl : MonoBehaviour
{
    // Start is called before the first frame update
    private Tank hull;
    [SerializeField]
    public Slider hpBar;

    void Start()
    {
        if (PreLevelDataController.instance && PreLevelDataController.instance.holdPlayer)
        {
            hull = PreLevelDataController.instance.hull;
        }
        else hull = GameObject.FindGameObjectWithTag(GlobalVar.pTag).GetComponent<Tank>();
        
        if (!hpBar || !hull) this.enabled = false;

        if (hpBar && hull)
        {
            hpBar.maxValue = hull.maxHP;
            hpBar.value = hull.HP;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.value = hull.HP;
    }
}
