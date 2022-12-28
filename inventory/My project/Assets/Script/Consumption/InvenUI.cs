using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenUI : MonoBehaviour
{
    public GameObject Inventory;
    private void Awake()
    {
        Button CloseButton = transform.GetChild(1).GetComponent<Button>();
        CloseButton.onClick.AddListener(Close);
    }

    void Close()
    {
        Inventory.SetActive(false);
    }
}
