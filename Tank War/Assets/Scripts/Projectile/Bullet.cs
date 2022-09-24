using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum Shell { NORM, EMP, CONV}
    protected Shell shellType;

    public int dmg;
    public float activeTime = 2f;
    public bool explosive = true;

    //if EMP shell
    public bool emp = false;
    public float empTime = 5f;
    public bool convert = false;

    Vector3 startingPoint;
    [SerializeField]
    float maxRange = 30f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected IEnumerator disableSelf()
    {
        yield return new WaitForSeconds(activeTime);
        gameObject.SetActive(false);
    }

    

    // Update is called once per frame
    void Update()
    {
        DisableAfterReachingMaxRange();
    }

    private void OnEnable()
    {
        //StartCoroutine(disableSelf());
        //startingPoint = transform.position;
    }

    public void SetStartingPoint(Transform firingPoint) { startingPoint = firingPoint.position; }
    void DisableAfterReachingMaxRange()
    {
        if (Vector2.Distance(transform.position, startingPoint) >= maxRange) gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //handle explosion effect 
        if (explosive)
        {
            GameObject e = ObjectPool.instance.GetObjectForType("Explosion", false);
            e.transform.position = this.transform.position;
        }
        if (emp)
        {
            GameObject e = ObjectPool.instance.GetObjectForType("EMP Explosion", false);
            e.transform.position = transform.position;
        }

        if (collision.gameObject.CompareTag(GlobalVar.obsBlockShellTag))
        {
            gameObject.SetActive(false);
        }
        if (collision.gameObject.CompareTag(gameObject.tag))
        {
            gameObject.SetActive(false);
        }

        //handle collision with missile
        if (collision.gameObject.CompareTag("Missile")) { gameObject.SetActive(false); collision.gameObject.SetActive(false); }
    }

    public float GetMaxRange() { return maxRange; }
}