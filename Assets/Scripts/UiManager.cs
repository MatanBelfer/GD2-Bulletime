using System;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

    [SerializeField] private GameObject[] healthBits;
    [SerializeField] private GameObject[] ammoBits;

    [SerializeField] private TextMeshProUGUI pickupText;
    [SerializeField] private RectTransform pickupRect;

    [SerializeField] private TextMeshProUGUI hintText;
    
    private Pickup _pickup;
    
    #region Singleton
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
    }
    #endregion


    private void Start()
    {
        HidePickupText();
        HideHintText();
    }


    private void Update()
    {
        if (!_pickup || pickupText.text == "")
            return;
        
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(_pickup.gameObject.transform.position);
        Vector2 rectPos = new Vector2(
            GameManager.Remap(screenPosition.x,0,Screen.width,0,1),
            GameManager.Remap(screenPosition.y,0,Screen.height,0,1)
        );
        pickupRect.anchorMin = rectPos;
        pickupRect.anchorMax = rectPos;
    }
    
    
    #region Hint


    public void SetHintText(string text)
    {
        hintText.text = text;
    }


    public void HideHintText()
    {
        hintText.text = "";
    }
    
    
    #endregion
    
    
    #region Pickup

    public void SetPickupText(Pickup pickup)
    {
        pickupText.text = $"Press [Interact] to pick up {pickup.type}";
        _pickup = pickup;
    }


    public void HidePickupText()
    {
        pickupText.text = "";
    }
    
    
    #endregion
    
    
    #region UI Updates
    public void UpdateAmmo(int currentAmmo, int magazineSize)
    {
        for (int i = 0; i < ammoBits.Length; i++)
        {
            if (ammoBits[i] != null)
            {
                    ammoBits[i].gameObject.SetActive(i < currentAmmo); 
            }
        }
    }

    public void UpdateHealth(int currentHealth)
    {
        for (int i = 0; i < healthBits.Length; i++)
        {
            if (healthBits[i] != null)
            {
                healthBits[i].gameObject.SetActive(i < currentHealth);
            }
        }   
    }
    #endregion
}