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
            Vector3 GoalPos = (transform.position - mySensor.targetPos).normalized;
            transform.position += GoalPos * 2.0f * Time.deltaTime;
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
