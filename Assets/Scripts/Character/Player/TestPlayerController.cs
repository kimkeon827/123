using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayerController : MonoBehaviour
{
    public float moveSpeed = 3.0f;  //이동 속도
    Vector3 inputDir = Vector3.zero;    //인풋 벡터2를 벡터3로 바꾸기
    TestPlayerInputActions inputActions;

    private void Awake()
    {
        inputActions = new TestPlayerInputActions();
    }
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
    }
    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Disable();
    }

    private void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime * inputDir);
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        Debug.Log(input);
        inputDir.x = input.x;
        inputDir.y = 0.0f;  
        inputDir.z = input.y;
        
    }
}
