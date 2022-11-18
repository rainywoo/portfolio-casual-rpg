using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider myHpBar = null;
    public Transform myTarget = null;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        myHpBar.value = myTarget.GetComponent<Player>().myInfo.CurHP / myTarget.GetComponent<Player>().myInfo.MaxHP;
    }
}
