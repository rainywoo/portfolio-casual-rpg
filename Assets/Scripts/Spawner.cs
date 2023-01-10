using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform myTop;
    public GameObject[] mySpawnList; //최소한 두 가지 몹을 포함할거니까 그 전제로 제작
    public int[] mySpawnPer;
    public GameObject SpawnEffect = null;
    // Start is called before the first frame update
    public enum STATE { Normal, Spawn }
    public STATE myState = STATE.Normal;
    void Start()
    {
        ChangeState(STATE.Normal);
    }

    // Update is called once per frame
    void Update()
    {
        myTop.Rotate(Vector3.up * 25 * Time.deltaTime);
    }
    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(10.0f);
        Vector3 SpawnPlace = transform.position;
        for(int i = 0; i < mySpawnList.Length; i++)
        {
            int SpawnPer = Random.Range(0, 101);
            if(SpawnPer >= 0 && SpawnPer <= mySpawnPer[i])
            {
                SpawnPlace.x += Random.Range(-15, 16);
                SpawnPlace.z += Random.Range(-15, 16);
                GameObject obj = Instantiate(mySpawnList[i], SpawnPlace, Quaternion.identity, null);
                Instantiate(SpawnEffect, SpawnPlace, Quaternion.identity, null);
                obj.GetComponent<EnemyA>().Initialize();
            }
        }
        StartCoroutine(Spawn());
    }
    public void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;
        switch(myState)
        {
            case STATE.Normal:
                StopAllCoroutines();
                break;
            case STATE.Spawn:
                StartCoroutine(Spawn());
                break;
        }
    }
    public bool Changable()
    {
        return myState != STATE.Spawn;
    }
}
