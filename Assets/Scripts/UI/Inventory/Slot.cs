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
        if (my_accessories == null && _accessories == null)
            return false;
        if (my_accessories != _accessories)
        {
            my_accessories = _accessories;
            mySlotImage.sprite = my_accessories.itemImage;
            return true;
        }
        return false;
    }
    public bool SetNullitem()
    {
        my_accessories = null;
        mySlotImage.sprite = null;
        return true;
    }
    public void PutItem() //인벤토리에 있는 아이템 클릭할때 나오는 버튼 활성화 실행
    {
        if (IsHaveItem())
        {
            InventoryUI.Inst.ClickInventoryItemButton();
        }
    }
    public void UsingItem()
    {
        Inventory.Inst.UseItem(my_accessories);
    }
    bool IsHaveItem()
    {
        if (my_accessories == null) return false;
        return true;
    }
    private void Update()
    {
        if(mySlotImage.sprite != null && !mySlotImage.gameObject.activeSelf)
            mySlotImage.gameObject.SetActive(true);
        else if (mySlotImage.sprite == null && mySlotImage.gameObject.activeSelf)
            mySlotImage.gameObject.SetActive(false);
    }
}
