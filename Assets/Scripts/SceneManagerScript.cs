using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript Inst;
    public Transform myHpBar;
    public Transform HpBars;
    public Transform AmmoText;
    public TMPro.TMP_Text curAmmo;
    public TMPro.TMP_Text maxAmmo;
    public Transform GrenadeText;
    public TMPro.TMP_Text GrenadeAmmo;
    public Transform PotionText;
    public TMPro.TMP_Text PotionAmmo;
    public Transform closeGre;
    public Transform closePot;
    // Start is called before the first frame update
    void Awake()
    {
        Inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
