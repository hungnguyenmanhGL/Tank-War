using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public enum tank { Light, Medium, Heavy, EMP, Laser};

    public static EnemyPool instance;
    [SerializeField]
    public GameObject[] enemyPrefabs;

    public List<GameObject>[] pooledEnemyArr;

    public int[] numToSpawn;

    //all active enemies are added here and removed when inactive/destroyed
    public List<GameObject> activeEnemies;
    private List<Tank> hullStatus;
    //keep track to check victory condition and result report
    public int killCount = 0;

    private void Awake()
    {
        instance = this;
    }
   
    void Start()
    {
        killCount = 0;

        pooledEnemyArr = new List<GameObject>[enemyPrefabs.Length];
        int i = 0; 
        foreach(GameObject ePrefab in enemyPrefabs)
        {
            pooledEnemyArr[i] = new List<GameObject>();
            int bufferNum = numToSpawn[i];
            for (int n=0; n<bufferNum; n++)
            {
                GameObject e = Instantiate(ePrefab);
                e.name = ePrefab.name;
                e.SetActive(false);
                pooledEnemyArr[i].Add(e);
            }
            i++;
        }
    }

    private void FixedUpdate()
    {
        removeKilled();
    }

    public GameObject getEnemyByNum(int type)
    {
        for (int i=0; i<pooledEnemyArr[type].Count; i++)
        {
            if (!pooledEnemyArr[type][i].activeInHierarchy)
            {
                pooledEnemyArr[type][i].SetActive(true);
                return pooledEnemyArr[type][i];
            }
        }
        //if none inactive left, make 1 new + add to arr
        GameObject e = Instantiate(enemyPrefabs[type]);
        pooledEnemyArr[type].Add(e);
        return e;
    }

    public GameObject getEnemyByName(string name)
    {
        return null;
    }

    public void removeKilled()
    {
        for (int i=0; i<activeEnemies.Count; i++)
        {
            if (!activeEnemies[i].activeInHierarchy)
            {
                //need optimize
                if(activeEnemies[i].GetComponent<Tank>().HP <= 0) killCount++;
                activeEnemies.RemoveAt(i);
            }
        }
    }

    //use this to remove enemy off bound or cutscene
    public void ForceRemove()
    {
        
    }
}
