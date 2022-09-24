using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscortMovement : MonoBehaviour
{
    private bool reachedCheckPoint = false;
    List<EnemyAttack> atkComp;
   
    [HideInInspector]
    public bool escaped = false;
    [HideInInspector]
    public bool destroyed = false;
    //wall object collider2D component to ignore when convoy move into map
    public Collider2D bound;

    //the convoy
    public GameObject escortTarget;

    //escorting force follow escort target movement
    public List<GameObject> escortForce;

    //the predetermined path
    public FixedPath path;
    public Transform nextCheckPoint;

    public float speed;
    public int checkPointIndex = 0;

    // Use this for initialization
    void Start()
    {
        if (path.checkPoints.Count > 0) nextCheckPoint = path.checkPoints[checkPointIndex];

        atkComp = new List<EnemyAttack>();
        escortTarget.SetActive(false);
        atkComp.Add(escortTarget.GetComponent<EnemyAttack>());
        foreach(GameObject tank in escortForce)
        {
            tank.SetActive(false);
            atkComp.Add(tank.GetComponent<EnemyAttack>());
        }
        Debug.Log("Disable");
        setActiveBoundCollision(false);
    }

    private void OnEnable()
    {
        escortTarget.SetActive(true);
        foreach (GameObject tank in escortForce)
        {
            tank.SetActive(true);
        }
    }

    //private void OnDisable()
    //{
    //    escortTarget.SetActive(false);
    //    foreach (GameObject tank in escortForce)
    //    {
    //        tank.SetActive(false);
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        checkReachedCheckPoint();
        findNextCheckPoint();
        moveToNextCheckPoint();
        if (!escortTarget.activeInHierarchy && !escaped)
        {
            destroyed = true;
            escortTargetDown();
            setActiveBoundCollision(true);
        }
    }

    void checkReachedCheckPoint()
    {
        if (Vector2.Distance(escortTarget.transform.position, nextCheckPoint.position) < 0.1f)
        {
            //Debug.Log("Reached checkpoint " + checkPointIndex);
            reachedCheckPoint = true;
        }
    }

    void findNextCheckPoint()
    {
        if (reachedCheckPoint)
        {
            if (checkPointIndex == 0)
            {
                foreach (EnemyAttack attack in atkComp) attack.enabled = true;
            }

            checkPointIndex++;
            if (checkPointIndex < path.checkPoints.Count)
            {
                nextCheckPoint = path.checkPoints[checkPointIndex];
            }
            reachedCheckPoint = false;

            //if reached final check point -> escaped 
            if (checkPointIndex == path.checkPoints.Count)
            {
                escaped = true;
                escortTarget.SetActive(false);
                for (int i = 0; i < escortForce.Count; i++)
                {
                    escortForce[i].SetActive(false);
                }
                this.enabled = false;
            }
        }
    }

    void moveToNextCheckPoint()
    {
        if (nextCheckPoint)
        {
            Vector2 dirToDes = (nextCheckPoint.position - escortTarget.transform.position).normalized;
            Vector2 targetPos = escortTarget.transform.position;
            //rotate escort target based on direction
            float rotateAngle = Mathf.Atan2(dirToDes.y, dirToDes.x) * Mathf.Rad2Deg - 90;
            Quaternion to = Quaternion.Euler(0, 0, rotateAngle);
            escortTarget.transform.rotation = Quaternion.Lerp(escortTarget.transform.rotation, to, 1);
            //move the escort target 
            targetPos.x += dirToDes.x * Time.deltaTime * speed;
            targetPos.y += dirToDes.y * Time.deltaTime * speed;
            escortTarget.transform.position = targetPos;

            for (int i=0; i<escortForce.Count; i++)
            {
                Vector2 pos = escortForce[i].transform.position;
                pos.x += dirToDes.x * Time.deltaTime * speed;
                pos.y += dirToDes.y * Time.deltaTime * speed;
                escortForce[i].transform.position = pos;
                escortForce[i].transform.rotation = escortTarget.transform.rotation;
            }
        }
    }

    //if escort target down -> revert to normal movement behaviors
    public void escortTargetDown()
    {
        for (int i=0; i<escortForce.Count; i++)
        {
            EnemyMovement enemyMovement = escortForce[i].GetComponent<EnemyMovement>();
            enemyMovement.enabled = true;
        }
        this.enabled = false;
    }

    private void setActiveBoundCollision(bool active)
    {
        if (active)
        {
            for (int i = 0; i < escortForce.Count; i++)
            {
                Physics2D.IgnoreCollision(bound, escortForce[i].GetComponent<Collider2D>(), false);
            }
        }
        else
        {
            Physics2D.IgnoreCollision(bound, escortTarget.GetComponent<Collider2D>());
            for (int i = 0; i < escortForce.Count; i++)
            {
                Physics2D.IgnoreCollision(bound, escortForce[i].GetComponent<Collider2D>());
            }
        }
    }
}
