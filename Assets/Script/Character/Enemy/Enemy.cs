using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]   //필수적인 컴포넌트가 있을때 자동으로 넣는 유니티 속성(현재는 리지드바디)
[RequireComponent(typeof(Animator))]

public class Enemy : MonoBehaviour
{
    public Waypoints waypoints;
    public float moveSpeed = 3.0f;

    Transform moveTarget;
    Vector3 lookDir;
    float moveSpeedPerSecond;

    Rigidbody rigid;
    Animator anim;

    protected Transform MoveTarget
    {
        get => moveTarget;
        set
        {
            moveTarget = value;
            lookDir = (moveTarget.position - transform.position).normalized;
        }
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    { 
        moveSpeedPerSecond = moveSpeed * Time.fixedDeltaTime;
        if (waypoints != null)
        {
            MoveTarget = waypoints.Current;
        }
        else
        {
            MoveTarget = transform;
        }
        
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(transform.position + moveSpeedPerSecond * lookDir);
        rigid.rotation = Quaternion.Slerp(rigid.rotation, Quaternion.LookRotation(lookDir), 0.1f); // 시선 회전

        if((transform.position - moveTarget.position).sqrMagnitude < 0.1f )
        {
            MoveTarget = waypoints.MoveNext();
        }
    }
}
