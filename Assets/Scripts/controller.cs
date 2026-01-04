using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class controller : MonoBehaviour
{
    
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private InputActionReference shootAction;
    
    [SerializeField] private float speed = 30f;
    [SerializeField] private InputActionReference MoveAction;
    private Rigidbody2D _rb;
    private Vector2 move;
    
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        HandleMovement();
        // HandleShooting();
    }
    

    private void HandleShooting()
    {
        // Spawn Bullet
        Bullet bullet = Instantiate(bulletPrefab,transform.position,Quaternion.identity).GetComponent<Bullet>();
        // Set Bullet Direction & Rotation
        // TODO : 
        // FIGURE OUT ERROR WITH MOUSE POSITION ( ALWAYS RETURNING 0,0 )
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, 0));
        bullet.direction = (mouseWorldPosition - transform.position).normalized;
        bullet.transform.rotation = Quaternion.LookRotation(bullet.direction);
        // Set Bullet Tags
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
        
        MoveAction.action.Enable();
        MoveAction.action.performed += OnMove;
        MoveAction.action.canceled += OnMove;
    }
    
    private void OnDisable()
    {
        shootAction.action.Disable();
        shootAction.action.performed -= OnShoot;
        
        MoveAction.action.Disable();
        MoveAction.action.performed -= OnMove;
        MoveAction.action.canceled -= OnMove;
    }


    private void OnShoot(InputAction.CallbackContext context)
    {
        // TODO :
        // FIX INPUT ERROR ( NOT READING MOUSE CLICK )
        if (context.performed) {
            HandleShooting();
        }
    }
    

    private void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }
    #endregion
}