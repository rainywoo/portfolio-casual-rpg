using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCMissile : MonoBehaviour
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
        MoveSpeed = 100.0f;
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
                IBattle ib = other.gameObject.GetComponent<IBattle>();
                ib?.OnDamage(Damage);
            }
            Bomb();
            Destroy(gameObject);
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
        yield return new WaitForSeconds(1.0f);
        float Dist = (myTarget.position - transform.position).magnitude * 2;
        Vector3 dir = (myTarget.position - transform.position).normalized;
        transform.LookAt(myTarget);
        while (Dist > 0)
        {
            float delta = MoveSpeed * Time.deltaTime;
            if(delta >= Dist)
            {
                delta = Dist;
            }
            Dist -= delta;
            transform.position += dir * delta;
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
