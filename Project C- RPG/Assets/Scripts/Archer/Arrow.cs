using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GameObject arrow;
    public Transform StartArrowPos;

    public float arrowSpeed = 15.0f;
    public GameObject target;

    private void Start()
    {
        
    }

    private void Update()
    {
        if(target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, 1.0f);
        }
    }
}
