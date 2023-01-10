using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC : EnemyA
{
    public GameObject myMissile = null;
    public Transform ShotPos;
    void Awake()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }
    public void OnShot()
    {
        GameObject obj = Instantiate(myMissile, ShotPos.position, ShotPos.rotation, null) as GameObject;
        obj.GetComponent<EnemyCMissile>().Initialize(myStat.Power, myTarget.GetComponent<Player>().myHeadPos);
    }
}
