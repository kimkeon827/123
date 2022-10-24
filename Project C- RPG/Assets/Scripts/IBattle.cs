using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattle
{
    float AttackPower { get; }
    float DefensePower { get; }

    void Attack(IBattle target);
    void Defense(float damage);
}
