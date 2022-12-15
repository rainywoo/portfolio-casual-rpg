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
            if(ib!=null && ib.IsLive)
            {
                Target++;
                if (Target >= 1 && myParent.Changable())
                {
                    myParentObj.GetComponent<EnemyA>().ChangeState(Enemy.STATE.Battle);
                }
                Debug.Log(Target);
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
                Target--;
                if (Target <= 0 && myParent.Changable())
                {
                    myParentObj.GetComponent<EnemyA>().ChangeState(Enemy.STATE.Alive);
                }
                Debug.Log(Target);
            }
        }
    }
}
