using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        if ( !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance )   //경로계산이 완료, 도착지점가지 이동하지 않았다.
        {
            MoveTarget = waypoints.MoveNext();  //다음 웨이포인트 지점을 MoveTarget으로 설정
            State = EnemyState.Wait;    //대기 상태로 변경
        }
    }

    //wait 상태일 때 실행될 업데이트 함수
    void Update_Wait()
    {
        WaitTimer -= Time.fixedDeltaTime;   //시간 지속적으로 감소
    }
}
