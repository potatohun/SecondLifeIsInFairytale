using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;

    void Awake()
    { instance = this; }

    public CardData[] cardData;
    // Start is called before the first frame update
    void Start()
    {

    }
}
