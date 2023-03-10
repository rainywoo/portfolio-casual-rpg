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
            Debug.Log("�Ǽ� ȹ�� �κ��丮 ǥ��");
            my_accessories = _accessories;
            mySlotImage.sprite = my_accessories.itemImage;
            return true;
        }
        Debug.Log("�Ǽ� ȹ�� �κ��丮 ǥ�� �ȵ�");
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
