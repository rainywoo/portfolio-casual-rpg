using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exp : MonoBehaviour
{
    public ExpSensor mySensor = null;
    public float myExp;
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {
        if(mySensor.isPlayer)
        {
            Vector3 GoalPos = mySensor.targetPos;
            transform.parent.position += GoalPos * 5.0f * Time.deltaTime;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Player>().GetExp(myExp);
            Destroy(gameObject);
        }
    }
}
