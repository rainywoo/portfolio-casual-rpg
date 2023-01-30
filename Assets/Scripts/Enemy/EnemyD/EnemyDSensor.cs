using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyDSensor : MonoBehaviour
{
    EnemyD myParent = null;
    public GameObject myParentObj = null;
    public LayerMask myTargetMask = default;
    // Start is called before the first frame update
    void Start()
    {
        myParent = myParentObj.GetComponent<EnemyD>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if ((myTargetMask & 1 << other.gameObject.layer) != 0)
        {
            Player ib = other.GetComponent<Player>();
            if (ib != null && ib.IsLive && myParent.myTarget == null)
            {
                myParent.myTarget = other.transform;
                if (myParent.isBattle() && myParent.IsLive)
                {
                    myParent.ChangeState(Enemy.STATE.Alive);
                }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if ((myTargetMask & 1 << other.gameObject.layer) != 0)
        {
            Player ib = other.GetComponent<Player>();
            if (ib != null && ib.IsLive)
            {
                myParent.myTarget = other.transform;
                if (myParent.isBattle() && myParent.IsLive)
                {
                    myParent.ChangeState(Enemy.STATE.Alive);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        
    }
}
