using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CharactorStat
{
    [SerializeField] float maxHP;
    public float MaxHP
    {
        get => maxHP;
        set => maxHP = value;
    }

    [SerializeField] float curHP;
    public float CurHP
    {
        get => curHP;
        set => curHP = Mathf.Clamp(value, 0.0f, maxHP);
    }
    [SerializeField] int maxLevel;
    public int MaxLevel
    {
        get => maxLevel;
        set => maxLevel = value;
    }
    [SerializeField] int curLevel;
    public int CurLevel
    {
        get => curLevel;
        set => curLevel = Mathf.Clamp(value, 0, maxLevel);
    }
    [SerializeField] int coin;
    public int Coin
    {
        get => coin;
        set => coin = value;
    }

    public CharactorStat(float hp, int level, int coin)
    {
        curHP = maxHP = hp;
        this.curLevel = 1;
        this.maxLevel = level;
        this.coin = coin;
    }
}