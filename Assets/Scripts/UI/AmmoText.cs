using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoText : MonoBehaviour
{
    // Start is called before the first frame update
    public TMPro.TMP_Text curAmmo;
    public TMPro.TMP_Text maxAmmo;
    public Player myTarget = null;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        curAmmo.text = myTarget.cur_number_Weapon.ToString();
        if (myTarget.number_Weapon == myTarget.FirstWeaponNumber)
            maxAmmo.text = "¡Ä".ToString();
        else maxAmmo.text = (myTarget.max_number_Weapon * myTarget.number_Ammo).ToString();
    }
}
