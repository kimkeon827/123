using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackItem : MonoBehaviour
{
    float power = 0.0f;
    float range = 0.0f;
    private bool isAimming;

    void Aim()
    {
        if(!isAimming)
            return;

        Debug.DrawRay(transform.position, Vector3.forward * 1000f, Color.red); ;
    }
}
