using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ���� ������ ���� </summary>
[CreateAssetMenu(fileName = "Item_AttackItem_", menuName = "Inventory System/Item Data/Attack", order = 5)]
public class AttackItemData : CountableItemData
{

    public override Item CreateItem()
    {
        throw new System.NotImplementedException();
    }

    public bool Use()
    {
        throw new System.NotImplementedException();
    }


}
