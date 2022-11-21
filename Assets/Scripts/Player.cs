using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterProperty
{
    public CharactorStat myInfo;
    public LayerMask ShotLay;

    public static Player Inst;

    public Transform myCamera;
    public GameObject[] myWeaponlist;
    public GameObject myCurWeapon = null;
    public GameObject[] myGrenade = null;
    public GameObject myCurGrenade = null;
    public GameObject[] myPotion = null;
    public GameObject myCurPotion = null;
    public Transform myHpBar = null;

    [SerializeField] float handRange = 1.0f;
    float MoveSpeed = 20.0f;
    float WalkSpeed;
    float RotateSpeed = 5.0f;
    float jumpPower = 15.0f;
    float DodgeDistance;
    float DodgeSpeed = 40.0f;

    float x;
    float y;

    public bool CanMeleeAttack = false;
    [SerializeField] bool isJump = false;
    bool isWalk;
    bool isRun;
    bool isWall;
    bool isDodge;
    [SerializeField] bool isReloading;
    [SerializeField] bool isAttacking;

    [SerializeField] int number_Grenade = 0;
    [SerializeField] int number_Potion = 0;
    [SerializeField] int number_Ammo = 0;
    [SerializeField] int number_Weapon = 0;
    int FirstWeaponNumber = 0;


    Vector3 moveDir = Vector3.zero;
    Vector3 lookForward = Vector3.zero;
    Vector3 ZoomPosVec = Vector3.zero;

    public enum STATE
    {
        Create, Alive, Death
    }
    public STATE myState = STATE.Create;
    private void Awake()
    {
        Inst = this;
    }
    void Start()
    {
        myState = STATE.Alive;
        number_Weapon = 0;
        myCurWeapon = myWeaponlist[number_Weapon];

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
        GiveWeaponInformaition();
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
                WeaponReset();
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
                WeaponReset();
                FindZoomVec();

                if (!isWall && !isDodge && !isAttacking) PlayerMove(x, y, moveDir); //벽에 부딪치면 못움직이게 , 움직이는 코드
                if (isAttacking)
                {
                    isRun = false;
                    isWalk = false;
                }
                if(!isAttacking) PlayerRotate(moveDir); //캐릭터 회전

                if(!isReloading && !isAttacking && myCurWeapon.activeSelf && myCurWeapon.GetComponent<Weapons>().Curbullet == 0) //재장전
                {
                    if(number_Ammo > 0 || number_Weapon == FirstWeaponNumber)
                    {
                        isReloading = true;
                        Reload();
                    }
                    else if (number_Ammo == 0 && number_Weapon != FirstWeaponNumber)
                    {
                        myCurWeapon.SetActive(false);
                        myCurWeapon = null;
                        WeaponReset();
                    }
                }

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
            lookForward = new Vector3(myCamera.forward.x, 0f, myCamera.forward.z).normalized;
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

        if (!myAnim.GetBool("isJump") && !myAnim.GetBool("isDodge"))
        {
            if(myCurWeapon.GetComponent<Weapons>().Curbullet <= 0)
            {
                isAttacking = false;
            }
            if (!isReloading && Input.GetMouseButton(0))
            {
                PlayerRotate(lookForward);
                if (myCurWeapon != null && myCurWeapon.GetComponent<Weapons>().Curbullet > 0 && myCurWeapon.activeSelf)
                {
                    switch (myCurWeapon.GetComponent<Weapons>().myType)
                    {
                        case Weapons.WEAPONTYPE.NULL:
                        case Weapons.WEAPONTYPE.Melee:
                            break;
                        case Weapons.WEAPONTYPE.Weapon:
                            if (number_Weapon != FirstWeaponNumber)
                            {
                                myCurWeapon.GetComponent<Weapons>().OnFire(ZoomPosVec);
                                myAnim.SetTrigger("Shot");
                            }
                            break;
                    }
                }
                if (myCurGrenade != null && number_Grenade > 0 && myCurGrenade.activeSelf)
                {

                }
                if (myCurPotion != null && number_Potion > 0 && myCurPotion.activeSelf)
                {

                }
            }
            if (!isReloading && Input.GetMouseButtonDown(0))
            {
                isAttacking = true;
                if (myCurWeapon != null && myCurWeapon.GetComponent<Weapons>().Curbullet > 0 && myCurWeapon.activeSelf)
                {
                    switch (myCurWeapon.GetComponent<Weapons>().myType)
                    {
                        case Weapons.WEAPONTYPE.NULL:
                        case Weapons.WEAPONTYPE.Melee:
                            if (CanMeleeAttack)
                            {
                                myCurWeapon.GetComponent<Weapons>().OnFire(ZoomPosVec);
                                myAnim.SetTrigger("Swing");
                            }
                            break;
                        case Weapons.WEAPONTYPE.Weapon:
                            if (number_Weapon == FirstWeaponNumber)
                            {
                                myCurWeapon.GetComponent<Weapons>().ButtonDownOnFire(ZoomPosVec);
                                myAnim.SetTrigger("Shot");
                            }
                            break;
                    }
                }
                if (myCurGrenade != null && number_Grenade > 0 && myCurGrenade.activeSelf)
                {

                }
                if (myCurPotion != null && number_Potion > 0 && myCurPotion.activeSelf)
                {

                }
            }
            if (!isReloading && Input.GetMouseButtonUp(0))
            {
                if (myCurWeapon != null && myCurWeapon.GetComponent<Weapons>().Curbullet > 0 && myCurWeapon.activeSelf)
                {
                    switch (myCurWeapon.GetComponent<Weapons>().myType)
                    {
                        case Weapons.WEAPONTYPE.NULL:
                        case Weapons.WEAPONTYPE.Melee:
                            break;
                        case Weapons.WEAPONTYPE.Weapon:
                            break;
                    }
                }
                if (myCurGrenade != null && number_Grenade > 0 && myCurGrenade.activeSelf)
                {

                }
                if (myCurPotion != null && number_Potion > 0 && myCurPotion.activeSelf)
                {

                }
                isAttacking = false;
            }
        }
    }
    

    void PlayerMove(float x, float y, Vector3 dir)
    {
        if (isRun)
        {
            transform.position += moveDir * MoveSpeed * Time.deltaTime; //구한 벡터로 움직임
        }
    }

    void PlayerRotate(Vector3 dir)
    {
        if (!isRun && !isAttacking) return;
        myRigid.rotation = Quaternion.Slerp(myRigid.rotation, Quaternion.LookRotation(dir), Time.deltaTime * MoveSpeed);
    }
    void Jump()
    {
        isAttacking = false;
        myRigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        myAnim.SetTrigger("Jump");
        myAnim.SetBool("isJump", true);
        isJump = true;
    }

    IEnumerator Dodge()
    {
        isAttacking = false;
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
                    myInfo.Coin += item.value;
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
                number_Weapon = item.value;
                if (myCurWeapon != null)
                {
                    if (myCurWeapon == myWeaponlist[number_Weapon])
                    {
                        number_Ammo += 1;
                    }
                    else
                    {
                        myCurWeapon.SetActive(false);
                        number_Ammo = 1;
                    }
                }
                myCurWeapon = myWeaponlist[number_Weapon];
                myCurWeapon.GetComponent<Weapons>().Value = number_Weapon;
                if (myCurGrenade != null && myCurGrenade.activeSelf) myCurGrenade.SetActive(false);
                if (myCurPotion != null && myCurPotion.activeSelf) myCurPotion.SetActive(false);
                myAnim.SetTrigger("Swap");
                myCurWeapon.SetActive(true);
                Weapons.Inst.BulletSetup();
                Destroy(item.gameObject);
            }
        }
    }

    void WeaponReset() // 총알 다 쓰면 기본총으로 돌아오기
    {
        if (myCurWeapon == null)
        {
            number_Weapon = FirstWeaponNumber;
            myCurWeapon = myWeaponlist[number_Weapon];
            if (!myCurWeapon.activeSelf)
            {
                myAnim.SetTrigger("Swap");
                myCurWeapon.gameObject.SetActive(true);
            }
        }
    }

    void GiveWeaponInformaition() //무기 정보 전달
    {
        CurWeaponInfor.Inst.WeaponNumber = number_Weapon;
        CurWeaponInfor.Inst.curAmmo = myCurWeapon.GetComponent<Weapons>().Curbullet;
        CurWeaponInfor.Inst.maxAmmo = myCurWeapon.GetComponent<Weapons>().Maxbullet * number_Ammo;
    }
    void FindZoomVec() //조준점 벡터 구하기
    {
        RaycastHit hit;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if(Physics.Raycast(ray, out hit, 1000, ShotLay))
        {
            ZoomPosVec = hit.point;
        }
        else
        {
            ZoomPosVec = Camera.main.transform.position + Camera.main.transform.forward * 300;
        }
    }

    void Reload(float playTime = 2.32f)
    {
        StartCoroutine(Reloading(playTime));
    }
    IEnumerator Reloading(float playTime)
    {
        myAnim.SetTrigger("Reload");
        while(playTime > 0.0f)
        {
            playTime -= Time.deltaTime;
            yield return null;
        }
        if (number_Weapon != FirstWeaponNumber)
        {
            number_Ammo -= 1;
        }
        myCurWeapon.GetComponent<Weapons>().Curbullet = myCurWeapon.GetComponent<Weapons>().Maxbullet;
        isReloading = false;
    }
}
