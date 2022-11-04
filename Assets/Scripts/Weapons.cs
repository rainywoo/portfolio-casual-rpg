using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public enum WEAPONTYPE { NULL, Weapon, Grenade, Potion };
    public WEAPONTYPE myType = WEAPONTYPE.NULL;

    public GameObject myBullet = null;
    public Transform myMuzzle = null;
    public Transform myBinMuzzle = null;
    public GameObject myBinBullet = null;

    public Weapons Inst;

    public float playTime = 0.0f;

    void Awake()
    {
        Inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(playTime < 0.5f)
        {
            playTime += Time.deltaTime;
        }   
    }

    public IEnumerator Shot()
    {
        if (playTime >= 0.5f && Input.GetMouseButton(0))
        {
            if (myType == WEAPONTYPE.Weapon)
            {
                Instantiate(myBullet, myMuzzle.position, myMuzzle.localRotation, null);
                yield return null;

                Instantiate(myBinBullet, myBinMuzzle.position, myBinMuzzle.localRotation, null);
            }
        }
    }
}
