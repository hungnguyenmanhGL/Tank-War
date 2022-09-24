using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{
    protected int dmg = 60;
    protected bool EMP = false;

    float animTime = 0.5f;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(DisableAfterAnim());
    }

    IEnumerator DisableAfterAnim()
    {
        yield return new WaitForSeconds(animTime);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
