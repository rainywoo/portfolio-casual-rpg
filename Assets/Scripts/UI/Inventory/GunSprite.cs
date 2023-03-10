using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunSprite : MonoBehaviour
{
    public Image myImage;
    public Image myTarget;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        myImage.sprite = myTarget.sprite;
    }
}
