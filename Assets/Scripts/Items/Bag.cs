using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ��� �޴��� ��ɹ�ư �߰�.
[CreateAssetMenu(fileName = "Bag", menuName = "Items/Bag", order = 1)]
public class Bag : Item, IUseable
{
    public Sprite MyIcon => throw new System.NotImplementedException();

    private int slots;

    [SerializeField]
    protected GameObject bagPrefab;

    public BagScript MyBagScript { get; set; }

    // ���� ����
    public int Slots
    {
        get
        {
            return slots;
        }
    }

    public void Initalize(int slots)
    {
        // Bag�� ���԰��� ����
        this.slots = slots;
    }

    // ������ ���
    public void Use()
    {
        throw new System.NotImplementedException();

     
    }
}
