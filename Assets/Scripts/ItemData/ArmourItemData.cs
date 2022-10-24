using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

/// <summary> 아머 아이템 정보 </summary>
[CreateAssetMenu(fileName = "Item_Armour_", menuName = "Inventory System/Item Data/Armour", order = 2)]
public class ArmourItemData : ItemData
{
    public override Item CreateItem()
    {
        throw new System.NotImplementedException();
    }
}
