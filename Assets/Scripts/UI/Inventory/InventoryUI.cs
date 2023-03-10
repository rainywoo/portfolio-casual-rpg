using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryUI : MonoBehaviour
{
    Inventory inven;

    public GameObject InventoryPanel;

    bool activeInventory = false;

    public Slot[] slots;
    public Transform slotHolder;

    public static InventoryUI Inst;
    private void Awake()
    {
        Inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        inven = Inventory.Inst;
        slots = slotHolder.GetComponentsInChildren<Slot>();
        inven.onSlotCountChange += SlotChange;
        InventoryPanel.SetActive(activeInventory);
    }

    private void SlotChange(int val)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if(i < inven.SlotCnt)
            {
                slots[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                slots[i].GetComponent<Button>().interactable = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            activeInventory = !activeInventory;
            InventoryPanel.SetActive(activeInventory);
        }
    }

    public void AddSlot()
    {
        inven.SlotCnt += 5;
    }
    public void AddDraw(Accessories _accessories)
    {
        Debug.Log("악세 획득 인벤토리 그리기 " + inven.SlotCnt);
        for (int i = 0; i < inven.SlotCnt; i++)
        {
            if (slots[i].my_accessories.itemImage == null) //이미지가 없으면 아이템이 없는걸로
            {
                slots[i].Setitem(_accessories);
                Debug.Log("악세 획득 인벤토리 표시");
                return;
            }
        }
    }
}
