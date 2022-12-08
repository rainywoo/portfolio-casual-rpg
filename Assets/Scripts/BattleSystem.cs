using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattle
{
    Transform HeadPos { get; }
    Transform transform { get; }
    void OnDamage(float dmg);
    bool IsLive
    {
        get;
    }
}

public class BattleSystem : MonoBehaviour
{
    public static IEnumerator Damaging(Renderer myRen)
    {
        myRen.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        myRen.material.color = Color.white;
    }
}
