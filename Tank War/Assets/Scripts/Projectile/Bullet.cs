using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GlobalVar.ammo ammoType;

    public int dmg;
    protected float activeTime = 3f;
    //if EMP shell
    public bool emp = false;
    public float empTime = 5f;
    public bool convert = false;

    protected Vector3 startingPoint;

    [SerializeField]
    protected GameObject explosion;
    
    [SerializeField]
    protected float maxRange = 30f;

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


    public void SetStartingPoint(Transform firingPoint) { startingPoint = firingPoint.position; }
    protected virtual void DisableAfterReachingMaxRange()
    {
        if (Vector2.Distance(transform.position, startingPoint) >= maxRange) gameObject.SetActive(false);
    }

    //the damage deal logic is handled by Tank.cs
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //handle explosion effect 
        if (explosion) { 
            GameObject e = ObjectPool.instance.GetObjectForType(explosion.name, false);
            e.transform.position = this.transform.position;
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
