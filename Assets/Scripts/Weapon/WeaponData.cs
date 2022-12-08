using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Scriptable Object/Weapon Data", order = -1)]
public class WeaponData : ScriptableObject
{
    [SerializeField] float MoveSpeed = 3.0f;
    [SerializeField] float AttackDelay = 1.0f;
    [SerializeField] float AttackDamage = 10.0f;
    [SerializeField] float criticalChance = 10.0f;
    [SerializeField] int MaxBullet = 60;
    public float Speed { get => MoveSpeed; }
    public float Delay { get => AttackDelay; }
    public float Damage { get => AttackDamage; }
    public float Chance { get => criticalChance; }
    public int Bullet { get => MaxBullet; }
}
