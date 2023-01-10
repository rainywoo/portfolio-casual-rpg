using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRock : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody myRigid;
    public float playTime = 0.0f;
    float ScaleValue = 0.1f;
    float AngularPower = 2.0f;
    public GameObject BombEfc = null;
    public LayerMask HitMask = default;
    float Damage;
    private void Awake()
    {
        myRigid = GetComponent<Rigidbody>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Shot()
    {
        StartCoroutine(RockShot());
    }
    IEnumerator RockShot()
    {
        playTime = 7f;
        ScaleValue = 0.1f;
        AngularPower = 2.0f;
        float culTime = 0.0f;
        while (culTime < playTime)
        {
            AngularPower += 1.5f;
            ScaleValue += 0.005f;
            transform.localScale = Vector3.one * ScaleValue;
            myRigid.AddTorque(transform.right * AngularPower, ForceMode.Acceleration);
            culTime += Time.deltaTime;
            yield return null;
        }
        Bomb();
    }
    void Bomb()
    {
        GameObject obj = Instantiate(BombEfc, transform.position, transform.rotation, null);
        obj.transform.localScale = Vector3.one * ScaleValue;
        Destroy(gameObject);
    }
    public void Initialize(float Damage)
    {
        this.Damage = Damage * 2;
        Shot();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if((HitMask & 1 << collision.gameObject.layer) != 0)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                IBattle ib = collision.gameObject.GetComponent<IBattle>();
                ib?.OnDamage(Damage);
            }
            Bomb();
        }
    }
}
