using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range; // 아이템 습득이 가능한 최대 거리

    // private bool pickupActivated = false; // 아이템 습득 가능할 시 True

    private RaycastHit hitInfo; // 충돌체 정보 저장

    
}
