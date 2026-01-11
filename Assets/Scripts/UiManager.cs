using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

    [SerializeField] private TextMeshProUGUI ammo;

    [SerializeField] private GameObject[] healthBits;
    
    [SerializeField] private GameObject[] ammoBits;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
    }

    public void UpdateAmmo(int currentAmmo, int magazineSize)
    {
        for (int i = 0; i < ammoBits.Length; i++)
        {
            if (ammoBits[i] != null)
            {
                    ammoBits[i].gameObject.SetActive(i < currentAmmo); 
            }
        }
        
        ammo.SetText($"{currentAmmo} / {magazineSize}");
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
}