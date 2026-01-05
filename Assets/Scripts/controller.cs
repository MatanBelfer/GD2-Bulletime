using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class controller : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private InputActionReference shootAction;
    [SerializeField] private InputActionReference aimAction;


    [SerializeField] private float speed = 30f;
    [SerializeField] private InputActionReference moveAction;
    private Rigidbody2D _rb;
    private Vector2 move;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        HandleMovement();
    }


    private void HandleShooting()
    {
        Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
        Vector2 mouseScreenPosition = aimAction.action.ReadValue<Vector2>();
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Mathf.Abs(Camera.main.transform.position.z)));
 
        bullet.direction = (mouseWorldPosition - transform.position);
        bullet.direction.z = 0;
        bullet.direction.Normalize();

        float angle = Mathf.Atan2(bullet.direction.y, bullet.direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        bullet.targetTag = GameManager.EnemyTag;
        bullet.ignoreTag = GameManager.PlayerTag;
    }

    private void HandleMovement()
    {
        _rb.MovePosition(_rb.position + move * (Time.fixedDeltaTime * speed));
    }


    #region Input Handle

    private void OnEnable()
    {
        shootAction.action.Enable();
        shootAction.action.performed += OnShoot;
        aimAction.action.Enable();


        moveAction.action.Enable();
        moveAction.action.performed += OnMove;
        moveAction.action.canceled += OnMove;
    }

    private void OnDisable()
    {
        shootAction.action.Disable();
        shootAction.action.performed -= OnShoot;
        aimAction.action.Disable();

        moveAction.action.Disable();
        moveAction.action.performed -= OnMove;
        moveAction.action.canceled -= OnMove;
    }


    private void OnShoot(InputAction.CallbackContext context)
    {
        HandleShooting();
    }


    private void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    #endregion
}