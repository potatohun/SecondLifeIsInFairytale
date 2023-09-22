using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterSpawner : MonoBehaviour
{
    //public static MonsterSpawner instance;

    public GameObject pozol;
    public GameObject arrow_pozol;
    public GameObject spawnPoint;
    public int howMany;

    public GameObject portal;
    public List<GameObject> monsters;

    public void Awake()
    {
        portal = GameObject.FindGameObjectWithTag("Portal");
        portal.gameObject.SetActive(false);
        howMany = Random.Range(2, 5);
    }
    public void Start()
    {
        monsters = new List<GameObject>();

        Debug.Log("몬스터스폰!");
        for(int i = 0; i < howMany; i++)
        {
            int n = Random.Range(1, 3);

            if (n == 1)
            {
                PozolSpawn(i);
            }
            else if (n == 2)
            {
                Arrow_PozolSpawn(i);
            }
        }
        Debug.Log(howMany);
    }

    public void Update()
    {
        Debug.Log(ListEmptyCheck());
    }

    public void PozolSpawn(int i)
    {
        GameObject monster = Instantiate(pozol, spawnPoint.transform.position + new Vector3(i, 0, 0), Quaternion.identity);
        //monster.transform.position = new Vector3(0, 0, 0);
        monster.SetActive(true);
        monsters.Add(monster);
    }
    public void Arrow_PozolSpawn(int i)
    {
        GameObject monster = Instantiate(arrow_pozol, spawnPoint.transform.position + new Vector3(i,0,0), Quaternion.identity);
        //monster.transform.position = new Vector3(3, 0, 0);
        monster.SetActive(true);
        monsters.Add(monster);
    }

    public bool ListEmptyCheck()
    {
        if (monsters.Any())
            return false;
        else
            return true;
    }
    public void ActivePortal()
    {
        portal.gameObject.SetActive(true);
    }
}
