using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;  //위의 전처리기 있을 때만 실행 버전에 넣어라. 
#endif

[RequireComponent(typeof(Rigidbody))]   //필수적인 컴포넌트가 있을때 자동으로 넣는 유니티 속성
[RequireComponent(typeof(Animator))]

public class Enemy : MonoBehaviour
{
    //웨이포인트 관련 변수...................................
    public Waypoints waypoints; //순찰에 필요한 웨이포인트들
    Transform moveTarget;   //적이 이동할 목표 지점의 트랜스폼
    //Vector3 lookDir;    //적이 이동하는 방향(바라보는 방향)
    //..........................................................

    public float moveSpeed = 3.0f;  //적 이동속도

    //추적 관련 변수(적 추적 기능 추가)...........................
    public float sightRange = 10.0f;    //시야거리 10.0f
    public float sightHalfAngle = 50.0f;   //시야각 50도 
    //.......................................................

    //상태 관련 변수 ............................................
    EnemyState state; //적 현재 상태
    public float waitTime = 2.0f;   //목적지에 도달했을 때 기다리는 시간 설정
    float waitTimer;    //남아있는 기다리는 시간
    //..........................................................

    //컴포넌트 캐싱용 변수.......................
    Animator anim;
    NavMeshAgent agent;

    //추가 데이터 타입..............................
    protected enum EnemyState //적 상태 대기,순찰 
    {
        Wait = 0,
        Patrol
    }

    //델리게이트...........................
    Action stateUpdate; //상태별 업데이터 함수를 가질 델리게이트

    //이동할 목적지를 나타내는 프로퍼티...........................................
    protected Transform MoveTarget
    {
        get => moveTarget;
        set
        {
            moveTarget = value;
            //lookDir = (moveTarget.position - transform.position).normalized;    //lookDir도 함께 갱신
            //agent.SetDestination(moveTarget.position);
        }
    }

    //적의 상태를 나타내는 프로퍼티..........................
    protected EnemyState State
    {
        get => state;
        set
        {
            //switch (state)//이전 상태(상태를 나가면서 해야 할일 처리)
            //{
            //    case EnemyState.Wait:
            //        break;
            //    case EnemyState.Patrol:
            //        break;
            //    default:
            //        break;
            //}
            state = value;  //새로운 상태로 변경
            switch (state) //새로운 상태(새로운 상태로 들어가면서 해야 할 일 처리)
            {
                case EnemyState.Wait:
                    agent.isStopped = true;
                    waitTimer = waitTime;   //타이머 초기화
                    anim.SetTrigger("Stop");    //Idle 애니메이션 재생
                    stateUpdate = Update_Wait;  //FixedUpdate에서 실행될 델리게이트 변경
                    break;
                case EnemyState.Patrol:
                    agent.isStopped = false;
                    agent.SetDestination(MoveTarget.position);
                    anim.SetTrigger("Move");    //Run 애니메이션 재생
                    stateUpdate = Update_Patrol;//FixedUpdate에서 실행될 델리게이트 변경
                    break;
                default:
                    break;
            }
        }
    }

    //남은 대기 시간을 나타내는 프로퍼티...................
    protected float WaitTimer
    {
        get => waitTimer;
        set
        {
            waitTimer = value;
            if(waitTimer < 0.0f)    //남은 시간이 다 되면
            {
                State = EnemyState.Patrol;  //Patrol 상태로 전환
            }
        }
    }
    //.................................................................

    private void Awake()
    {
        //컴포넌트 찾기
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        agent.speed = moveSpeed;

        //웨이포인트가 없을 때를 대비한 코드
        if (waypoints != null)
        {
            MoveTarget = waypoints.Current;
        }
        else
        {
            MoveTarget = transform;
        }

        //값 초기화 작업
        State = EnemyState.Wait;    //기본 상태 설정(wait)
        anim.ResetTrigger("Stop");  //트리거 쌓이는 현상을 리셋해 방지
    }

    private void FixedUpdate()
    {
        stateUpdate();
    }

    //Patrol 상태일 때 실행될 업데이트 함수
    void Update_Patrol()
    {
        //도착 확인
        //agent.pathPending : 경로 계산이 진행중인지 확인.true면 아직 경로 계산 중 - 플레이어 추적때 멀리 나가면 돌아올때 
        //agent.remainingDistance : 도착지점까지 남아있는 거리
        //agent.stoppingDistance : 도착지점에 도착했다고 인정되는 거리
        if ( !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance )   //경로계산이 완료, 도착지점까지 이동하지 않았다.
        {
            MoveTarget = waypoints.MoveNext();  //다음 웨이포인트 지점을 MoveTarget으로 설정
            State = EnemyState.Wait;    //대기 상태로 변경
        }
    }

    //적 추적 기능 함수..............................................
    //wait 상태일 때 실행될 업데이트 함수
    void Update_Wait()
    {
        WaitTimer -= Time.fixedDeltaTime;   //시간 지속적으로 감소
    }

