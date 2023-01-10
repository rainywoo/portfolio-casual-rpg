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
            SceneManager.Inst.AmmoText.gameObject.SetActive(true);
            SceneManager.Inst.curAmmo.text = curAmmo.ToString();
            if (WeaponNumber == 0)
            {
                SceneManager.Inst.maxAmmo.text = "¡Ä";
            }
            else SceneManager.Inst.maxAmmo.text = maxAmmo.ToString();
        }
        else SceneManager.Inst.AmmoText.gameObject.SetActive(false);

        if (isGrenade)
        {
            SceneManager.Inst.GrenadeText.gameObject.SetActive(true);
            SceneManager.Inst.GrenadeAmmo.text = curGrenade.ToString();
        }
        else
        {
            SceneManager.Inst.GrenadeText.gameObject.SetActive(false);
        }
        if (isPotion)
        {
            SceneManager.Inst.PotionText.gameObject.SetActive(true);
            SceneManager.Inst.PotionAmmo.text = curPotion.ToString();
        }
        else
        {
            SceneManager.Inst.PotionText.gameObject.SetActive(false);
        }

        if (curGrenade <= 0) SceneManager.Inst.closeGre.gameObject.SetActive(true);
        else SceneManager.Inst.closeGre.gameObject.SetActive(false);
        if (curPotion <= 0) SceneManager.Inst.closePot.gameObject.SetActive(true);
        else SceneManager.Inst.closePot.gameObject.SetActive(false);
    }
}
