using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : MonoBehaviour
{
    [SerializeField] EnemyData myData = default;
    [field: SerializeField] public float CurHp { get; private set; }
    [field: SerializeField] public float RunSpeed { get; private set; }
    [field: SerializeField] public float WalkSpeed { get; private set; }
    [field: SerializeField] public float Power { get; private set; }
    [field: SerializeField] public float CurMp { get; private set; }
    [field: SerializeField] public float Defense { get; private set; }
    [field: SerializeField] public int Gold { get; private set; }
    [field: SerializeField] public float AttackRange { get; private set; }
    [field: SerializeField] public float AttackSpeed { get; private set; }

    private void Start()
    {
        
    }
    public void Initialize()
    {
        CurHp = myData.HP;
        RunSpeed = myData.runSpeed;
        WalkSpeed = myData.walkSpeed;
        Power = myData.power;
        CurMp = myData.MP;
        Defense = myData.Def;
        Gold = myData.gold;
        AttackRange = myData.AttackDist;
        AttackSpeed = myData.Attackspeed;
    }

    public void UpdateHp(float v)
    {
        CurHp = Mathf.Clamp(CurHp + v, 0, myData.HP);
    }

    public void UpdateMp(float v)
    {
        CurMp = Mathf.Clamp(CurMp + v, 0, myData.MP);
    }
}
