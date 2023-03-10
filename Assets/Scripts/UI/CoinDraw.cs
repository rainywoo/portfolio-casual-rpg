using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinDraw : MonoBehaviour
{
    public TMPro.TMP_Text coinText = null;
    int mycoin = 0;
    private void Update()
    {
        if (mycoin == 0)
            coinText.text = "0";
        if (mycoin != Player.Inst.myInfo.Coin)
        {
            mycoin = Player.Inst.myInfo.Coin;
            coinText.text = mycoin.ToString();
        }
    }
}
