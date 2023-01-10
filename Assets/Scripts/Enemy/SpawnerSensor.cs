using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerSensor : MonoBehaviour
{
    Spawner myParent = null;
    public GameObject myParentObj = null;
    public LayerMask myTargetMask = default;
    // Start is called before the first frame update
    void Start()
    {
        myParent = myParentObj.GetComponent<Spawner>();
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
            if (ib != null)
            {
                if (ib.IsLive && myParent.Changable())
                {
                    myParent.ChangeState(Spawner.STATE.Spawn);
                }
                else if(!ib.IsLive)
                {
                    myParent.ChangeState(Spawner.STATE.Normal);
                }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if ((myTargetMask & 1 << other.gameObject.layer) != 0 && myParent.myState == Spawner.STATE.Normal)
        {
            Player ib = other.GetComponent<Player>();
            if (ib != null)
            {
                if (ib.IsLive && myParent.Changable())
                {
                    myParent.ChangeState(Spawner.STATE.Spawn);
                }
                else if (!ib.IsLive)
                {
                    myParent.ChangeState(Spawner.STATE.Normal);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((myTargetMask & 1 << other.gameObject.layer) != 0)
        {
            Player ib = other.GetComponent<Player>();
            if (ib != null)
            {
                if (ib.IsLive && !myParent.Changable())
                {
                    myParent.ChangeState(Spawner.STATE.Normal);
                }
            }
        }
    }
}
