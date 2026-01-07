using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

    [SerializeField] private TextMeshProUGUI ammo;

    void Awake()
    {
        if(Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }

    public void UpdateAmmo(int currentAmmo, int magazineSize)
    {
        ammo.SetText($"{currentAmmo} / {magazineSize}");
    }
}
