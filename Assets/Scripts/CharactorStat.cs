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

    [SerializeField] float curNeedExp;
    public float CurNeedExp
    {
        get => curNeedExp;
        set => curNeedExp = value;
    }
    [SerializeField] float needExp;
    public float NeedExp
    {
        get => needExp;
        set => needExp = value;
    }
    [SerializeField] int coin;
    public int Coin
    {
        get => coin;
        set => coin = value;
    }

    public CharactorStat(float hp, int coin)
    {
        curHP = maxHP = hp;
        this.curLevel = 1;
        this.maxLevel = 110;
        this.curNeedExp = 0.0f;
        this.needExp = 20.0f;
        this.coin = coin;
    }
    public void LevelUp(int level)
    {
        if (curLevel >= maxLevel) return;
        curLevel++;
        CurNeedExp = 0.0f;
        needExp *= 1.2f;
        maxHP *= 1.2f;
        curHP = maxHP;
    }
}