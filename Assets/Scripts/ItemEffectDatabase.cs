using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemEffect
{
    public string itemName; // 아이템의 이름(Key값으로 사용할 것)
    [Tooltip("HP 만 가능합니다.")]
    public string[] part; // 효과. 어느 부분을 회복하거나 혹은 깎을 포션인지. 포션 하나당 미치는 효과가 여러개일 수 있어 배열.
    public int[] num; // 수치. 포션 하나당 미치는 효과가 여러개일 수 있어 배열. 그에 따른 수치.
}

public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    private ItemEffect[] itemEffects;

    private const string HP = "HP";

    //public void UseItem(Item _item)
    //{
    //    if (_item.itemType == Item.ItemType.Equipment)
    //    {
    //        // 장착
            
    //    }
    //    if (_item.itemType == Item.ItemType.Used)
    //    {
    //        for (int i = 0; i < itemEffects.Length; i++)
    //        {
    //            if (itemEffects[i].itemName == _item.itemName)
    //            {
    //                for (int j = 0; j < itemEffects[i].part.Length; j++)
    //                {
    //                    switch (itemEffects[i].part[j])
    //                    {
    //                      //  플레이어 HP 증가
                            
    //                      //  default:
    //                      //  Debug.Log("잘못된 Status 부위. HP 만 가능합니다.");
    //                    }
    //                    Debug.Log(_item.itemName + " 을 사용했습니다.");
    //                }
    //                return;
    //            }
    //        }
    //        Debug.Log("itemEffectDatabase에 일치하는 itemName이 없습니다.");
    //    }
    //}
}
