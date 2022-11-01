using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform myPlayer;
    void Start()
    {
        
    }

    
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(myPlayer.position, transform.position) <= Mathf.Epsilon)
            return;
        else transform.position = Vector3.Lerp(transform.position, myPlayer.position, 10.0f * Time.deltaTime);
    }
}
