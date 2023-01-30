using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStart : MonoBehaviour
{
    // Start is called before the first frame update
    public string lastExitName;
    void Start()
    {
        if(PlayerPrefs.GetString("LastExitName") == lastExitName)
        {
            Player.Inst.transform.position = transform.position;
            Player.Inst.transform.eulerAngles = transform.eulerAngles;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
