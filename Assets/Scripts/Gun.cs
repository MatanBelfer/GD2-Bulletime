using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private InputActionReference shootAction;
    [SerializeField] private InputActionReference mousePositionAction;

    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        shootAction.action.Enable();
        mousePositionAction.action.Enable();
        
        shootAction.action.performed += OnShoot;
    }

    private void OnDisable()
    {
        shootAction.action.Disable();
        mousePositionAction.action.Disable();
        
        shootAction.action.performed -= OnShoot;
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        Vector2 screenMousePos = mousePositionAction.action.ReadValue<Vector2>();
        
        Vector3 worldMousePos = _mainCamera.ScreenToWorldPoint(new Vector3(screenMousePos.x, screenMousePos.y, _mainCamera.nearClipPlane));
        worldMousePos.z = 0;

        Vector2 shootDirection = ((Vector2)worldMousePos - (Vector2)firePoint.position).normalized;

        // Spawn and launch
        GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        
        if (bulletScript != null)
        {
            bulletScript.Launch(shootDirection, bulletSpeed);
        }
    }

}
