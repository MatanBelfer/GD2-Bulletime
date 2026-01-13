using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject Player; 
    
    #region Tags

    public const string PlayerTag = "Player";
    public const string EnemyTag = "Enemy";
    
    #endregion

    #region Sprites

    public Sprite weaponPickupSprite;
    public Sprite healthPickupSprite;
    public float pickupSizeMultiplier = 3f;
    
    #endregion
     

    // Probably better to use events but who cares
    private Shoot shoot;
    
    private Health playerHealth;

    #region Singleton Structure

    public static GameManager Instance;


    private void Awake()
    {
        Instance = this;
    }
    
    #endregion


    void Start()
    {
        shoot = Player.GetComponent<Shoot>();
        playerHealth = Player.GetComponent<Health>();
        
        foreach (Pickup p in FindObjectsByType<Pickup>(FindObjectsInactive.Include,FindObjectsSortMode.None)) {
            p.OnExitedArea += ExitedPickupArea;
            p.OnEnteredArea += EnteredPickupArea;
            p.OnPickup += PickUp;
        }

        foreach (HintArea a in FindObjectsByType<HintArea>(FindObjectsInactive.Include,FindObjectsSortMode.None)) {
            a.OnPlayerEnter += EnteredHintArea;
            a.OnPlayerExit += ExitedHintArea;
        }
    }

    
    #region Hint

    void EnteredHintArea(string text)
    {
        UiManager.Instance.SetHintText(text);
    }

    void ExitedHintArea()
    {
        UiManager.Instance.HideHintText();
    }
    
    #endregion
    

    #region Pickup
    void EnteredPickupArea(Pickup pickup)
    {
        UiManager.Instance.SetPickupText(pickup);
    }


    void ExitedPickupArea()
    {
        UiManager.Instance.HidePickupText();
    }


    void PickUp(Pickup.PickupType type)
    {
        switch (type)
        {
            case Pickup.PickupType.Weapon:
                shoot.doesHaveWeapon = true;
                break;
            case Pickup.PickupType.Health:
                playerHealth.Heal(5);
                break;
        }
        UiManager.Instance.HidePickupText();
    }
    #endregion
    
    
    public static float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        return toMin + (value - fromMin) * (toMax - toMin) / (fromMax - fromMin);
    }
    
}
