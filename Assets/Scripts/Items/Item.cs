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

    public string itemName; // 아이템의 이름
    public ItemType itemType; // 아이템 유형
    public Sprite itemImage; // 아이템 이미지(인벤토리 안에서 띄움)
    public GameObject itemPrefab; // 아이템의 프리팹 ( 아이템 생성시 프리팹으로 찍어냄)

    public string weaponType; // 무기 유형

    public Sprite Icon
    {
        get
        {
            return icon;
        }
    }

    // 아이템이 중첩될 수 있는 개수
    // 예) 소모성 물약의 경우 한개의 slot에 여러개가 중첩되어서 보관될 수있음.
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

    public enum ItemType    // 아이템 유형
    {
        Equipment,
        Used,
        Ingredient,
        ETC,
    }
    

}
