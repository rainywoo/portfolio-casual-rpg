using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushAttackZone : MonoBehaviour
{
    // Start is called before the first frame update
    float Damage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Initialize(float Damage)
    {
        this.Damage = Damage;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player ib = other.GetComponent<Player>();
            ib?.OnDamage(Damage);
            Vector3 NuckDir = other.transform.position - transform.position;
            NuckDir.y = 0;
            ib?.NuckBack(NuckDir.normalized, 25);
        }
    }
}
