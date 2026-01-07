using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{    
    [SerializeField] private InputActionReference shootAction;
    [SerializeField] private InputActionReference rewindAction;
    [SerializeField] private InputActionReference moveAction;

    [SerializeField] private float speed = 30f;
    private Rigidbody2D _rb;
    private Shoot _shoot;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _shoot = GetComponent<Shoot>();
    }


    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 movement = moveAction.action.ReadValue<Vector2>();
        _rb.MovePosition(_rb.position + movement * Time.fixedDeltaTime * speed);
    }


    #region Input Handle

    private void OnEnable()
    {
        shootAction.action.Enable();
        shootAction.action.performed += OnShoot;

        rewindAction.action.Enable();
        rewindAction.action.performed += OnRewind;
        moveAction.action.Enable();
    }

    private void OnDisable()
    {
        shootAction.action.Disable();
        shootAction.action.performed -= OnShoot;

        rewindAction.action.Disable();
        rewindAction.action.performed -= OnRewind;
        moveAction.action.Disable();
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        var mouse = Mouse.current;
        _shoot.RequestShoot(mouse.position.ReadValue());
    }

    private void OnRewind(InputAction.CallbackContext context)
    {
        _shoot.RequestRewind();
    }


    #endregion
}