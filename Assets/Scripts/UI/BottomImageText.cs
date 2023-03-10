using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomImageText : MonoBehaviour
{
    // Start is called before the first frame update
    public Player myTarget;
    public GameObject GrenadeActive;
    public GameObject PotionActive;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (myTarget.number_Grenade > 0) GrenadeActive.SetActive(false);
        else GrenadeActive.SetActive(true);
        if (myTarget.number_Potion > 0) PotionActive.SetActive(false);
        else PotionActive.SetActive(true);
    }
}
