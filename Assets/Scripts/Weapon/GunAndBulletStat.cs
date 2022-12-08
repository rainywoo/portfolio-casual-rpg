using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAndBulletStat : MonoBehaviour
{
    [SerializeField] WeaponData myData = default;
    [field: SerializeField] public float MoveSpeed { get; private set; }
    [field: SerializeField] public float Damage { get; private set; }
    [field: SerializeField] public float criticalChance { get; private set; }
    [field: SerializeField] public float AttackDelay { get; private set; }
    [field: SerializeField] public int Maxbullet { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        MoveSpeed = myData.Speed;
        Damage = myData.Damage;
        criticalChance = myData.Chance;
        AttackDelay = myData.Delay;
        Maxbullet = myData.Bullet;
        Weapons Wp = GetComponent<Weapons>();
        Wp.Initialize();
    }

    public void Initialize()
    {
        
    }
}
