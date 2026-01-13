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
    private Animator _anim;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _shoot = GetComponent<Shoot>();
        _anim = GetComponentInChildren<Animator>();
    }


    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 movement = moveAction.action.ReadValue<Vector2>();
        _rb.MovePosition(_rb.position + movement * Time.fixedDeltaTime * speed);

        if (movement.x != 0 || movement.y != 0) //Setting up idle animation
        {
            _anim.SetFloat("horizontal", movement.normalized.x);
            _anim.SetFloat("vertical", movement.normalized.y);
            _anim.SetBool("isWalking", true);
        }
        else
        {
            _anim.SetBool("isWalking", false);
        }
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
        shootAction.action.performed -= OnShoot;
        rewindAction.action.performed -= OnRewind;
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        var mouse = Mouse.current;
        Vector2 targetPos = mouse.position.ReadValue();
        
        
        Vector3 mouseWorldPosition =
            Camera.main.ScreenToWorldPoint(new Vector3(targetPos.x, targetPos.y,
                Mathf.Abs(Camera.main.transform.position.z)));

        _shoot.RequestShoot(mouseWorldPosition);
    }

    private void OnRewind(InputAction.CallbackContext context)
    {
        _shoot.RequestRewind();
    }


    #endregion
}