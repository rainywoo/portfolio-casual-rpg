using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider myHpBar = null;
    public Player myTarget = null;
    public TMPro.TMP_Text myText = null;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        myHpBar.value = myTarget.myInfo.CurHP / myTarget.myInfo.MaxHP;
        myText.text = (myTarget.myInfo.CurHP.ToString()) + " / " + myTarget.myInfo.MaxHP.ToString().;
    }
}
