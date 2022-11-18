using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyBulletCase());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DestroyBulletCase()
    {
        yield return new WaitForSeconds(4.0f);
        Destroy(gameObject);
    }
}
