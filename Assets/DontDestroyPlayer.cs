using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyPlayer : MonoBehaviour
{
    public static DontDestroyPlayer Inst;
    private void Awake()
    {
        if (Inst != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Inst = this;
        }
        DontDestroyOnLoad(gameObject);
    }
}
