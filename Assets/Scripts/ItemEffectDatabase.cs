using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemEffect
{
    public string itemName; // �������� �̸�(Key������ ����� ��)
    [Tooltip("HP �� �����մϴ�.")]
    public string[] part; // ȿ��. ��� �κ��� ȸ���ϰų� Ȥ�� ���� ��������. ���� �ϳ��� ��ġ�� ȿ���� �������� �� �־� �迭.
    public int[] num; // ��ġ. ���� �ϳ��� ��ġ�� ȿ���� �������� �� �־� �迭. �׿� ���� ��ġ.
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
    //        // ����
            
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
    //                      //  �÷��̾� HP ����
                            
    //                      //  default:
    //                      //  Debug.Log("�߸��� Status ����. HP �� �����մϴ�.");
    //                    }
    //                    Debug.Log(_item.itemName + " �� ����߽��ϴ�.");
    //                }
    //                return;
    //            }
    //        }
    //        Debug.Log("itemEffectDatabase�� ��ġ�ϴ� itemName�� �����ϴ�.");
    //    }
    //}
}
