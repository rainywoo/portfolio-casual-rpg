using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimEvent : MonoBehaviour
{
    public UnityEvent Fire = null;
    public void OnFire()
    {
        Fire?.Invoke();
    }
}
