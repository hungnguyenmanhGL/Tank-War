using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public enum Hull { Light, Medium, Heavy, Super}
    public Hull hullType;
    public static string SHELL_TAG = "Shell";
    public static int playerLayer = 6;
    public static int enemyLayer = 7;

    public int maxHP = 100;
    public int HP = 100;
    public int armor;

    [SerializeField]
    PlayerTextBubbleController commController;

    [SerializeField]
    List<MonoBehaviour> affectedByEMP;

    // Start is called before the first frame update
    void Start()
    {
        //if (gameObject.CompareTag(GlobalVar.pTag) || commController) isPlayer = true;
    }

    public IEnumerator hitByEMP(float time)
    {
        foreach (MonoBehaviour script in affectedByEMP) script.enabled = false;
        yield return new WaitForSeconds(time);
        foreach (MonoBehaviour script in affectedByEMP) script.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0)
        {
            explodeUponKilled();
            //if the tank get killed while affected by EMP -> component must be set active for next respawn
            ResetCompUponKilled();
            gameObject.SetActive(false);
        }
    }

    void ResetCompUponKilled()
    {
        foreach (MonoBehaviour script in affectedByEMP)
        {
            script.enabled = true;

            AutoMovement moveComp = script as AutoMovement;
            if (moveComp) moveComp.ResetUponKilled();

            EnemyAttack eAtk = script as EnemyAttack;
            AllyAttack aAtk = script as AllyAttack;
            if (eAtk) eAtk.ResetUponKilled();
            if (aAtk) aAtk.ResetUponKilled();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(SHELL_TAG))
        {
            int dmg = collision.gameObject.GetComponent<Bullet>().dmg;
            if (commController) commController.WhenGotHit();
            TakeDamage(dmg);

            Bullet b = collision.gameObject.GetComponent<Bullet>();
            if (b.emp)
            {
                if (hullType <= Hull.Heavy) StartCoroutine(hitByEMP(b.empTime));
                else StartCoroutine(hitByEMP(b.empTime / 1));
            }
            //replace enemy tank with ally tank (same tank model with different color and different component)
            if (b.convert)
            {
                if (hullType > Hull.Heavy)
                {
                    collision.gameObject.SetActive(false);
                    return; 
                }

                string tankName = "Converted Heavy Tank";
                if (this.hullType == Hull.Light)
                {
                    tankName = "Converted Light Tank";
                }
                if (this.hullType == Hull.Medium)
                {
                    tankName = "Converted Medium Tank";
                }

                GameObject tankReplacement = ObjectPool.instance.GetObjectForType(tankName, false);
                tankReplacement.transform.position = gameObject.transform.position;
                tankReplacement.transform.rotation = gameObject.transform.rotation;
                Tank t = tankReplacement.GetComponent<Tank>();
                t.HP = this.HP;
                AllyHolder.instance.allyList.Add(tankReplacement);

                EnemyPool.instance.killCount++;
                gameObject.SetActive(false);
            }

            collision.gameObject.SetActive(false);
        }

        //handle missile collision and - HP
        if (collision.gameObject.CompareTag("Missile"))
        {
            int dmg = collision.gameObject.GetComponent<GuidedMissile>().dmg;
            TakeDamage(dmg);

            collision.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        HP = maxHP;
    }

    public void TakeDamage(int dmg)
    {
        int damageToHull = dmg - armor;
        if (damageToHull < 0) damageToHull = 0;
        HP -= damageToHull;
    }

    void explodeUponKilled()
    {
        GameObject e = ObjectPool.instance.GetObjectForType("Tank Explosion", false);
        if (e != null) e.transform.position = gameObject.transform.position;
    }
}
