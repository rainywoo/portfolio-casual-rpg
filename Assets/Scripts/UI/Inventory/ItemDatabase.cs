using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Inst;
    private void Awake()
    {
        if(Inst != null)
        {
            Destroy(gameObject);
        }
        Inst = this;
    }
    public List<Accessories> itemDB = new List<Accessories>();

    private void Start()
    {
        
    }
}
