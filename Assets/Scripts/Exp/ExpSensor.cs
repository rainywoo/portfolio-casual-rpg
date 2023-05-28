using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpSensor : MonoBehaviour
{
    public bool isPlayer = false;
    public Vector3 targetPos = Vector3.zero;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isPlayer)
            {
                isPlayer = true;
            }
            targetPos = other.transform.position;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (isPlayer)
                isPlayer = false;
            targetPos = Vector3.zero;
        }
    }
}
