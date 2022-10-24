using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 무기 아이템 정보 </summary>
[CreateAssetMenu(fileName = "Item_Weapon_", menuName = "Inventory System/Item Data/Weapon", order = 1)]
public class WeaponItemData : ItemData
{
    public override Item CreateItem()
    {
        throw new System.NotImplementedException();
    }
}
