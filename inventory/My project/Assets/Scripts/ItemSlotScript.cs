using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Rito.InventorySystem;

namespace Rito.InventorySystem
{
    public class ItemSlotScript : MonoBehaviour
    {

        [Tooltip("아이템 아이콘 이미지")]
        [SerializeField] private Image _iconImage;





        /// <summary> 접근 가능한 슬롯인지 여부 </summary>
        public bool IsAccessible => _isAccessibleSlot && _isAccessibleItem;



        private EquipmentUI _inventoryUI;


        private GameObject _iconGo;
        private GameObject _textGo;

        public int Index { get; private set; }
        private bool _isAccessibleSlot = true; // 슬롯 접근가능 여부
        private bool _isAccessibleItem = true; // 아이템 접근가능 여부
        public RectTransform IconRect => _iconRect;
        private RectTransform _iconRect;
        private RectTransform _highlightRect;
        public bool HasItem => _iconImage.sprite != null; // 슬롯이 아이템을 보유하고 있는지 여부
        public void SetSlotIndex(int index) => Index = index;

        private void ShowIcon() => _iconGo.SetActive(true);
        private void HideIcon() => _iconGo.SetActive(false);
        private void HideText() => _textGo.SetActive(false);

        /// <summary> 비활성화된 아이콘 색상 </summary>
        private static readonly Color InaccessibleIconColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);


        private void Awake()
        {
            _inventoryUI = GetComponentInParent<EquipmentUI>();
        }

        public void SetItemAccessibleState(bool value)
        {
            // 중복 처리는 지양
            if (_isAccessibleItem == value) return;

            if (value)
            {
                _iconImage.color = Color.white;
               // _amountText.color = Color.white;
            }
            else
            {
                _iconImage.color = InaccessibleIconColor;
               // _amountText.color = InaccessibleIconColor;
            }

            _isAccessibleItem = value;
        }
        public void SetHighlightOnTop(bool value)
        {
            if (value)
                _highlightRect.SetAsLastSibling();
            else
                _highlightRect.SetAsFirstSibling();
        }

        public void SetSlotAccessibleState(bool value)
        {
            return;
        }

            /// <summary> 슬롯에 아이템 등록 </summary>
            public void SetItem(Sprite itemSprite)
        {
            //if (!this.IsAccessible) return;

            if (itemSprite != null)
            {
                _iconImage.sprite = itemSprite;
                ShowIcon();
            }
            else
            {
                RemoveItem();
            }
        }

        /// <summary> 슬롯에서 아이템 제거 </summary>
        public void RemoveItem()
        {
            _iconImage.sprite = null;
            HideIcon();
            HideText();
        }

        /// <summary> 다른 슬롯과 아이템 아이콘 교환 </summary>
        public void SwapOrMoveIcon(ItemSlotScript other)
        {
            if (other == null) return;
            if (other == this) return; // 자기 자신과 교환 불가
            if (!this.IsAccessible) return;
            if (!other.IsAccessible) return;

            var temp = _iconImage.sprite;

            // 1. 대상에 아이템이 있는 경우 : 교환
            if (other.HasItem) SetItem(other._iconImage.sprite);

            // 2. 없는 경우 : 이동
            else RemoveItem();

            other.SetItem(temp);
        }

    }
}
