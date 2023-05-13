using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryUI : MonoBehaviour
{
    Inventory inven;

    public GameObject InventoryPanel; //인벤토리 UI
    public GameObject ItemSelectedUI; //인벤토리에서 아이템 누르면 나오는 UI
    public GameObject UsingItemSeletedUI; //장착된 장비 클릭시 나오는 UI

    bool activeInventory = false;

    public Slot[] slots; //인벤토리 슬롯들의 배열
    public Transform slotHolder; //슬롯 한칸 ( 슬롯들의 상위 게임 오브젝트 )

    public Slot[] usingItemSlots; //장착한 아이템 슬롯
    public Transform usingSlotHolder;

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
        usingItemSlots = usingSlotHolder.GetComponentsInChildren<Slot>();
        inven.onSlotCountChange += SlotChange;
        InventoryPanel.SetActive(activeInventory);
    }

    private void SlotChange(int val)
    {
        for(int i = 0; i < slots.Length; i++) //지정된 슬롯 갯수만 활성화
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
            if(activeInventory == false)
            {
                ItemSelectedUI.SetActive(false);
                UsingItemSeletedUI.SetActive(false);
            }
        }
    }

    public void AddSlot() //슬롯 확장
    {
        inven.SlotCnt += 5;
    }
    public void AddDraw() //인벤토리와 장착한 장비 창 이미지 새로고침
    {
        Debug.Log("악세 획득 인벤토리 그리기 " + inven.SlotCnt);
        int a = 0;
        int b = 0;
        foreach(Accessories access in inven.accessories)
        {
            slots[a].Setitem(access);
            if (a < inven.SlotCnt)
                a++;
        }
        if (a + 1 < inven.SlotCnt)
        {
            for (int i = a + 1; i < inven.SlotCnt; i++)
            {
                slots[i].SetNullitem();
            }
        }

        Debug.Log("현재 인벤토리 아이템 개수 : " + inven.accessories.Count);
        foreach (Accessories access in inven.usingAccessories)
        {
            usingItemSlots[b].Setitem(access);
            if (b < inven.UsingSlotCnt)
                b++;
        }
        if (b + 1 < inven.UsingSlotCnt)
        {
            for (int i = b + 1; i < inven.UsingSlotCnt; i++)
            {
                slots[i].SetNullitem();
            }
        }
        Debug.Log("현재 장착한 아이템 개수 : " + inven.usingAccessories.Count);
    }
    public void ClickInventoryItemButton() //아이템 장착버튼 UI 활성화 및 마우스 위치로 이동
    {
        if (UsingItemSeletedUI.activeSelf)
            UsingItemSeletedUI.SetActive(false);

        if (!ItemSelectedUI.activeSelf)
            ItemSelectedUI.SetActive(true);
        ItemSelectedUI.transform.position = Input.mousePosition;
    }
}
