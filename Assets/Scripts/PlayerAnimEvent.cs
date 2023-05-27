using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimEvent : MonoBehaviour
{
    public UnityEvent Fire = null;
    public Transform leftFoot = null;
    public Transform rightFoot = null;
    public GameObject footEfftect = null;
    public void OnFire()
    {
        Fire?.Invoke();
    }
    public void LeftFootDust()
    {
        Instantiate(footEfftect, leftFoot.position, leftFoot.rotation);
    }

    public void RightFootDust()
    {
        Instantiate(footEfftect, rightFoot.position, rightFoot.rotation);
    }
}
