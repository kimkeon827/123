using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rito.InventorySystem
{
    public class EquipmentUI : MonoBehaviour
    {
        private EquipmentInventory _inventory;

        [SerializeField] private RectTransform _contentAreaRT; // 슬롯들이 위치할 영역

        [Header("Buttons")]
        [SerializeField] private Button _trimButton;
        [SerializeField] private Button _sortButton;

        /*[Header("Filter Toggles")]
        [SerializeField] private Toggle _toggleFilterAll;
        [SerializeField] private Toggle _toggleFilterEquipments;
        [SerializeField] private Toggle _toggleFilterPortions;*/

        private List<ItemSlotScript> _slotUIList = new List<ItemSlotScript>();
        private GraphicRaycaster _gr;
        private PointerEventData _ped;
        private List<RaycastResult> _rrList;

        private ItemSlotScript _pointerOverSlot; // 현재 포인터가 위치한 곳의 슬롯
        private ItemSlotScript _beginDragSlot; // 현재 드래그를 시작한 슬롯
        private Transform _beginDragIconTransform; // 해당 슬롯의 아이콘 트랜스폼

        private int _leftClick = 0;
        private int _rightClick = 1;

        private Vector3 _beginDragIconPoint;   // 드래그 시작 시 슬롯의 위치
        private Vector3 _beginDragCursorPoint; // 드래그 시작 시 커서의 위치
        private int _beginDragSlotSiblingIndex;

        private enum FilterOption
        {
            All, Equipment, Portion
        }
        private FilterOption _currentFilterOption = FilterOption.All;

        private void Awake()
        {
            Init();
            InitButtonEvents();
            InitToggleEvents();
        }

        private void Update()
        {
            _ped.position = Input.mousePosition;

            OnPointerEnterAndExit();
            OnPointerDown();
            OnPointerDrag();
            OnPointerUp();
        }

        private void Init()
        {
            TryGetComponent(out _gr);
            if (_gr == null)
                _gr = gameObject.AddComponent<GraphicRaycaster>();

            // Graphic Raycaster
            _ped = new PointerEventData(EventSystem.current);
            _rrList = new List<RaycastResult>(10);

            // Item Tooltip UI
            /*if (_itemTooltip == null)
            {
                _itemTooltip = GetComponentInChildren<ItemTooltipUI>();
            }*/
        }
        public void SetInventoryReference(EquipmentInventory inventory)
        {
            _inventory = inventory;
        }
        private void InitButtonEvents()
        {
            //_trimButton.onClick.AddListener(() => _inventory.TrimAll());
           // _sortButton.onClick.AddListener(() => _inventory.SortAll());
        }

        private void InitToggleEvents()
        {
            /*_toggleFilterAll.onValueChanged.AddListener(flag => UpdateFilter(flag, FilterOption.All));
            _toggleFilterEquipments.onValueChanged.AddListener(flag => UpdateFilter(flag, FilterOption.Equipment));
            _toggleFilterPortions.onValueChanged.AddListener(flag => UpdateFilter(flag, FilterOption.Portion));*/

            // Local Method
            void UpdateFilter(bool flag, FilterOption option)
            {
                if (flag)
                {
                    _currentFilterOption = option;
                    UpdateAllSlotFilters();
                }
            }
        }

        /// <summary> 접근 가능한 슬롯 범위 설정 </summary>
        public void SetAccessibleSlotRange(int accessibleSlotCount)
        {
            for (int i = 0; i < _slotUIList.Count; i++)
            {
                _slotUIList[i].SetSlotAccessibleState(i < accessibleSlotCount);
            }
        }

        /// <summary> 슬롯에 아이템 아이콘 등록 </summary>
        public void SetItemIcon(int index, Sprite icon)
        {
            _slotUIList[index].SetItem(icon);
        }

        public void UpdateAllSlotFilters()
        {
            int capacity = _inventory.Capacity;

            for (int i = 0; i < capacity; i++)
            {
                ItemData data = _inventory.GetItemData(i);
                UpdateSlotFilterState(i, data);
            }
        }

        public void UpdateSlotFilterState(int index, ItemData itemData)
        {
            bool isFiltered = true;

            // null인 슬롯은 타입 검사 없이 필터 활성화
            if (itemData != null)
                switch (_currentFilterOption)
                {
                    case FilterOption.Equipment:
                        isFiltered = (itemData is EquipmentItemData);
                        break;

                    case FilterOption.Portion:
                        isFiltered = (itemData is PortionItemData);
                        break;
                }

            _slotUIList[index].SetItemAccessibleState(isFiltered);
        }
        /***********************************************************************
            *                               Mouse Event Methods
            ***********************************************************************/
        #region .
        private bool IsOverUI()
            => EventSystem.current.IsPointerOverGameObject();

        /// <summary> 레이캐스트하여 얻은 첫 번째 UI에서 컴포넌트 찾아 리턴 </summary>
        private T RaycastAndGetFirstComponent<T>() where T : Component
        {
            _rrList.Clear();

            _gr.Raycast(_ped, _rrList);

            if (_rrList.Count == 0)
                return null;

            return _rrList[0].gameObject.GetComponent<T>();
        }
        /// <summary> 슬롯에 포인터가 올라가는 경우, 슬롯에서 포인터가 빠져나가는 경우 </summary>
        private void OnPointerEnterAndExit()
        {
            // 이전 프레임의 슬롯
            var prevSlot = _pointerOverSlot;

            // 현재 프레임의 슬롯
            var curSlot = _pointerOverSlot = RaycastAndGetFirstComponent<ItemSlotScript>();

            if (prevSlot == null)
            {
                // Enter
                if (curSlot != null)
                {
                    //OnCurrentEnter();
                }
            }
            else
            {
                // Exit
                if (curSlot == null)
                {
                    //OnPrevExit();
                }

                // Change
                else if (prevSlot != curSlot)
                {
                    //OnPrevExit();
                    //OnCurrentEnter();
                }
            }

            // ===================== Local Methods ===============================
        }


        /// <summary> 슬롯에 클릭하는 경우 </summary>
        private void OnPointerDown()
        {
            // Left Click : Begin Drag
            if (Input.GetMouseButtonDown(_leftClick))
            {
                _beginDragSlot = RaycastAndGetFirstComponent<ItemSlotScript>();

                // 아이템을 갖고 있는 슬롯만 해당
                if (_beginDragSlot != null && _beginDragSlot.HasItem && _beginDragSlot.IsAccessible)
                {
                    // 위치 기억, 참조 등록
                    _beginDragIconTransform = _beginDragSlot.IconRect.transform;
                    _beginDragIconPoint = _beginDragIconTransform.position;
                    _beginDragCursorPoint = Input.mousePosition;

                    // 맨 위에 보이기
                    _beginDragSlotSiblingIndex = _beginDragSlot.transform.GetSiblingIndex();
                    _beginDragSlot.transform.SetAsLastSibling();

                    // 해당 슬롯의 하이라이트 이미지를 아이콘보다 뒤에 위치시키기
                    _beginDragSlot.SetHighlightOnTop(false);
                }
                else
                {
                    _beginDragSlot = null;
                }
            }

            // Right Click : Use Item
            /*   else if (Input.GetMouseButtonDown(_rightClick))
               {
                   ItemSlotUI slot = RaycastAndGetFirstComponent<ItemSlotUI>();

                   if (slot != null && slot.HasItem && slot.IsAccessible)
                   {
                       TryUseItem(slot.Index);
                   }
               }*/
        }
        /// <summary> 드래그하는 도중 </summary>
        private void OnPointerDrag()
        {
            if (_beginDragSlot == null) return;

            if (Input.GetMouseButton(_leftClick))
            {
                // 위치 이동
                _beginDragIconTransform.position =
                    _beginDragIconPoint + (Input.mousePosition - _beginDragCursorPoint);
            }
        }
        /// <summary> 클릭을 뗄 경우 </summary>
        private void OnPointerUp()
        {
            if (Input.GetMouseButtonUp(_leftClick))
            {
                // End Drag
                if (_beginDragSlot != null)
                {
                    // 위치 복원
                    _beginDragIconTransform.position = _beginDragIconPoint;

                    // UI 순서 복원
                    _beginDragSlot.transform.SetSiblingIndex(_beginDragSlotSiblingIndex);

                    // 드래그 완료 처리
                    EndDrag();

                    // 해당 슬롯의 하이라이트 이미지를 아이콘보다 앞에 위치시키기
                    _beginDragSlot.SetHighlightOnTop(true);

                    // 참조 제거
                    _beginDragSlot = null;
                    _beginDragIconTransform = null;
                }
            }
        }

        private void EndDrag()
        {
            ItemSlotScript endDragSlot = RaycastAndGetFirstComponent<ItemSlotScript>();

            // 아이템 슬롯끼리 아이콘 교환 또는 이동
            if (endDragSlot != null && endDragSlot.IsAccessible)
            {
                // 수량 나누기 조건
                // 1) 마우스 클릭 떼는 순간 좌측 Ctrl 또는 Shift 키 유지
                // 2) begin : 셀 수 있는 아이템 / end : 비어있는 슬롯
                // 3) begin 아이템의 수량 > 1
                bool isSeparatable =
                    (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftShift)) &&
                    (_inventory.IsCountableItem(_beginDragSlot.Index) && !_inventory.HasItem(endDragSlot.Index));

                // true : 수량 나누기, false : 교환 또는 이동
                bool isSeparation = false;
                int currentAmount = 0;

                // 현재 개수 확인
                if (isSeparatable)
                {
                    currentAmount = _inventory.GetCurrentAmount(_beginDragSlot.Index);
                    if (currentAmount > 1)
                    {
                        isSeparation = true;
                    }
                }

                /*            // 1. 개수 나누기
                            if (isSeparation)
                                TrySeparateAmount(_beginDragSlot.Index, endDragSlot.Index, currentAmount);*/
                // 2. 교환 또는 이동
                else
                    TrySwapItems(_beginDragSlot, endDragSlot);

                // 툴팁 갱신
                //UpdateTooltipUI(endDragSlot);
                return;
            }

            // 버리기(커서가 UI 레이캐스트 타겟 위에 있지 않은 경우)
            if (!IsOverUI())
            {
                // 확인 팝업 띄우고 콜백 위임
                int index = _beginDragSlot.Index;
                string itemName = _inventory.GetItemName(index);
                int amount = _inventory.GetCurrentAmount(index);

                // 셀 수 있는 아이템의 경우, 수량 표시
                if (amount > 1)
                    itemName += $" x{amount}";
                /*
                            if (_showRemovingPopup)
                                _popup.OpenConfirmationPopup(() => TryRemoveItem(index), itemName);
                            else
                                TryRemoveItem(index);*/
            }
        }

        private void TrySwapItems(ItemSlotScript from, ItemSlotScript to)
        {
            if (from == to)
            {
                return;
            }

            from.SwapOrMoveIcon(to);
            _inventory.Swap(from.Index, to.Index);
        }

        #endregion
    }
}