    bool SearchPlayer()
    {
        bool result = false;    //에러 제거용

        //Overlap = 겹친 영역 안에 있는 물체 감지   
        //특정 범위 안에 플레이어 감지
        Collider[] colliders = Physics.OverlapSphere(transform.position, sightRange, LayerMask.GetMask("Player"));  
        if( colliders.Length > 0)
        {
            //Player가 sightRange 안에 있다.
            //Debug.Log("Player가 시야 범위에 있다"); //시야 체크용 디버그


            //특정 시야각 안에 있는지 확인하는 방법 
            //Vector3.Angle(a,b)  //a벡터와 b벡터 사이각을 계산
            //sightHalfAngle;
            //Vector3 playerPos = colliders[0].transform.position;
            //float angle = Vector3.Angle(transform.forward, playerPos - transform.position);
            //if( sightHalfAngle > angle) 
            Vector3 playerPos = colliders[0].transform.position;    //플레이어의 위치
            Vector3 toPlayerDir = playerPos - transform.position;   //플레이어로 가는 방향

            //시야각 안에 플레이어가 있는지 확인
            if (IsInSightAngle(toPlayerDir))
            {
                //시야에 player가 있다.
                //Debug.Log("Player가 시야 각안에 들어왔다");   //시야 범위 체크용 디버그

                //시야가 다른 물체로 인해 막혔는지 확인
                if (!IsSightBlocked(toPlayerDir))
                {
                    //다른 물체로 인해 막히지 않을 때
                    result = true;
                }

            }
        }
        //LayerMask.GetMask("Player","Water"));   // 2^6(64) 리턴, getmask = 레이어 여러개를 한번에 받을수 있음    > 2^6+2^4 = 80
        //LayerMask.NameToLayer("Player"); //6 리턴
        return result;
    }

    /// <summary>
    /// 대상이 시야각안에 들어와 있는지 확인하는 함수
    /// </summary>
    /// <param name="toTargetDir">대상으로 가는 방향 벡터</param>
    /// <returns>true면 대상이 시야각안에 있다. false면 없다.</returns>
    bool IsInSightAngle(Vector3 toTargetDir)
    {
        float angle = Vector3.Angle(transform.forward, toTargetDir);    // forward 벡터와 플레어어로 가는 방향 벡터의 사이각 구하기
        return (sightHalfAngle > angle);
    }

    /// <summary>
    /// 플레이어를 바라보는 시야가 막혔는지 확인하는 함수
    /// </summary>
    /// <param name="toTargetDir">대상으로 가는 방향 벡터</param>
    /// <returns>true면 시야가 막혀있다. false면 아니다.</returns>
    bool IsSightBlocked(Vector3 toTargetDir)
    {
        bool result = true;
        // 레이 만들기 : 시작점 = 적의 위치 + 적의 눈높이, 방향 = 적에서 플레이어로 가는 방향
        Ray ray = new(transform.position + transform.up * 0.5f, toTargetDir);
        if (Physics.Raycast(ray, out RaycastHit hit, sightRange))
        {
            // 레이에 부딪친 컬라이더가 있다.
            if (hit.collider.CompareTag("Player"))
            {
                // 그 컬라이더가 플레이어이다.
                result = false;
            }
        }
        return result;
    }

    public void Test()
    {
        SearchPlayer();
        //Debug.Log(this.gameObject.layer);
        //this.gameObject.layer = 0b_0000_0000_0000_0000_0000_0000_0000_1101;
    }

    private void OnDrawGizmos() //OnDrawGizmos + Selected 추가로 쓰면 선택한 몬스터의 시야범위만 볼 수 있음
    {
#if UNITY_EDITOR
        Handles.color = Color.green;    //기본 시야 색
        //Gizmos.DrawWireSphere(transform.position, sightRange);
        Handles.DrawWireDisc(transform.position, transform.up, sightRange); //시야 범위를 디스트 평면원 형태로

        if (SearchPlayer()) //플레이어 감지 
        {
            Handles.color = Color.red;  //감지 시야 색 
        }

        Vector3 forward = transform.forward * sightRange;   //시야 범위 벡터 
        Handles.DrawDottedLine(transform.position, transform.position + forward, 2.0f);    // 시야 반경 원 

        Quaternion q1 = Quaternion.AngleAxis(-sightHalfAngle, transform.up);    //up벡터를 축으로 반시계 반앵글만큼 회전
        Quaternion q2 = Quaternion.AngleAxis(sightHalfAngle, transform.up);     //up벡터를 축으로 시계 반앵글만큼 회전

        //시야 범위 표시용 선긋기 기즈모
        //Handles.DrawLine(transform.position, transform.position + halfAngle만큼 회전된 forward * sightRange);  
        Handles.DrawLine(transform.position, transform.position + q1 * forward);    //중심선 반시계 회전시켜 그리기
        Handles.DrawLine(transform.position, transform.position + q2 * forward);    //중심선 시계 회전시켜 그리기

        Handles.DrawWireArc(transform.position, transform.up, q1 * forward, sightHalfAngle * 2, sightRange, 5.0f);  //부채꼴 호 강조
#endif
    }
}
