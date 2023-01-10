using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float MoveSpeed;
    [SerializeField] float Damage;
    [SerializeField] float criticalChance;
    Vector3 MoveVec = Vector3.zero;

    float Dist = 500.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(transform.forward * MoveSpeed * Time.deltaTime);
    }

    public void Initialize(float speed, float damage, float chance, Vector3 pos)
    {
        MoveSpeed = speed;
        Damage = damage;
        criticalChance = chance;
        MoveVec = pos;
        StartCoroutine(OnFire());
    }

    IEnumerator OnFire()
    {
        while (Dist > 0)
        {
            float delta = MoveSpeed * Time.deltaTime;
            if(delta > Dist)
            {
                delta = Dist;
            }
            Dist -= delta;
            transform.Translate(MoveVec.normalized * delta, Space.World);
            yield return null;
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            IBattle ib = other.GetComponent<IBattle>();
            if (ib != null && ib.IsLive)
            {
                ib.OnDamage(Damage);
                Destroy(gameObject);
            }
        }
        if(other.gameObject.layer == LayerMask.NameToLayer("Floor") || other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
