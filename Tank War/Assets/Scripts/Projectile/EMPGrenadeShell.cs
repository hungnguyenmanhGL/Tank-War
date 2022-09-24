using System.Collections;
using UnityEngine;

//the type of shell pass through obstacle and explode on impact with enemy only.
//The shell do not does anything but decide the explosion position.(check EMPExplosion class)
public class EMPGrenadeShell : Bullet
{
    [SerializeField]
    GameObject empAOE;

    [SerializeField]
    bool AOE = false;

    // Use this for initialization
    void Start()
    {
        empAOE.SetActive(false);
        activeTime = 3f;
        AOE = true;
    }

    private void OnEnable()
    {
        StartCoroutine(disableSelf());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag(GlobalVar.eTag))
        {
            empAOE.transform.position = transform.position;
            empAOE.transform.rotation = transform.rotation;
            empAOE.SetActive(true);

            gameObject.SetActive(false);
        }
        
    }
}
