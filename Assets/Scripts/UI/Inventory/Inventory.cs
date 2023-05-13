using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region SingleTon
    public static Inventory Inst;
    private void Awake()
    {
        if(Inst != null)
        {
            Destroy(gameObject);
        }
        Inst = this;
    }
    #endregion

    private int slotCnt;
    private int usingSlotCnt;

    public delegate void OnSlotCountChange(int val);
    public OnSlotCountChange onSlotCountChange;

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    public List<Accessories> accessories = new List<Accessories>();
    public List<Accessories> usingAccessories = new List<Accessories>();

    public int SlotCnt
    {
        get => slotCnt;
        set
        {
            slotCnt = value;
            onSlotCountChange(slotCnt);
        }
    }
    public int UsingSlotCnt
    {
        get => usingSlotCnt;
        set
        {
            usingSlotCnt = value;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        SlotCnt = 5;
        usingSlotCnt = 4;
    }

    public bool Additem(Accessories _accessorie)
    {
        if(accessories.Count < SlotCnt)
        {
            accessories.Add(_accessorie);
            InventoryUI.Inst.AddDraw();
            if (onChangeItem != null)
                onChangeItem.Invoke();
            return true;
        }
        return false;
    }
    public void UseItem(Accessories _accessories) //아이템 장착하기
    {
        if(usingAccessories.Count < usingSlotCnt)
        {
            usingAccessories.Add(_accessories);
            accessories.Remove(_accessories);
            Debug.Log("아이템 장착");
            InventoryUI.Inst.AddDraw();
        }
    }
}
