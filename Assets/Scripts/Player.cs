using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterProperty
{
    public Transform myCamera;
    public GameObject[] myWeaponlist;
    public GameObject myCurWeapon = null;
    public GameObject[] myGrenade = null;
    public GameObject myCurGrenade = null;
    public GameObject[] myPotion = null;
    public GameObject myCurPotion = null;

    [SerializeField] float myMoney = 0.0f;
    
    float MoveSpeed = 20.0f;
    float WalkSpeed;
    float RotateSpeed = 5.0f;
    float jumpPower = 15.0f;
    float DodgeDistance;
    float DodgeSpeed = 40.0f;

    float x;
    float y;

    [SerializeField] bool isJump = false;
    bool isWalk;
    bool isRun;
    bool isWall;
    bool isDodge;

    [SerializeField] int number_Grenade = 0;
    [SerializeField] int number_Potion = 0;
    [SerializeField] int number_Ammo = 0;

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
        if (!myAnim.GetBool("isDodge") && !myAnim.GetBool("isJump") && Input.GetKeyDown(KeyCode.LeftControl)) StartCoroutine(Dodge());

        if(!myAnim.GetBool("isDodge") && !myAnim.GetBool("isJump"))Grabitem();
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
    void Grabitem()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (myCurGrenade != null && myCurGrenade.activeSelf) myCurGrenade.SetActive(false);
            if (myCurPotion != null && myCurPotion.activeSelf) myCurPotion.SetActive(false);
            if(myCurWeapon != null && !myCurWeapon.activeSelf)
            {
                myCurWeapon.SetActive(true);
            }
            myAnim.SetTrigger("Swap");
        }

        if(myCurGrenade != null && myCurGrenade.activeSelf == false && number_Grenade > 0 && Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (myCurPotion != null && myCurPotion.activeSelf) myCurPotion.SetActive(false);
            if (myCurWeapon != null && myCurWeapon.activeSelf) myCurWeapon.SetActive(false);
            myCurGrenade.SetActive(true);
            myAnim.SetTrigger("Swap");
        }

        if (myCurPotion != null && myCurPotion.activeSelf == false && number_Potion > 0 && Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (myCurGrenade != null && myCurGrenade.activeSelf) myCurGrenade.SetActive(false);
            if (myCurWeapon != null && myCurWeapon.activeSelf) myCurWeapon.SetActive(false);
            myCurPotion.SetActive(true);
            myAnim.SetTrigger("Swap");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == ("CanJump")) //점프 가능구역 밟으면 다시 점프 가능
        {
            myAnim.SetBool("isJump", false);
            isJump = false;
        }

        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == ("Item"))
        {
            Item item = other.transform.GetComponent<Item>();
            switch (item.myType)
            {
                case Item.TYPE.Coin:
                    myMoney += item.value;
                    break;
                case Item.TYPE.Ammo:
                    number_Ammo += 1;
                    break;
                case Item.TYPE.Grenade:
                    if (myCurGrenade == null)
                    {
                        myCurGrenade = myGrenade[item.value];
                        number_Grenade = 1;
                    }
                    else
                    {
                        if (myCurGrenade.GetComponent<Item>().value == item.value)
                            number_Grenade += 1;
                        else
                        {
                            number_Grenade = 1;
                            myCurGrenade = myGrenade[item.value];
                        }
                    }
                    break;
                case Item.TYPE.Heart:
                    if (myCurPotion == null)
                    {
                        myCurPotion = myPotion[item.value];
                        number_Potion = 1;
                    }
                    else
                    {
                        if (myCurPotion.GetComponent<Item>().value == item.value)
                            number_Potion += 1;
                        else
                        {
                            number_Potion = 1;
                            myCurPotion = myPotion[item.value];
                        }
                    }
                    break;
            }
            Destroy(item.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.transform.tag == ("Weapon"))
        {
            Debug.Log(other.transform.GetComponent<Item>().value);

            if (Input.GetKey(KeyCode.E))
            {
                Item item = other.GetComponent<Item>();
                if (myCurWeapon != null)
                {
                    myCurWeapon.SetActive(false);
                }
                myCurWeapon = myWeaponlist[item.value];
                if (myCurGrenade != null && myCurGrenade.activeSelf) myCurGrenade.SetActive(false);
                if (myCurPotion != null && myCurPotion.activeSelf) myCurPotion.SetActive(false);
                myAnim.SetTrigger("Swap");
                myCurWeapon.SetActive(true);
                Destroy(item.gameObject);
            }
        }
    }
}
