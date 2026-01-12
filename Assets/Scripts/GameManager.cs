using UnityEngine;

public class GameManager : MonoBehaviour
{
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
    public Shoot shoot;
    public Health playerHealth;

    #region Singleton Structure

    public static GameManager Instance;


    private void Awake()
    {
        Instance = this;
    }
    
    #endregion


    void Start()
    {
        shoot = FindFirstObjectByType<Shoot>();
        
        foreach (Pickup p in FindObjectsByType<Pickup>(FindObjectsInactive.Include,FindObjectsSortMode.None)) {
            p.OnExitedArea += ExitedPickupArea;
            p.OnEnteredArea += EnteredPickupArea;
            p.OnPickup += PickUp;
        }
    }


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
                playerHealth.Heal(1);
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
