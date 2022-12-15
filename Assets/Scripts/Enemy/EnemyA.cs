using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyA : Enemy , IBattle
{
    public Transform myHeadPos;

    public Transform HeadPos
    {
        get => myHeadPos;
    }
    public bool IsLive
    {
        get
        {
            if (myStat.CurHp > 0) return true;
            else return false;
        }
    }
    public Transform myTarget;
    [SerializeField] STATE myState = STATE.Create;
    [SerializeField] TYPE myType = TYPE.Normal;
    public EnemyStat myStat;
    public Transform myHitPos;

    public LayerMask myTargetLay = default;

    float playtime = 0.0f;

    [SerializeField] bool CanMove = true;

    Coroutine AtCo;
    // Start is called before the first frame update
    void Start()
    {
        myPath = new NavMeshPath();
        ChangeState(STATE.Alive);
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }
    void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;
        switch (s)
        {
            case STATE.Create:
                myAnim.SetBool("CanMove", true);
                break;
            case STATE.Alive:
                myAnim.SetBool("CanMove", true);
                myStat.Initialize();
                MoveSpeed = myStat.WalkSpeed;
                break;
            case STATE.Battle:
                myAnim.SetBool("CanMove", true);
                MoveSpeed = myStat.RunSpeed;
                break;
            case STATE.Death:
                StopAllCoroutines();
                myAnim.SetBool("CanMove", false);
                if (myAnim.GetBool("isMoving"))
                    myAnim.SetBool("isMoving", CanMove);
                StartCoroutine(OnDeath());
                break;
        }
    }
    void StateProcess()
    {
        CanMove = myAnim.GetBool("CanMove");
        switch (myState)
        {
            case STATE.Create:
                break;
            case STATE.Alive:
                break;
            case STATE.Battle:
                if (myTarget != null && CanMove && myTarget.GetComponent<Player>().IsLive)
                {
                    FollowPlayer(myTarget, myStat.AttackRange, OnAttack);
                }
                
                if (myStat.AttackSpeed > playtime)
                {
                    playtime += Time.deltaTime;
                }
                
                for (int i = 0; i < myPath.corners.Length - 1; ++i)
                {
                    Debug.DrawLine(myPath.corners[i], myPath.corners[i + 1], Color.red);
                }
                break;
            case STATE.Death:
                break;
        }
    }
    void OnAttack()
    {
        if (playtime >= myStat.AttackSpeed)
        {
            playtime = 0.0f;
            StartCoroutine(Attack());
        }
    }
    IEnumerator Attack()
    {
        myAnim.SetTrigger("Attack");
        while(myAnim.GetBool("isAttacking"))
        {
            yield return null;
        }
        yield return null;
    }
    
    public void OnDamage(float dmg)
    {
        if(IsLive)
        {
            myStat.UpdateHp(-dmg);
            StartCoroutine(BattleSystem.Damaging(myRenderer));
            if(myState != STATE.Battle)
            {
                ChangeState(STATE.Battle);
            }
            if(myStat.CurHp <= 0)
            {
                ChangeState(STATE.Death);
            }
        }
    }

    IEnumerator OnDeath()
    {
        myAnim.SetTrigger("Die");
        yield return new WaitForSeconds(4.0f);
        Destroy(gameObject);
    }

    public void AttackDamage()
    {
        Collider[] list = Physics.OverlapSphere(myHitPos.position, 2.0f, myTargetLay);
        foreach(Collider col in list)
        {
            IBattle ib = col.GetComponent<IBattle>();
            if(ib != null && ib.IsLive) ib.OnDamage(myStat.Power);
        }
    }

    public void AttackStart()
    {
        myAnim.SetBool("CanMove", false);
        myAnim.SetBool("isAttacking", true);
    }
    public void AttackExit()
    {
        myAnim.SetBool("CanMove", true);
        myAnim.SetBool("isAttacking", false);
    }
}
