using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public float range; // 공격 범위
    public int damage; // 공격력
    public float workSpeed; // 작업속도
    public float attackDelay; // 공격 딜레이
    public float attackDelayA; // 공격 활성화 시점. 공격 애니메이션 중에서 검이 다 뻗어졌을 때부터 공격 데미지가 들어가야 한다.
    public float attackDelayB; // 공격 비활성화 시점. 이제 다 때리고 주먹을 빼는 애니메이션이 시작되면 공격 데미지가 들어가면 안된다.

    protected bool isAttack = false; // 현재 공격 중인지
    protected bool isSwing = false; // 팔을 휘두르는 중인지. isSwing = True 일 때만 데미지를 적용

    public Animator anim; // 애니메이터 컴포넌트

    protected void TryAttack()
    {
        //공격
    }

 
}
