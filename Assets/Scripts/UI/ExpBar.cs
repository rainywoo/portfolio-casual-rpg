using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    public TMPro.TMP_Text myExpText = null;
    public Slider myExpSlider = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        myExpText.text = "Lv : " + Player.Inst.myInfo.CurLevel.ToString();
        myExpSlider.value = Player.Inst.myInfo.CurNeedExp / Player.Inst.myInfo.NeedExp;
    }
}
