using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    private static InventoryScript instance;
    public static InventoryScript MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryScript>();
            }
            return instance;
        }

        set
        {
            instance = value;
        }
    }

    // �׽�Ʈ�� ���� �뵵
    [SerializeField]
    private Item[] items;

    private void Awake()
    {
        // ������ �����ϰ�
        Bag bag = (Bag)Instantiate(items[0]);

        // ������ ���� ������ �����ϰ�
        bag.Initalize(16);

        // ���� �������� ����Ѵ�.
        bag.Use();
    }
}
