using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterProperty
{
    public Transform myCamera;
    
    float MoveSpeed = 20.0f;
    float WalkSpeed;
    float RotateSpeed = 5.0f;
    float jumpPower = 15.0f;
    float DodgeDistance;
    float DodgeSpeed = 40.0f;

    float x;
    float y;

    bool isWalk;
    bool isRun;
    bool isWall;
    bool isDodge;
    [SerializeField] bool isJump = false;

    Vector3 moveDir = Vector3.zero;

    public enum STATE
    {
        Create, Alive, Death
    }
    public STATE myState = STATE.Create;
    void Start()
    {
        myState = STATE.Alive;

        WalkSpeed = MoveSpeed / 2;
    }
    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, transform.forward * 5, Color.black);
        isWall = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }
    
    void ChangeState(STATE t)
    {
        if (myState == t) return;
        myState = t;
        switch (t)
        {
            case STATE.Create:
                break;
            case STATE.Alive:
                break;
            case STATE.Death:
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
                InputProcess(); //Input 기능들 모음


                if(!isWall && !isDodge) PlayerMove(x, y, moveDir); //벽에 부딪치면 못움직이게 , 움직이는 코드
                PlayerRotate(moveDir); //캐릭터 회전

                myAnim.SetBool("isRun", isRun);
                myAnim.SetBool("isWalk", isWalk);
                myAnim.SetBool("isDodge", isDodge);
                break;
            case STATE.Death:
                break;
        }
    }

    void InputProcess()
    {
        {   //움직일 방향의 벡터 구하기
            Vector2 Moveinput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            isRun = Moveinput.magnitude != 0;
            Vector3 lookForward = new Vector3(myCamera.forward.x, 0f, myCamera.forward.z).normalized;
            Vector3 lookRight = new Vector3(myCamera.right.x, 0f, myCamera.right.z).normalized;
            moveDir = lookForward * Moveinput.y + lookRight * Moveinput.x;
        }
        if (Input.GetKey(KeyCode.LeftShift)) //L쉬프트 누르면 걷기
        {
            isWalk = true;
            MoveSpeed = WalkSpeed;
        }
        else //떼면 다시 달리기
        {
            isWalk = false;
            MoveSpeed = WalkSpeed * 2;
        }
        if(!isJump && Input.GetKeyDown(KeyCode.Space)) Jump(); //점프
        if (!myAnim.GetBool("isDodge") && !isJump && Input.GetKeyDown(KeyCode.LeftControl)) StartCoroutine(Dodge());
    }

    void PlayerMove(float x, float y, Vector3 dir)
    {
        if (isRun)
        {
            transform.parent.position += moveDir * MoveSpeed * Time.deltaTime; //구한 벡터로 움직임
        }
    }

    void PlayerRotate(Vector3 dir)
    {
        if (!isRun) return;
        myRigid.rotation = Quaternion.Slerp(myRigid.rotation, Quaternion.LookRotation(dir), Time.deltaTime * MoveSpeed);
    }
    void Jump()
    {
        myRigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        myAnim.SetTrigger("Jump");
        myAnim.SetBool("isJump", true);
        isJump = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == ("CanJump")) //점프 가능구역 밟으면 다시 점프 가능
        {
            myAnim.SetBool("isJump", false);
            isJump = false;
        }
    }
    IEnumerator Dodge()
    {
        isDodge = true;
        myAnim.SetTrigger("Dodge");
        DodgeDistance = 20.0f;
        while (DodgeDistance > Mathf.Epsilon)
        {
            float delta = Time.deltaTime * DodgeSpeed;
            if(delta >= DodgeDistance)
            {
                delta = DodgeDistance;
            }
            DodgeDistance -= delta;
            if(!isWall) transform.Translate(transform.forward * delta, Space.World);
            yield return null;
        }
        isDodge = false;
    }
}
