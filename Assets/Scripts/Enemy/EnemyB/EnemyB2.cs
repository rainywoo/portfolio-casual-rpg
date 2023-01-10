using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyB2 : EnemyA
{
    // Start is called before the first frame update
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
        obj.GetComponent<EnemyBMissile>().Initialize(myStat.Power, myTarget.GetComponent<Player>().myHeadPos);
    }
}
