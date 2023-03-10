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

    public delegate void OnSlotCountChange(int val);
    public OnSlotCountChange onSlotCountChange;

    public delegate void OnChangeItem();
    public OnChangeItem onChangeItem;

    [SerializeField] List<Accessories> accessories = new List<Accessories>();

    public int SlotCnt
    {
        get => slotCnt;
        set
        {
            slotCnt = value;
            onSlotCountChange(slotCnt);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SlotCnt = 5;
    }

    public bool Additem(Accessories _accessorie)
    {
        if(accessories.Count < SlotCnt)
        {
            accessories.Add(_accessorie);
            InventoryUI.Inst.AddDraw(_accessorie);
            if (onChangeItem != null)
                onChangeItem.Invoke();
            return true;
        }
        return false;
    }
}
