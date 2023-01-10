using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAnimEvent : MonoBehaviour
{
    public UnityEvent AttackStateEvent1 = null;
    public UnityEvent AttackStateEvent2 = null;
    public UnityEvent Attack = null;
    public UnityEvent CameraShake = null;
    public UnityEvent LandCameraShake = null;
    public UnityEvent Shot1 = null;
    public UnityEvent Shot2 = null;

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
    public void LandingCameraShake()
    {
        LandCameraShake?.Invoke();
    }
    public void OnCameraShake()
    {
        CameraShake?.Invoke();
    }
    public void RightShot()
    {
        Shot1?.Invoke();
    }
    public void LeftShot()
    {
        Shot2?.Invoke();
    }
    
}
