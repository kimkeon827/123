using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public float range; // ���� ����
    public int damage; // ���ݷ�
    public float workSpeed; // �۾��ӵ�
    public float attackDelay; // ���� ������
    public float attackDelayA; // ���� Ȱ��ȭ ����. ���� �ִϸ��̼� �߿��� ���� �� �������� ������ ���� �������� ���� �Ѵ�.
    public float attackDelayB; // ���� ��Ȱ��ȭ ����. ���� �� ������ �ָ��� ���� �ִϸ��̼��� ���۵Ǹ� ���� �������� ���� �ȵȴ�.

    protected bool isAttack = false; // ���� ���� ������
    protected bool isSwing = false; // ���� �ֵθ��� ������. isSwing = True �� ���� �������� ����

    public Animator anim; // �ִϸ����� ������Ʈ

    protected void TryAttack()
    {
        //����
    }

 
}
