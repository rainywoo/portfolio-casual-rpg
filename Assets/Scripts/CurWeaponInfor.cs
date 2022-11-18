using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurWeaponInfor : MonoBehaviour
{
    public static CurWeaponInfor Inst;

    public int WeaponNumber;
    public int curAmmo;
    public int maxAmmo;
    // Start is called before the first frame update

    private void Awake()
    {
        Inst = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SceneManager.Inst.curAmmo.text = curAmmo.ToString();
        if(WeaponNumber == 0)
        {
            SceneManager.Inst.maxAmmo.text = "¡Ä";
        }
        else SceneManager.Inst.maxAmmo.text = maxAmmo.ToString();
    }
}
