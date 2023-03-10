using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoActive : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject WeaponAmmoText;
    public GameObject GrenadeAmmoText;
    public GameObject PotionAmmoText;
    public Player myTarget;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (myTarget.isWeapon) WeaponAmmoText.SetActive(true);
        else WeaponAmmoText.SetActive(false);
        if (myTarget.isGrenade) GrenadeAmmoText.SetActive(true);
        else GrenadeAmmoText.SetActive(false);
        if (myTarget.isPotion) PotionAmmoText.SetActive(true);
        else PotionAmmoText.SetActive(false);
    }
}
