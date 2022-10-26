using Rito.InventorySystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.Port;

public class EquipmentInventory : MonoBehaviour
{
    [SerializeField]
    private EquipmentUI _inventoryUI;

    /// <summary> 업데이트 할 인덱스 목록 </summary>
    private readonly HashSet<int> _indexSetForUpdate = new HashSet<int>();

    /// <summary> 아이템 수용 한도 </summary>
    public int Capacity { get; private set; }

    /// <summary> 아이템 목록 </summary>
    [SerializeField]
    private Item[] _items;

    // 초기 수용 한도
    [SerializeField, Range(8, 64)]
    private int _initalCapacity = 32;

    // 최대 수용 한도(아이템 배열 크기)
    [SerializeField, Range(8, 64)]
    private int _maxCapacity = 64;

    /// <summary> 해당 슬롯이 아이템을 갖고 있는지 여부 </summary>
    public bool HasItem(int index)
    {
        return IsValidIndex(index) && _items[index] != null;
    }

    /// <summary> 해당 슬롯이 셀 수 있는 아이템인지 여부 </summary>
    public bool IsCountableItem(int index)
    {
        return HasItem(index) && _items[index] is CountableItem;
    }


    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < Capacity;
    }
    private void Awake()
    {
        _items = new Item[_maxCapacity];
        Capacity = _initalCapacity;
        _inventoryUI.SetInventoryReference(this);
    }

    private void Start()
    {
        UpdateAccebssileStatesAll();
    }
    public void TrimAll()
    {
        // 가장 빠른 배열 빈공간 채우기 알고리즘

        // i 커서와 j 커서
        // i 커서 : 가장 앞에 있는 빈칸을 찾는 커서
        // j 커서 : i 커서 위치에서부터 뒤로 이동하며 기존재 아이템을 찾는 커서

        // i커서가 빈칸을 찾으면 j 커서는 i+1 위치부터 탐색
        // j커서가 아이템을 찾으면 아이템을 옮기고, i 커서는 i+1 위치로 이동
        // j커서가 Capacity에 도달하면 루프 즉시 종료

        _indexSetForUpdate.Clear();

        int i = -1;
        while (_items[++i] != null) ;
        int j = i;

        while (true)
        {
            while (++j < Capacity && _items[j] == null) ;

            if (j == Capacity)
                break;

            _indexSetForUpdate.Add(i);
            _indexSetForUpdate.Add(j);

            _items[i] = _items[j];
            _items[j] = null;
            i++;
        }

        foreach (var index in _indexSetForUpdate)
        {
            UpdateSlot(index);
        }
        _inventoryUI.UpdateAllSlotFilters();
    }

    /// <summary> 두 인덱스의 아이템 위치를 서로 교체 </summary>
    public void Swap(int indexA, int indexB)
    {
        if (!IsValidIndex(indexA)) return;
        if (!IsValidIndex(indexB)) return;

        Item itemA = _items[indexA];
        Item itemB = _items[indexB];

        // 1. 셀 수 있는 아이템이고, 동일한 아이템일 경우
        //    indexA -> indexB로 개수 합치기
        if (itemA != null && itemB != null &&
            itemA.Data == itemB.Data &&
            itemA is CountableItem ciA && itemB is CountableItem ciB)
        {
            int maxAmount = ciB.MaxAmount;
            int sum = ciA.Amount + ciB.Amount;

            if (sum <= maxAmount)
            {
                ciA.SetAmount(0);
                ciB.SetAmount(sum);
            }
            else
            {
                ciA.SetAmount(sum - maxAmount);
                ciB.SetAmount(maxAmount);
            }
        }
        // 2. 일반적인 경우 : 슬롯 교체
        else
        {
            _items[indexA] = itemB;
            _items[indexB] = itemA;
        }

        // 두 슬롯 정보 갱신
        UpdateSlot(indexA, indexB);
    }

    /// <summary> 해당 슬롯의 아이템 정보 리턴 </summary>
    public ItemData GetItemData(int index)
    {
        if (!IsValidIndex(index)) return null;
        if (_items[index] == null) return null;

        return _items[index].Data;
    }

    /// <summary> 
    /// 해당 슬롯의 현재 아이템 개수 리턴
    /// <para/> - 잘못된 인덱스 : -1 리턴
    /// <para/> - 빈 슬롯 : 0 리턴
    /// <para/> - 셀 수 없는 아이템 : 1 리턴
    /// </summary>
    public int GetCurrentAmount(int index)
    {
        if (!IsValidIndex(index)) return -1;
        if (_items[index] == null) return 0;

        CountableItem ci = _items[index] as CountableItem;
        if (ci == null)
            return 1;

        return ci.Amount;
    }

    /// <summary> 해당 슬롯의 아이템 이름 리턴 </summary>
    public string GetItemName(int index)
    {
        if (!IsValidIndex(index)) return "";
        if (_items[index] == null) return "";

        return _items[index].Data.Name;
    }

    /// <summary> 해당하는 인덱스의 슬롯 상태 및 UI 갱신 </summary>
    private void UpdateSlot(int index)
    {
        if (!IsValidIndex(index)) return;

        Item item = _items[index];

        // 1. 아이템이 슬롯에 존재하는 경우
        if (item != null)
        {
            // 아이콘 등록
            _inventoryUI.SetItemIcon(index, item.Data.IconSprite);

            // 1-1. 셀 수 있는 아이템
            if (item is CountableItem ci)
            {
                // 1-1-1. 수량이 0인 경우, 아이템 제거
                if (ci.IsEmpty)
                {
                    _items[index] = null;
                    RemoveIcon();
                    return;
                }
                // 1-1-2. 수량 텍스트 표시
                else
                {
                    //_inventoryUI.SetItemAmountText(index, ci.Amount);
                }
            }
            // 1-2. 셀 수 없는 아이템인 경우 수량 텍스트 제거
            else
            {
                //_inventoryUI.HideItemAmountText(index);
            }

            // 슬롯 필터 상태 업데이트
            _inventoryUI.UpdateSlotFilterState(index, item.Data);
        }
        // 2. 빈 슬롯인 경우 : 아이콘 제거
        else
        {
            RemoveIcon();
        }

        // 로컬 : 아이콘 제거하기
        void RemoveIcon()
        {
           // _inventoryUI.RemoveItem(index);
           // _inventoryUI.HideItemAmountText(index); // 수량 텍스트 숨기기
        }
    }

    /// <summary> 해당하는 인덱스의 슬롯들의 상태 및 UI 갱신 </summary>
    private void UpdateSlot(params int[] indices)
    {
        foreach (var i in indices)
        {
            UpdateSlot(i);
        }
    }

    public void UpdateAccebssileStatesAll()
    {
        _inventoryUI.SetAccessibleSlotRange(Capacity);
    }
}
