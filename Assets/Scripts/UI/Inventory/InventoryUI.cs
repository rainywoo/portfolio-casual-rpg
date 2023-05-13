using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryUI : MonoBehaviour
{
    Inventory inven;

    public GameObject InventoryPanel; //�κ��丮 UI
    public GameObject ItemSelectedUI; //�κ��丮���� ������ ������ ������ UI
    public GameObject UsingItemSeletedUI; //������ ��� Ŭ���� ������ UI

    bool activeInventory = false;

    public Slot[] slots; //�κ��丮 ���Ե��� �迭
    public Transform slotHolder; //���� ��ĭ ( ���Ե��� ���� ���� ������Ʈ )

    public Slot[] usingItemSlots; //������ ������ ����
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
        for(int i = 0; i < slots.Length; i++) //������ ���� ������ Ȱ��ȭ
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

    public void AddSlot() //���� Ȯ��
    {
        inven.SlotCnt += 5;
    }
    public void AddDraw() //�κ��丮�� ������ ��� â �̹��� ���ΰ�ħ
    {
        Debug.Log("�Ǽ� ȹ�� �κ��丮 �׸��� " + inven.SlotCnt);
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

        Debug.Log("���� �κ��丮 ������ ���� : " + inven.accessories.Count);
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
        Debug.Log("���� ������ ������ ���� : " + inven.usingAccessories.Count);
    }
    public void ClickInventoryItemButton() //������ ������ư UI Ȱ��ȭ �� ���콺 ��ġ�� �̵�
    {
        if (UsingItemSeletedUI.activeSelf)
            UsingItemSeletedUI.SetActive(false);

        if (!ItemSelectedUI.activeSelf)
            ItemSelectedUI.SetActive(true);
        ItemSelectedUI.transform.position = Input.mousePosition;
    }
}
