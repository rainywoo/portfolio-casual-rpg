using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponImage : MonoBehaviour
{
    public static WeaponImage Inst;
    public int ImageIndex;
    public Sprite[] Icon = null;
    public Image myImage;
    private void Awake()
    {
        Inst = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeImageIndex(int i)
    {
        ImageIndex = i;
        ImageIconChange(ImageIndex);
    }
    void ImageIconChange(int i)
    {
        if(Icon[i] == null)
        {
            i = 0;
        }
        myImage.sprite = Icon[i];
    }
}
