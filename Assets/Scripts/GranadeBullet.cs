using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeBullet : MonoBehaviour
{
    public enum TYPE
    {
        Basic
    }
    public TYPE myType = TYPE.Basic;

    public GameObject BombEffect = null;

    float playTime = 0;
    float bombTime = 3.0f;
    
    void Start()
    {
        StartCoroutine(Bomb());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Bomb()
    {
        while(playTime < bombTime)
        {
            playTime += Time.deltaTime;
            yield return null;
        }
        DoBomb();
    }

    void DoBomb()
    {
        GameObject obj = Instantiate(BombEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
