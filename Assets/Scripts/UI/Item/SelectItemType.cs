using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectItemType : MonoBehaviour
{
    public int DBIndex;

    private void Start()
    {
        CanInvenItem canInvenItem = transform.GetComponent<CanInvenItem>();
        canInvenItem.Setitem(ItemDatabase.Inst.itemDB[DBIndex]);
    }
}
