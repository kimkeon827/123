using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArcherController : MonoBehaviour
{
    PlayerInputActions inputActions;
    Animator anim;
    Player player;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        inputActions.Player.Attack.performed -= OnAttack;
        inputActions.Player.Disable();
    }

    private void OnAttack(InputAction.CallbackContext _)
    {
        
        anim.SetTrigger("Attack");
    }
}
