using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IBattle, IHealth
{
    Transform weapon;

    Collider weaponBlade;

    Animator anim;

    public float attackPower;
    public float defensePower;
    public float maxHP;
    float hp;
    bool isAlive = true;

    public float AttackPower => attackPower;
    public float DefensePower => defensePower;

    public float HP
    {
        get => hp;
        set
        {
            if(isAlive && hp != value)
            {
                hp = value;

                if(hp < 0)
                {
                    Die();
                }

                hp = Mathf.Clamp(hp, 0.0f, maxHP);

                onHealthChange?.Invoke(hp / maxHP);
            }
        }
    }

    

    public float MaxHP => maxHP;
    public bool IsAlive => isAlive;

    public Action<float> onHealthChange { get; set; }

    public Action onDie { get; set; }





    private void Awake()
    {
        anim = GetComponent<Animator>();


    }

    private void Start()
    {
        hp = maxHP;
        isAlive = true;
    }

    public void WeaponBladeEnable()
    {
        if(weaponBlade != null)
        {
            weaponBlade.enabled = true;
        }
    }

    public void WeaponBladeDisable()
    {
        if (weaponBlade != null)
        {
            weaponBlade.enabled = false;
        }
    }



    public void Attack(IBattle target)
    {
        target?.Defense(AttackPower);
    }

    public void Defense(float damage)
    {
        if (isAlive)
        {
            anim.SetTrigger("Hit");
            HP -= (damage - DefensePower);
        }
    }

    public void Die()
    {
        isAlive = false;
        anim.SetBool("IsAlive", isAlive);
        onDie?.Invoke();
    }
}
