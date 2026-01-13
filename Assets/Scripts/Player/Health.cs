using System;
using UnityEngine;


public class Health : MonoBehaviour, IDamageable, IHealable
{
    [SerializeField] private int startingHealth = 1;
    [SerializeField] private int maxHealth = 5;
    private int _currentHealth;

    void Awake()
    {
        _currentHealth = startingHealth;
    }

    void Start()
    {
        UpdateHealthBar();
    }

    public void OnHit(int damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
            //GetComponent<Animator>().SetTrigger("onDeath");
            _currentHealth = 1;

        if (gameObject.CompareTag(GameManager.PlayerTag))
            UpdateHealthBar();


        Debug.Log($"{gameObject.name}: Hit for {damage} damage.");
    }

    public void Heal(int health)
    {
        Debug.Log($"{gameObject.name}: Healed for {health} health.");
        _currentHealth += health;
        _currentHealth = Math.Min(_currentHealth, maxHealth);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (gameObject.CompareTag(GameManager.PlayerTag))
        {
            UiManager.Instance.UpdateHealth(_currentHealth);
        }
    }
}