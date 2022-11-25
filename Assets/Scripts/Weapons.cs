using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public enum WEAPONTYPE { NULL, Melee, Weapon, Granade };
    public WEAPONTYPE myType = WEAPONTYPE.NULL;

    public GameObject myBullet = null;
    public Transform myMuzzle = null;
    public Transform myBinMuzzle = null;
    public GameObject myBinBullet = null;

    public static Weapons Inst;

    public int Maxbullet;
    public int Curbullet;
    public float playTime = 0.0f;
    [SerializeField] float curplayTime = 0.0f;
    [SerializeField] bool CanShot = false;
    public int Value;

    void Awake()
    {
        Inst = this;
    }
    private void Start()
    {
        Curbullet = Maxbullet;
    }
    // Update is called once per frame
    void Update()
    {
        if(curplayTime < playTime)
        {
            curplayTime += Time.deltaTime;
        }
        CanShot = curplayTime >= playTime ? true : false;
        if (myType == WEAPONTYPE.Melee) Player.Inst.CanMeleeAttack = CanShot;
    }
    public void OnFire(Vector3 targetPos, float ShotPower = 100.0f)
    {
        Vector3 GoingVec = ((targetPos - myMuzzle.position).normalized);
        if (CanShot && Curbullet > 0)
        {
            curplayTime = 0.0f;
            Fire(GoingVec, ShotPower);
        }
    }

    public void ButtonDownOnFire(Vector3 targetPos, float ShotPower = 100.0f, bool CanShot = false)
    {
        Vector3 GoingVec = ((targetPos - myMuzzle.position).normalized);
        if (Curbullet > 0 || CanShot)
        {
            Fire(GoingVec, ShotPower);
        }
    }
    void Fire(Vector3 GoingVec, float ShotPower)
    {
        if (myType == WEAPONTYPE.Weapon || myType == WEAPONTYPE.Granade) StartCoroutine(Shot(GoingVec, ShotPower));
        if (myType == WEAPONTYPE.Melee) StartCoroutine(Swing(GoingVec));
        switch(myType)
        {
            case WEAPONTYPE.Granade:
            case WEAPONTYPE.NULL:
                break;
            default: Curbullet--;
                break;
        }
    }

    IEnumerator Shot(Vector3 targetPos, float ShotPower)
    {
        GameObject obj = Instantiate(myBullet, myMuzzle.position, Quaternion.Euler(targetPos), null) as GameObject;
        Rigidbody myBulRigid = obj.GetComponent<Rigidbody>();
        myBulRigid.velocity = targetPos * ShotPower;
        yield return null;

        GameObject bulletobj = Instantiate(myBinBullet, myBinMuzzle.position, myBinMuzzle.localRotation, null) as GameObject;
        Rigidbody myBinBulRigid = bulletobj.GetComponent<Rigidbody>();
        myBinBulRigid.AddForce(myBinMuzzle.forward * Random.Range(5.0f, 10.0f), ForceMode.Impulse);
        myBinBulRigid.AddTorque(Vector3.up * 10.0f, ForceMode.Impulse);
    }
    IEnumerator Swing(Vector3 targetPos)
    {
        yield return new WaitForSeconds(0.5f);
        GameObject obj = Instantiate(myBullet, myMuzzle.position, Quaternion.Euler(targetPos), null) as GameObject;
        Rigidbody myBulRigid = obj.GetComponent<Rigidbody>();
        myBulRigid.velocity = targetPos * 100.0f;
        yield return null;
    }

    public void BulletSetup()
    {
        Curbullet = Maxbullet;
    }
}
