using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyD : Enemy, IBattle
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

    public EnemyStat myStat;
    public Transform myHitPos;
    // Start is called before the first frame update
    [SerializeField] STATE myState = STATE.Create;
    [SerializeField] TYPE myType = TYPE.Boss;
    public Transform myTarget = null;
    public GameObject[] JomuraeGi = null;
    public Transform[] JomuSpawnPos = null;
    public GameObject myRock = null;
    public GameObject myMissile = null;
    public Transform[] MissileSpawnPos = new Transform[2];
    public Transform myRushAttackZone = null;
    public Transform myJumpAttackZone = null;

    bool CanStun = false;
    bool isStun = false;

    int DoPattern = 0;
    void Start()
    {
        ChangeState(STATE.Create);
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }

    public void StateProcess()
    {
        switch (myState)
        {
            case STATE.Create:
                break;
            case STATE.Alive:
                break;
            case STATE.Battle:
                break;
            case STATE.Death:
                break;
        }
    }

    public void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;
        switch (s)
        {
            case STATE.Create:
                break;
            case STATE.Alive:
                StopAllCoroutines();
                myStat.Initialize();
                StartCoroutine(StartSin());
                break;
            case STATE.Battle:
                StartCoroutine(Think());
                break;
            case STATE.Death:
                StopAllCoroutines();
                OnDeath();
                break;
        }
    }

    Coroutine coRot = null;
    IEnumerator Pattern()
    {
        if (coRot != null) StopCoroutine(coRot);
        coRot = StartCoroutine(Movement1.Rotating(transform, myTarget.position, RotSpeed));
        switch (DoPattern)
        {
            //case 1:
            //    yield return StartCoroutine(MissileAttackPattern());
            //    break;
            //case 2:
            //    yield return StartCoroutine(JomuSpawnPattern());
            //    break;
            //case 3:
            //    yield return StartCoroutine(JumpShotPattern());
            //    break;
            //case 4:
            //    yield return StartCoroutine(RushPattern());
            //    break;
            //case 5:
            //    yield return StartCoroutine(RockShotPattern());
            //    break;
            default:
                yield return StartCoroutine(RushPattern());
                break;
        }
        Debug.Log(DoPattern);
        yield return StartCoroutine(Think());
    }
    IEnumerator Think()
    {
        int number = Random.Range(1, 6);
        if (number == DoPattern)
        {
            for(int i = 0; i <= 1;)
            {
                number = Random.Range(1, 6);
                if (number == DoPattern) continue;
                else if (number != DoPattern) i += 1;
            }
        }
        yield return new WaitForSeconds(3.0f);
        DoPattern = number;
        Debug.Log(DoPattern);
        StartCoroutine(Pattern());
    }
    IEnumerator OnDeath()
    {
        myAnim.SetTrigger("Die");
        yield return new WaitForSeconds(4.0f);
        Destroy(gameObject);
    }
    IEnumerator StartSin()
    {
        myAnim.SetBool("Start", true);
        yield return StartCoroutine(StartSinBehavior());
        yield return new WaitForSeconds(2.0f);
        myAnim.SetTrigger("Howling"); //하울링 소리 넣기
        myAnim.SetBool("Start", false);
        yield return new WaitForSeconds(4.0f);
        ChangeState(STATE.Battle);
    }
    public void OnDamage(float dmg)
    {
        if (IsLive && myState != STATE.Create && myState != STATE.Alive)
        {
            if (!isStun) myStat.UpdateHp(-dmg);
            else if (isStun) myStat.UpdateHp(-dmg * 2.5f);
            StartCoroutine(BattleSystem.Damaging(myRenderer));
            if (myState != STATE.Battle)
            {
                ChangeState(STATE.Battle);
            }
            if (myStat.CurHp <= 0)
            {
                ChangeState(STATE.Death);
            }
        }
    }
    public void RockShot()
    {
        GameObject obj = Instantiate(myRock, myHitPos.position, myHitPos.rotation, null);
        obj.GetComponent<BossRock>().Initialize(myStat.Power);
    }
    public void MissileShot_Left()
    {
        GameObject obj = Instantiate(myMissile, MissileSpawnPos[1].position , transform.rotation, null);
        obj.GetComponent<Missile>().Initialize(myTarget, myStat.Power);
    }
    public void MissileShot_Right()
    {
        GameObject obj = Instantiate(myMissile, MissileSpawnPos[0].position, transform.rotation, null);
        obj.GetComponent<Missile>().Initialize(myTarget, myStat.Power);
    }
    public void LandingCameraShake()
    {
        CameraMove.Inst.CameraShake(2, 4);
    }
    public void CameraShake()
    {
        CameraMove.Inst.CameraShake(2, 20);
    }
    IEnumerator MissileAttackPattern()
    {
        myAnim.SetTrigger("Shot");
        yield return new WaitForSeconds(Random.Range(1,3));
    }
    IEnumerator RockShotPattern()
    {
        myAnim.SetTrigger("BigShot");
        yield return new WaitForSeconds(Random.Range(2, 4));
    }
    IEnumerator JomuSpawnPattern()
    {
        yield return new WaitForSeconds(1.0f);
        myAnim.SetBool("isHowling", true);
        yield return new WaitForSeconds(2.0f);
        myAnim.SetTrigger("Howling");
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < JomuSpawnPos.Length; i++)
        {
            int a = Random.Range(0, 15);
            GameObject obj;
            switch(a)
            {
                case 12:
                case 13:
                    obj = JomuraeGi[2]; //미사일 몹
                    break;
                case 14:
                    obj = JomuraeGi[1]; //유도미사일 몹
                    break;
                default:
                    obj = JomuraeGi[0]; //근접 잡몹
                    break;
            }
            Instantiate(obj, JomuSpawnPos[i].position, transform.rotation, null);
            yield return null;
        }
        myAnim.SetBool("isHowling", false);
    }
    IEnumerator JumpShotPattern()
    {
        yield return StartCoroutine(JumpShot(myTarget.position));
        yield return new WaitForSeconds(3.0f);
        yield return StartCoroutine(JumpShot(myTarget.position));
        yield return new WaitForSeconds(3.0f);
        yield return StartCoroutine(JumpShot(myTarget.position));
        yield return new WaitForSeconds(2.0f);
        myAnim.SetBool("ReadyToJump", false);
    }
    IEnumerator JumpShot(Vector3 target)
    {
        if (coRot != null) StopCoroutine(coRot);
        coRot = StartCoroutine(Movement1.Rotating(transform, myTarget.position, RotSpeed));
        myAnim.SetBool("ReadyToJump", true);
        yield return new WaitForSeconds(1.0f);
        myAnim.SetTrigger("Taunt");
        myCollider.isTrigger = true;
        float lerpTime = 0.5f;
        float currentTime = 0.0f;

        JumpAttackZone mJAZ = myJumpAttackZone.GetComponent<JumpAttackZone>();

        Vector3 EndPos = target;
        EndPos.y += 15;
        while (currentTime < lerpTime)
        {
            if(currentTime < lerpTime)
                currentTime += Time.deltaTime;
            if (currentTime > lerpTime)
                currentTime = lerpTime;
            transform.position = Vector3.Slerp(transform.position, EndPos, currentTime / lerpTime);
            myAnim.SetBool("isJumping", true);
            yield return null;
        }
        myAnim.SetBool("ReadyToJump", false);
        myRigid.AddForce(Vector3.down * 40, ForceMode.Impulse);
        myCollider.isTrigger = false;

        if (!mJAZ.gameObject.activeSelf) mJAZ.gameObject.SetActive(true);
        mJAZ.Initialize(myStat.Power * 2);

        yield return new WaitForSeconds(0.5f);
        mJAZ.gameObject.SetActive(false);
        //CameraMove.Inst.CameraShake(4, 4);
    }
    IEnumerator RushPattern() //3단 대쉬
    {
        int i = 0;

        yield return StartCoroutine(OnRush(i));
        i = 1;
        yield return StartCoroutine(OnRush(i));
        i = 2;
        yield return StartCoroutine(OnRush(i));
        i = 0;

        CanStun = false;
    }
    IEnumerator OnRush(int i) //대쉬 공격
    {
        myAnim.SetBool("ReadyToRush", true);
        Vector3 dir = myTarget.position - transform.position;
        float dist = dir.magnitude;
        dir.Normalize();

        if (coRot != null) StopCoroutine(coRot);
        coRot = StartCoroutine(Movement1.Rotating(transform, myTarget.position, 720.0f));
        yield return new WaitForSeconds(1.0f);
        myAnim.SetTrigger("Rush");

        RushAttackZone mRAZ = myRushAttackZone.GetComponent<RushAttackZone>();

        if (!mRAZ.gameObject.activeSelf) mRAZ.gameObject.SetActive(true);
        mRAZ.Initialize(myStat.Power * 2);

        while (dist > 0)
        {
            float delta = 300.0f * Time.deltaTime;
            if (delta > dist)
            {
                delta = dist;
            }
            dist -= delta;
            if (i == 2)
            {
                CanStun = true;
            }
            transform.Translate(dir * delta, Space.World);
            yield return null;
        }
        mRAZ.gameObject.SetActive(false);
        myAnim.SetBool("ReadyToRush", false);
        yield return new WaitForSeconds(2.0f);
    }
    IEnumerator StartSinBehavior() //등장씬 튀어 나와서 하울링
    {
        myRigid.AddForce(Vector3.up * 25, ForceMode.Impulse);
        Vector3 dir = Vector3.forward * 48;
        float dist = dir.magnitude;
        if (dist <= Mathf.Epsilon) yield break;
        dir.Normalize();
        myAnim.SetBool("isJumping", true);

        while (dist > 0.0f)
        {
            float delta = myStat.RunSpeed * Time.deltaTime;
            if (delta > dist)
            {
                delta = dist;
            }
            dist -= delta;
            transform.position += dir * delta;
            yield return null;
        }
    }
    public void DoStun() //외부 사용 전용 그로기
    {
        StopAllCoroutines();
        StartCoroutine(Stun());
    }
    IEnumerator Stun() //그로기
    {
        CanStun = false;
        if (myRushAttackZone.gameObject.activeSelf) myRushAttackZone.gameObject.SetActive(false);
        isStun = true;
        myAnim.SetBool("DoStun", true);
        CameraMove.Inst.CameraShake(5, 4);

        yield return new WaitForSeconds(10.0f);

        isStun = false;
        Debug.Log("스턴 해제");
        myAnim.SetBool("ReadyToRush", false);
        myAnim.SetBool("DoStun", false);
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(Think());
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (myAnim.GetBool("isJumping") && (collision.gameObject.layer == LayerMask.NameToLayer("Floor") || collision.gameObject.layer == LayerMask.NameToLayer("Wall")))
        {
            myAnim.SetBool("isJumping", false);
        }
        if(CanStun && collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            StopAllCoroutines();
            StartCoroutine(Stun());
        }
    }
}