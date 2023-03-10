using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanInvenItem : MonoBehaviour
{
    public Accessories accessorie;
    public int DBIndex;

    public void Setitem(Accessories _accessorie)
    {
        accessorie.itemName = _accessorie.itemName;
        accessorie.itemImage = _accessorie.itemImage;
        accessorie.itemType = _accessorie.itemType;
    }
    public Accessories GetItem()
    {
        return accessorie;
    }
    public void DestroyItem()
    {
        Destroy(gameObject);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                if(Inventory.Inst.Additem(GetItem()))
                    DestroyItem();
                //if (inven.Additem(canInvenItem.GetItem()))
                //    canInvenItem.DestroyItem();
            }
        }
    }
    void Update()
    {
        transform.Rotate(Vector3.up * 35 * Time.deltaTime, Space.World);
    }
}
