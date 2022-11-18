using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Inst;
    public Transform myHpBar;
    public Transform HpBars;
    public TMPro.TMP_Text curAmmo;
    public TMPro.TMP_Text maxAmmo;
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
