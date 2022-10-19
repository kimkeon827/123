using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    public GameObject inventoryPanme;
    bool activeInventory = false;

    private void Start()
    {
        inventoryPanme.SetActive(activeInventory);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            activeInventory = !activeInventory;
            inventoryPanme.SetActive(activeInventory);
        }
    }
}
