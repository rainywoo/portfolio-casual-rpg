using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAmmoText : MonoBehaviour
{
    public TMPro.TMP_Text curAmmo;
    public Player myTarget = null;
    // Start is called before the first frame update
    public enum TYPE { None, Grenade, Potion }
    public TYPE myType = TYPE.None;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(myType)
        {
            case TYPE.None:
                break;
            case TYPE.Grenade:
                curAmmo.text = myTarget.number_Grenade.ToString();
                break;
            case TYPE.Potion:
                curAmmo.text = myTarget.number_Potion.ToString();
                break;
        }
    }
}
