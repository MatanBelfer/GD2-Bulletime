using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pickup : MonoBehaviour
{
    [Header("Pickup")]
    public PickupType type;
    
    [Header("Instance")]
    [SerializeField] private InputActionReference interactAction;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    
    private bool _isPlayerInArea = false;
    public event Action<PickupType> OnPickup;
    public event Action<Pickup> OnEnteredArea;
    public event Action OnExitedArea;
    

    private void Start()
    {
        if (!spriteRenderer) {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        Sprite s = null;
        switch (type) {
            case PickupType.Health:
                s = GameManager.Instance.healthPickupSprite;
                break;
            case PickupType.Weapon:
                s = GameManager.Instance.weaponPickupSprite;
                break;
        }

        spriteRenderer.sprite = s;
        transform.localScale = new Vector3(1, 1, 1) * GameManager.Instance.pickupSizeMultiplier;
    }

    private void OnInteraction(InputAction.CallbackContext context)
    {
        if (_isPlayerInArea) {
            OnPickup?.Invoke(type);
            OnExitedArea?.Invoke();
            Destroy(this.gameObject);
        }
    }


    #region Trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GameManager.PlayerTag)) {
            _isPlayerInArea = true;
            Debug.Log("Player in area");
            OnEnteredArea?.Invoke(this);
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(GameManager.PlayerTag)) {
            _isPlayerInArea = false;
                        Debug.Log("Player outside area");
            OnExitedArea?.Invoke();
        }
    }
    #endregion
    
    
    #region Input
    private void OnEnable()
    {
    interactAction.action.Enable();
    interactAction.action.performed += OnInteraction;

    }

    private void OnDisable()
    {
        interactAction.action.performed -= OnInteraction;
    }
    #endregion


    public enum PickupType
    {
        Weapon,
        Health,
    }
}
