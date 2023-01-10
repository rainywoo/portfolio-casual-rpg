using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public Transform myTarget = null;
    float playTime = 0.0f;
    public GameObject BombEfc = null;
    public LayerMask HitMask = default;
    Rigidbody myRigid;
    float myDamage = 0.0f;
    // Start is called before the first frame update
    void Awake()
    {
        myRigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Shot()
    {
        StartCoroutine(OnShot());
    }
    Coroutine coRot = null;
    Coroutine coMove = null;
    IEnumerator OnShot()
    {
        playTime = 0.0f;
        while(playTime < 5.0f)
        {
            if (coRot != null) StopCoroutine(coRot);
            coRot = StartCoroutine(Movement1.Rotating(transform, myTarget.position, 60.0f));
            transform.Translate(Vector3.forward * 20.0f * Time.deltaTime);

            playTime += Time.deltaTime;
            yield return null;
        }
        Bomb();
    }
    public void TargetSet(Transform target)
    {
        myTarget = target;
    }
    public void Initialize(Transform target, float Damage)
    {
        TargetSet(target);
        Shot();
        myDamage = Damage;
    }
    private void OnTriggerEnter(Collider other)
    {
        if((HitMask & 1 << other.gameObject.layer) != 0)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Monster"))
            {
                IBattle ib = other.GetComponent<Player>();
                if(ib != null && ib.IsLive)
                    ib?.OnDamage(myDamage);
            }
            Bomb();
        }
    }
    void Bomb()
    {
        GameObject obj = Instantiate(BombEfc, transform.position, transform.rotation, null);
        Destroy(gameObject);
    }
}
