using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sensor : MonoBehaviour
{
    EnemyA myParent = null;
    public GameObject myParentObj = null;
    public LayerMask myTargetMask = default;
    [SerializeField] static int Target;
    // Start is called before the first frame update
    void Start()
    {
        myParent = myParentObj.GetComponent<EnemyA>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if((myTargetMask & 1 << other.gameObject.layer) != 0)
        {
            Player ib = other.GetComponent<Player>();
            if(ib!=null && ib.IsLive && myParent.myTarget == null)
            {
                Target = 1;
                myParent.myTarget = other.transform;
                if (Target >= 1 && myParent.Changable())
                {
                    myParent.ChangeState(Enemy.STATE.Battle);
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
                if (Target <= 0) Target = 1;
                if (Target >= 1 && myParent.Changable())
                {
                    myParent.ChangeState(Enemy.STATE.Battle);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if((myTargetMask & 1 << other.gameObject.layer) != 0)
        {
            Player ib = other.GetComponent<Player>();
            if (ib != null && ib.IsLive)
            {
                Target = 0;
                if (Target <= 0 && myParent.Changable())
                {
                    myParent.ChangeState(Enemy.STATE.Alive);
                }
                Debug.Log(Target);
            }
        }
    }
}
