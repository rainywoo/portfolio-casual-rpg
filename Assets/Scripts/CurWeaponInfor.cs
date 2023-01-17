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

    public bool isWeapon;
    public bool isGrenade;
    public bool isPotion;

    public int curGrenade;
    public int curPotion;
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
        if (isWeapon)
        {
            SceneManagerScript.Inst.AmmoText.gameObject.SetActive(true);
            SceneManagerScript.Inst.curAmmo.text = curAmmo.ToString();
            if (WeaponNumber == 0)
            {
                SceneManagerScript.Inst.maxAmmo.text = "¡Ä";
            }
            else SceneManagerScript.Inst.maxAmmo.text = maxAmmo.ToString();
        }
        else SceneManagerScript.Inst.AmmoText.gameObject.SetActive(false);

        if (isGrenade)
        {
            SceneManagerScript.Inst.GrenadeText.gameObject.SetActive(true);
            SceneManagerScript.Inst.GrenadeAmmo.text = curGrenade.ToString();
        }
        else
        {
            SceneManagerScript.Inst.GrenadeText.gameObject.SetActive(false);
        }
        if (isPotion)
        {
            SceneManagerScript.Inst.PotionText.gameObject.SetActive(true);
            SceneManagerScript.Inst.PotionAmmo.text = curPotion.ToString();
        }
        else
        {
            SceneManagerScript.Inst.PotionText.gameObject.SetActive(false);
        }

        if (curGrenade <= 0) SceneManagerScript.Inst.closeGre.gameObject.SetActive(true);
        else SceneManagerScript.Inst.closeGre.gameObject.SetActive(false);
        if (curPotion <= 0) SceneManagerScript.Inst.closePot.gameObject.SetActive(true);
        else SceneManagerScript.Inst.closePot.gameObject.SetActive(false);
    }
}
