using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ���� ������ ���� </summary>
[CreateAssetMenu(fileName = "Item_Collect_", menuName = "Inventory System/Item Data/Collect", order = 4)]
public class CollectItemData : CountableItemData
{
    public override Item CreateItem()
    {
        throw new System.NotImplementedException();
    }
}
