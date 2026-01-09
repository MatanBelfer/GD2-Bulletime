using UnityEngine;

public class Health : MonoBehaviour, IDamageable, IHealable
{
    [SerializeField] private int startingHealth = 5;
    private float _currentHealth;

    void Awake()
    {
        _currentHealth = startingHealth;
    }

    public void OnHit(int damage)
    {
        Debug.Log($"{gameObject.name}: Hit for {damage} damage.");
    }

    public void Heal(int health)
    {
        Debug.Log($"{gameObject.name}: Healed for {health} health.");

    }
}
