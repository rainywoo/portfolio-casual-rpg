using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterProperty
{
    float MoveSpeed = 20.0f;
    float WalkSpeed;
    float RotateSpeed = 5.0f;

    float x;
    float y;

    bool isWalk;
    bool isRun;
    bool isWall;

    Ray fowRay = new Ray();
    Vector3 dir = Vector3.zero;

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
                InputProcess();

                if(!isWall) PlayerMove(x, y, dir);
                PlayerRotate(dir);

                myAnim.SetBool("isRun", isRun);
                myAnim.SetBool("isWalk", isWalk);
                break;
            case STATE.Death:
                break;
        }
    }

    void InputProcess()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        dir = new Vector3(x, 0.0f, y).normalized;
        if (dir.magnitude != 0) isRun = true;
        else isRun = false;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isWalk = true;
            MoveSpeed = WalkSpeed;
        }
        else
        {
            isWalk = false;
            MoveSpeed = WalkSpeed * 2;
        }
    }

    void PlayerMove(float x, float y, Vector3 dir)
    {
        transform.position += dir * Time.deltaTime * MoveSpeed;
    }

    void PlayerRotate(Vector3 dir)
    {
        if (dir.magnitude <= Mathf.Epsilon) return;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * MoveSpeed);
    }
}
