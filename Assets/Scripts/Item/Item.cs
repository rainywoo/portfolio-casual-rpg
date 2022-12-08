using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Start is called before the first frame update
    public enum TYPE { Ammo, Coin, Grenade, Heart, Weapon };
    public TYPE myType;
    public int value;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * 35 * Time.deltaTime,Space.World);
    }
}
