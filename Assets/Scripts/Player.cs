using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterProperty , IBattle
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
            if (myInfo.CurHP > Mathf.Epsilon) return true;
            else
            {
                return false;
            }
        }
    }

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
    float ThrowPower = 50.0f;

    float x;
    float y;

    public bool CanMeleeAttack = false;
    [SerializeField] bool isJump = false;
    bool isWalk;
    bool isRun;
    bool isWall;
    bool isDodge;
    bool canThrowBomb;
    [SerializeField] bool isReloading;
    [SerializeField] bool isAttacking;
    bool canShot;

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
        WeaponImageUpdate(number_Weapon);

        WalkSpeed = MoveSpeed / 2;
    }
    private void FixedUpdate()
    {
        isWall = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
        FixedStateProcess();
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
                myAnim.SetTrigger("Die");
                break;
        }
    }

    void FixedStateProcess()
    {
        switch (myState)
        {
            case STATE.Create:
                break;
            case STATE.Alive:
                if (!isWall && !isDodge && !isAttacking) PlayerMove(x, y, moveDir); //���� �ε�ġ�� �������̰� , �����̴� �ڵ�
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
                InputProcess(); //Input ��ɵ� ����
                WeaponReset();
                FindZoomVec();

                if (isAttacking)
                {
                    isRun = false;
                    isWalk = false;
                }
                if(!isAttacking) PlayerRotate(moveDir); //ĳ���� ȸ��

                if(!isReloading && !isAttacking && myCurWeapon.activeSelf && myCurWeapon.GetComponent<Weapons>().Curbullet == 0) //������
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

                AnimSetting();
                break;
            case STATE.Death:
                break;
        }
    }

    void InputProcess()
    {
        {   //������ ������ ���� ���ϱ�
            Vector2 Moveinput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            isRun = Moveinput.magnitude != 0;
            lookForward = new Vector3(myCamera.forward.x, 0f, myCamera.forward.z).normalized;
            Vector3 lookRight = new Vector3(myCamera.right.x, 0f, myCamera.right.z).normalized;
            moveDir = lookForward * Moveinput.y + lookRight * Moveinput.x;
        }
        if (Input.GetKey(KeyCode.LeftShift)) //L����Ʈ ������ �ȱ�
        {
            isWalk = true;
            MoveSpeed = WalkSpeed;
        }
        else //���� �ٽ� �޸���
        {
            isWalk = false;
            MoveSpeed = WalkSpeed * 2;
        }
        if(!isJump && Input.GetKeyDown(KeyCode.Space)) Jump(); //����
        if (!myAnim.GetBool("isDodge") && !myAnim.GetBool("isJump") && Input.GetKeyDown(KeyCode.LeftControl)) StartCoroutine(Dodge());

        if(!myAnim.GetBool("isDodge") && !myAnim.GetBool("isJump"))Grabitem();

        if (!isReloading && !isAttacking && myCurWeapon.activeSelf)
        {
            if(number_Ammo > 0 || number_Weapon == FirstWeaponNumber)
            {

            }
        }

        if (!myAnim.GetBool("isJump") && !myAnim.GetBool("isDodge") && !isDodge)
        {
            if(myCurWeapon.GetComponent<Weapons>().Curbullet <= 0)
            {
                isAttacking = false;
            }
            if (!isReloading && Input.GetMouseButton(0))
            {
                PlayerRotate(lookForward);
                if (myCurWeapon != null && myCurWeapon.GetComponent<Weapons>().Curbullet > 0 && myCurWeapon.activeSelf && canShot)
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
                PlayerRotate(lookForward);
                isAttacking = true;
                if (myCurWeapon != null && myCurWeapon.GetComponent<Weapons>().Curbullet > 0 && myCurWeapon.activeSelf && canShot)
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
                    if(!canThrowBomb) canThrowBomb = true;
                }
                if (myCurPotion != null && number_Potion > 0 && myCurPotion.activeSelf)
                {
                    //if (myInfo.CurHP >= myInfo.MaxHP) return;
                    myAnim.SetTrigger("Swing");
                    UsePotion();
                }
            }
            if (!isReloading && Input.GetMouseButtonUp(0))
            {
                if (myCurWeapon != null && myCurWeapon.GetComponent<Weapons>().Curbullet > 0 && myCurWeapon.activeSelf && canShot)
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
                    if (canThrowBomb)
                    {
                        myAnim.SetTrigger("Throw");
                        canThrowBomb = false;
                    }
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
            transform.position += moveDir * MoveSpeed * Time.fixedDeltaTime; //���� ���ͷ� ������
        }
    }

    void PlayerRotate(Vector3 dir)
    {
        if (!isRun && !isAttacking) return;
        myRigid.rotation = Quaternion.Slerp(myRigid.rotation, Quaternion.LookRotation(dir), Time.deltaTime * MoveSpeed);
    }
    void Jump()
    {
        canShot = false;
        isAttacking = false;
        myRigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        myAnim.SetTrigger("Jump");
        myAnim.SetBool("isJump", true);
        isJump = true;
    }

    IEnumerator Dodge()
    {
        canThrowBomb = false;
        isAttacking = false;
        isDodge = true;
        canShot = false;
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
        canShot = true;
    }
    void Grabitem()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwapToWeapon();
        }

        if(myCurGrenade != null && myCurGrenade.activeSelf == false && number_Grenade > 0 && Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (canThrowBomb) canThrowBomb = false; //����ź���� ������ �ٲܶ��� ���
            if (myCurPotion != null && myCurPotion.activeSelf) myCurPotion.SetActive(false);
            if (myCurWeapon != null && myCurWeapon.activeSelf) myCurWeapon.SetActive(false);
            myCurGrenade.SetActive(true);
            myAnim.SetTrigger("Swap");
            canShot = false;
        }

        if (myCurPotion != null && myCurPotion.activeSelf == false && number_Potion > 0 && Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (canThrowBomb) canThrowBomb = false; //����ź���� ������ �ٲܶ��� ���
            if (myCurGrenade != null && myCurGrenade.activeSelf) myCurGrenade.SetActive(false);
            if (myCurWeapon != null && myCurWeapon.activeSelf) myCurWeapon.SetActive(false);
            myCurPotion.SetActive(true);
            myAnim.SetTrigger("Swap");
            canShot = false;
        }
    }
    void SwapToWeapon()
    {
        if (canThrowBomb) canThrowBomb = false; //����ź���� ������ �ٲܶ��� ���
        if (myCurGrenade != null && myCurGrenade.activeSelf) myCurGrenade.SetActive(false);
        if (myCurPotion != null && myCurPotion.activeSelf) myCurPotion.SetActive(false);
        if (myCurWeapon != null && !myCurWeapon.activeSelf)
        {
            myCurWeapon.SetActive(true);
        }
        myAnim.SetTrigger("Swap");
        canShot = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == ("CanJump")) //���� ���ɱ��� ������ �ٽ� ���� ����
        {
            myAnim.SetBool("isJump", false);
            isJump = false;
            canShot = true;
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
                        if (myCurGrenade.GetComponent<Weapons>().Value == item.value)
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
                        if (myCurPotion.GetComponent<Useitem_Heal>().Value == item.value)
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
                WeaponImageUpdate(number_Weapon);
                if (myCurGrenade != null && myCurGrenade.activeSelf) myCurGrenade.SetActive(false);
                if (myCurPotion != null && myCurPotion.activeSelf) myCurPotion.SetActive(false);
                myAnim.SetTrigger("Swap");
                myCurWeapon.SetActive(true);
                Weapons.Inst.BulletSetup();
                Destroy(item.gameObject);
            }
        }
    }

    void WeaponReset() // �Ѿ� �� ���� �⺻������ ���ƿ���
    {
        if (myCurWeapon == null)
        {
            number_Weapon = FirstWeaponNumber;
            myCurWeapon = myWeaponlist[number_Weapon];
            WeaponImageUpdate(number_Weapon);
            if (!myCurWeapon.activeSelf)
            {
                myAnim.SetTrigger("Swap");
                myCurWeapon.gameObject.SetActive(true);
            }
        }
    }

    void GiveWeaponInformaition() //���� ���� ����
    {
        if(CurWeaponInfor.Inst.WeaponNumber != number_Weapon)
            CurWeaponInfor.Inst.WeaponNumber = number_Weapon;
        if(CurWeaponInfor.Inst.curAmmo != myCurWeapon.GetComponent<Weapons>().Curbullet)
            CurWeaponInfor.Inst.curAmmo = myCurWeapon.GetComponent<Weapons>().Curbullet;
        if(CurWeaponInfor.Inst.maxAmmo != myCurWeapon.GetComponent<Weapons>().Maxbullet * number_Ammo)
            CurWeaponInfor.Inst.maxAmmo = myCurWeapon.GetComponent<Weapons>().Maxbullet * number_Ammo;

        if (myCurWeapon != null && myCurWeapon.activeSelf) CurWeaponInfor.Inst.isWeapon = true;
        else CurWeaponInfor.Inst.isWeapon = false;
        if (myCurGrenade != null && myCurGrenade.activeSelf) CurWeaponInfor.Inst.isGrenade = true;
        else CurWeaponInfor.Inst.isGrenade = false;
        if (myCurPotion != null && myCurPotion.activeSelf) CurWeaponInfor.Inst.isPotion = true;
        else CurWeaponInfor.Inst.isPotion = false;

        CurWeaponInfor.Inst.curGrenade = number_Grenade;
        CurWeaponInfor.Inst.curPotion = number_Potion;
    }
    void FindZoomVec() //������ ���� ���ϱ�
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

    void AnimSetting()
    {
        myAnim.SetBool("isRun", isRun);
        myAnim.SetBool("isWalk", isWalk);
        myAnim.SetBool("isDodge", isDodge);
        myAnim.SetBool("CanThrow", canThrowBomb);
    }

    public void Fireinthehole()  
    {
        myCurGrenade.GetComponent<Weapons>().ButtonDownOnFire(ZoomPosVec, ThrowPower, !canThrowBomb);
        number_Grenade--;
        if(number_Grenade <= 0)
        {
            SwapToWeapon();
        }
    }

    void UsePotion()
    {
        myInfo.CurHP += (myInfo.MaxHP - myInfo.CurHP) * (myCurPotion.GetComponent<Useitem_Heal>().healAmount / 100);
        number_Potion--;
        if(number_Potion <= 0)
        {
            SwapToWeapon();
        }
    }

    public void OnDamage(float dmg)
    {
        myInfo.CurHP -= dmg;
        if(myInfo.CurHP <= Mathf.Epsilon) ChangeState(STATE.Death);
        StartCoroutine(BattleSystem.Damaging(myRenderer));
    }
    Coroutine NuckCo = null;
    public void NuckBack(Vector3 dir, float power = 25)
    {
        if (NuckCo != null) StopCoroutine(NuckCo);
        NuckCo = StartCoroutine(Forcing(power, dir));
    }
    IEnumerator Forcing(float power, Vector3 dir)
    {
        dir.Normalize();
        myRigid.AddForce(dir * power, ForceMode.Impulse);
        yield return new WaitForSeconds(1.0f);
        myRigid.velocity = Vector3.zero;
    }
    void WeaponImageUpdate(int i)
    {
        WeaponImage.Inst.ChangeImageIndex(i);
    }
}
