using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Useitem_Heal : MonoBehaviour
{
    // Start is called before the first frame update
    public enum POTION_TYPE
    {
        LOW, NORMAL, HIGH, GRAND, PERFECT
    }
    public float healAmount;
    public POTION_TYPE potionType = POTION_TYPE.LOW;
    public GameObject HealEffect = null;
    public int Value;
    void Start()
    {
        switch(potionType)
        {
            case POTION_TYPE.LOW:
                healAmount = 10;
                break;
            case POTION_TYPE.NORMAL:
                healAmount = 30;
                break;
            case POTION_TYPE.HIGH:
                healAmount = 50;
                break;
            case POTION_TYPE.GRAND:
                healAmount = 70;
                break;
            case POTION_TYPE.PERFECT:
                healAmount = 90;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
