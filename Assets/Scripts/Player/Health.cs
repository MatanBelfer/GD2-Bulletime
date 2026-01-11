using UnityEngine;


public class Health : MonoBehaviour, IDamageable, IHealable
{
    [SerializeField] private int startingHealth = 5;
    private float _currentHealth;

    void Awake()
    {
        _currentHealth = startingHealth;
        UpdateHealthBar();
    }

    public void OnHit(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
            Destroy(gameObject);
        UpdateHealthBar();


        Debug.Log($"{gameObject.name}: Hit for {damage} damage.");
    }

    public void Heal(int health)
    {
        Debug.Log($"{gameObject.name}: Healed for {health} health.");
        _currentHealth += health;
        UpdateHealthBar();
    }
    
    private void UpdateHealthBar()
    {
        if (gameObject.CompareTag("Player"))
        {
            UiManager.Instance.UpdateHealth((int)_currentHealth);
        }
    }
}