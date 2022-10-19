using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public abstract class Item : ScriptableObject
{
    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private int stackSize;

    private SlotScript slot;

    public string itemName; // �������� �̸�
    public ItemType itemType; // ������ ����
    public Sprite itemImage; // ������ �̹���(�κ��丮 �ȿ��� ���)
    public GameObject itemPrefab; // �������� ������ ( ������ ������ ���������� ��)

    public string weaponType; // ���� ����

    public Sprite Icon
    {
        get
        {
            return icon;
        }
    }

    // �������� ��ø�� �� �ִ� ����
    // ��) �Ҹ� ������ ��� �Ѱ��� slot�� �������� ��ø�Ǿ ������ ������.
    public int StackSize
    {
        get
        {
            return stackSize;
        }
    }

    protected SlotScript Slot
    {
        get
        {
            return slot;
        }

        set
        {
            slot = value;
        }
    }

    public enum ItemType    // ������ ����
    {
        Equipment,
        Used,
        Ingredient,
        ETC,
    }
    

}
