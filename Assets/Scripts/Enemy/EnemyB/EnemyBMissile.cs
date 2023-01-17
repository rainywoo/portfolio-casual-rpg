using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBMissile : MonoBehaviour
{
    public Transform myTarget = null;
    public LayerMask HitMask;
    [SerializeField] float Damage;
    public GameObject BombEf = null;
    public float Dmg
    {
        get => Damage;
        set => Damage = value;
    }
    float MoveSpeed = 30.0f;
    // Start is called before the first frame update
    void Start()
    {
        MoveSpeed = 30.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((HitMask & 1 << other.gameObject.layer) != 0)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                IBattle ib = other.GetComponent<IBattle>();
                ib?.OnDamage(Damage);
            }
            if(other.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                IBattle ib = other.GetComponent<IBattle>();
                if(ib != null && other.gameObject.name == "Enemy D")
                {
                    EnemyD boss = other.GetComponent<EnemyD>();
                    boss.DoStun();
                }
                ib?.OnDamage(Damage);
            }
            Bomb();
        }
    }

    public void Initialize(float Dmg, Transform target)
    {
        myTarget = target;
        Damage = Dmg;
        StartCoroutine(Shot());
    }

    IEnumerator Shot()
    {
        float Dist = 100;
        while(Dist > 0)
        {
            Vector3 dir = (myTarget.position - transform.position).normalized;
            float delta = MoveSpeed * Time.deltaTime;
            if(delta >= Dist)
            {
                delta = Dist;
            }
            Dist -= delta;
            transform.position += dir * delta;
            transform.LookAt(myTarget);
            yield return null;
        }
        Bomb();
    }
    void Bomb()
    {
        Instantiate(BombEf, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
