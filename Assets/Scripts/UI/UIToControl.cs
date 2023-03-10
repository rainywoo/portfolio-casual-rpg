using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToControl : MonoBehaviour
{
    public bool MouseLock = false;
    public GameObject[] myUI;
    public static UIToControl Inst;
    private void Awake()
    {
        Inst = this;
    }
    private void Update()
    {
        if (isActiveUI())
        {
            MouseLock = true;
        }
        else MouseLock = false;
        MouseEvent(MouseLock);
        ControlTime();
    }
    public bool isActiveUI()
    {
        for (int i = 0; i < myUI.Length; i++)
        {
            if (myUI[i].activeSelf)
            {
                return true;
            }
        }
        return false;
    }
    public void MouseEvent(bool isMouse)
    {
        Cursor.visible = isMouse;
        if (isMouse == true)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if (isMouse == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public void ControlTime()
    {
        if (MouseLock)
        {
            Time.timeScale = 0;
        }
        else Time.timeScale = 1;
    }
}
