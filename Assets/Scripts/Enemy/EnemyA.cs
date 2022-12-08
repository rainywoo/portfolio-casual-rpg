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
                break;
            case STATE.Alive:
                myStat.Initialize();
                MoveSpeed = myStat.WalkSpeed;
                break;
            case STATE.Battle:
                MoveSpeed = myStat.RunSpeed;
                break;
            case STATE.Death:
                StopAllCoroutines();
                if (myAnim.GetBool("isMoving"))
                    myAnim.SetBool("isMoving", false);
                StartCoroutine(OnDeath());
                break;
        }
    }
    void StateProcess()
    {
        switch (myState)
        {
            case STATE.Create:
                break;
            case STATE.Alive:
                break;
            case STATE.Battle:
                if (myTarget != null && Vector3.Distance(myTarget.position, transform.position) > myStat.AttackRange - 0.01f)
                {
                    FollowPlayer(myTarget, myStat.AttackRange);
                }
                float playtime = 0.0f;
                if (myStat.AttackSpeed > playtime)
                {
                    playtime += Time.deltaTime;
                }
                if (myTarget != null && Vector3.Distance(myTarget.position, transform.position) <= myStat.AttackRange)
                {
                    if (playtime >= myStat.AttackSpeed)
                    {
                        playtime = 0.0f;
                        StartCoroutine(Attack());
                    }
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
    IEnumerator Attack()
    {
        myAnim.SetTrigger("Attack");
        myAnim.SetBool("isAttacking", true);
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
}
