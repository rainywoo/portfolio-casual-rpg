using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAnimEvent : MonoBehaviour
{
    public UnityEvent AttackStateEvent1 = null;
    public UnityEvent AttackStateEvent2 = null;
    public UnityEvent Attack = null;

    public void OnAttack()
    {
        Attack?.Invoke();
    }
    public void OnAttackStart()
    {
        AttackStateEvent1?.Invoke();
    }
    public void OnAttackExit()
    {
        AttackStateEvent2?.Invoke();
    }
}
