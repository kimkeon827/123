using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public float moveSpeed = 3.0f;

    Transform target;
    Vector3 offset = Vector3.zero;
    bool isTargetAlive;

    Vector3 diePosition = Vector3.zero;
    Quaternion dieRotation = Quaternion.identity;
    public float dieSpeed = 0.3f;

    private void Start()
    {
        Player player = GameManager.Inst.Player;
        target = player.transform;     // ��� ���ϱ�(�÷��̾�)
        isTargetAlive = player.IsAlive;
        player.onDie += OnTargetDie;
        offset = transform.position - target.position;  // ������ ���� �����س���
    }

    private void LateUpdate()
    {
        if (isTargetAlive)
        {
            // ī�޶��� ��ġ -> ��ǥġ(����� ��ġ + ����)�� �����ϸ� ����. 1/moveSpeed �ʿ� ���� ��ǥġ���� ����
            transform.position = Vector3.Lerp(transform.position, target.position + offset, moveSpeed * Time.deltaTime);
        }
        else
        {
            float delta = dieSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, diePosition, delta);
            transform.rotation = Quaternion.Slerp(transform.rotation, dieRotation, delta);
        }
    }

    void OnTargetDie()
    {
        isTargetAlive = false;
        diePosition = target.position + target.up * 10.0f;
        dieRotation = Quaternion.LookRotation(-target.up, -target.forward);
    }
}
