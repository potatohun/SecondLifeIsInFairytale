using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    public GameObject[] SobiList= new GameObject[4];
    public GameObject[] AccList= new GameObject[4];
    public GameObject[] SwordList = new GameObject[5];

    public GameObject Rollpaper;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 오브젝트를 씬 전환 시에도 파괴되지 않도록 설정
        }
        else
        {
            // 이미 인스턴스가 존재하면 중복 생성된 것이므로 이 오브젝트를 파괴
            Destroy(gameObject);
        }
    }

    public GameObject InstantiateItem(string type,string name)
    {
        GameObject tmp;

        switch (type)
        {
            case "sobi":
                for(int i=0;i<4;i++)
                {
                    if (name == SobiList[i].name) 
                    {
                        tmp = Instantiate(SobiList[i]);
                        tmp.name = SobiList[i].name;
                        return tmp;
                    }
                    
                }
                break;
            case "acc":
                for(int i=0;i<4;i++)
                {
                    if (name == AccList[i].name) 
                    {
                        tmp = Instantiate(AccList[i], Player.instance.transform.position, Quaternion.identity);
                        tmp.name = AccList[i].name;
                        return tmp;
                    }
                }
                break;
            case "sword":
                for(int i=0;i<5;i++)
                {
                    if (name == SwordList[i].name) 
                    {
                        tmp = Instantiate(SwordList[i], Player.instance.transform.position, Quaternion.identity);
                        tmp.name = SwordList[i].name;
                        return tmp;
                    }
                }
                break;
        }
        return null;
    }
    public GameObject DropItem(Vector3 pos)
    {
       int randomNumber2 = UnityEngine.Random.Range(1, 11);
       if(randomNumber2==0)return null;
       else 
       {
            int randomNumber = UnityEngine.Random.Range(1, 11);
            if(randomNumber<=4) return Instantiate(Rollpaper, pos,Quaternion.identity);
            else if (randomNumber>5&&randomNumber<=7) return Instantiate(SobiList[UnityEngine.Random.Range(0, SobiList.Length)],pos,Quaternion.identity);
            else if (randomNumber>7&&randomNumber<=9)return Instantiate(AccList[UnityEngine.Random.Range(0, AccList.Length-1)],pos,Quaternion.identity);
            else return Instantiate(SwordList[UnityEngine.Random.Range(0, SwordList.Length)],pos,Quaternion.identity);
       }
    }
}
