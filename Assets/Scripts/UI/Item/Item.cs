using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // Start is called before the first frame update
    public enum TYPE { Ammo, Coin, Grenade, Heart, Weapon };
    public TYPE myType;
    public int value;
    public float breakTime = 20.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * 35 * Time.deltaTime,Space.World);
        breakTime -= Time.deltaTime;
        if (breakTime <= 0.0f)
            Destroy(gameObject);
    }
}
