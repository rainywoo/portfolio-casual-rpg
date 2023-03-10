using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    public GameObject ESCUI;

    bool activeESCUI = false;
    // Start is called before the first frame update
    void Start()
    {
        ESCUI.SetActive(activeESCUI);
    }

    // Update is called once per frame
    void Update()
    {
        InputProcess();
    }

    void InputProcess()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            activeESCUI = !activeESCUI;
            ESCUI.SetActive(activeESCUI);
        }
    }
}
