using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Accessories my_accessories = null;
    public Image mySlotImage;
    public bool Setitem(Accessories _accessories)
    {
        if (_accessories != null)
        {
            Debug.Log("æ«ºº »πµÊ ¿Œ∫•≈‰∏Æ «•Ω√");
            my_accessories = _accessories;
            mySlotImage.sprite = my_accessories.itemImage;
            return true;
        }
        Debug.Log("æ«ºº »πµÊ ¿Œ∫•≈‰∏Æ «•Ω√ æ»µ ");
        return false;
    }
    private void Update()
    {
        if(mySlotImage.sprite != null && !mySlotImage.gameObject.activeSelf)
            mySlotImage.gameObject.SetActive(true);
        else if (mySlotImage.sprite == null && mySlotImage.gameObject.activeSelf)
            mySlotImage.gameObject.SetActive(false);
    }
}
