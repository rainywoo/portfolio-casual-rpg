using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "Scriptable Object/Enemy Data", order = -1)]
public class EnemyData : ScriptableObject
{
    [SerializeField] float MaxHp;
    [SerializeField] float RunSpeed;
    [SerializeField] float WalkSpeed;
    [SerializeField] float Power;
    [SerializeField] float MaxMp;
    [SerializeField] float defense;
    [SerializeField] int Gold;
    [SerializeField] float AttackDistance;
    [SerializeField] float AttackSpeed;
    [SerializeField] int level;

    public float HP { get => MaxHp; }
    public float runSpeed { get => RunSpeed; }
    public float walkSpeed { get => WalkSpeed; }
    public float power { get => Power; }
    public float MP { get => MaxMp; }
    public float Def { get => defense; }
    public int gold { get => Gold; }
    public float AttackDist { get => AttackDistance; }
    public float Attackspeed { get => AttackSpeed; }

    public int Level { get => level; }
}
