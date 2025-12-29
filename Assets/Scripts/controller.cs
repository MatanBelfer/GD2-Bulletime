using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class controller : MonoBehaviour
{
    [SerializeField] private InputActionReference MoveAction;
    private Rigidbody2D _rb;
    private Vector2 move;

    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        MoveAction.action.Enable();
        MoveAction.action.performed += OnMove;
        MoveAction.action.canceled += OnMove;
    }
    
    private void OnDisable()
    {
        MoveAction.action.Disable();
        MoveAction.action.performed -= OnMove;
        MoveAction.action.canceled -= OnMove;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + move * Time.fixedDeltaTime);
        
    }
}