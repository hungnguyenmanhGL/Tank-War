using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootBullet : MonoBehaviour
{
    public enum Shell { NORM, EMP, CONV, ROCKET}

    //[SerializeField]
    //GameObject turret;

    [SerializeField]
    List<Transform> firingPoint;
    [SerializeField]
    List<GameObject> shellPrefabs;

    [SerializeField]
    PlayerTextBubbleController commController;
    

    Coroutine currentReloadCoroutine;

    public List<int> shellCount;

    [HideInInspector]
    public Shell shellType = Shell.NORM;
    private bool readyToFire = true;

    public float firingForce = 0.02f;
    public float reloadTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        //reloadText.enabled = false;
        //shellCount[(int)Shell.NORM] = 999;
        //shellCount[(int)Shell.EMP] = 15;
        //shellCount[(int)Shell.CONV] = 1;
    }

    public IEnumerator checkReload()
    {
        commController.WhenReload(reloadTime);
        yield return new WaitForSeconds(reloadTime);
        readyToFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return; 
            else Shoot();
        }
    }

    private void Shoot()
    {
        if (!readyToFire) return;

        string shellName = shellPrefabs[(int)shellType].name;
        int type = (int)shellType;
        //Debug.Log(shellName);

        if (shellType != Shell.ROCKET && shellCount[type] > 0)
        {
            foreach (Transform i in firingPoint)
            {
                GlobalVar.GetFiringFlash(i);
                GlobalVar.FireShell(shellName, i, firingForce);

                shellCount[(int)shellType]--;
            }

            readyToFire = false;
            currentReloadCoroutine = StartCoroutine(checkReload());
        }
        if (shellCount[type] <= 0) Debug.Log("Out of " + shellType + " ammunition");
    }

    //called when player click to change shell type -> have to reload again
    public void changeShell(int type) 
    {
        if (type > shellPrefabs.Count - 1)
        {
            Debug.LogWarning("Shell type invalid!");
        }
        else
        {
            shellType = (Shell)type;
            if (currentReloadCoroutine != null) StopCoroutine(currentReloadCoroutine);
            readyToFire = false;
            currentReloadCoroutine = StartCoroutine(checkReload());
        }
    }

    //void TurretFollowMouseDirection()
    //{
    //    if (turret)
    //    {
    //        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
    //        Vector3 lookPos = Camera.main.ScreenToWorldPoint(mousePos);
    //        lookPos = lookPos - turret.transform.position;
    //        float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;

    //        Quaternion to = Quaternion.Euler(turret.transform.position.x, turret.transform.position.y, angle - 90);
    //        //transform.rotation = Quaternion.Lerp(transform.rotation, to, rotateSpeed);
    //        turret.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    //    }
    //}
}