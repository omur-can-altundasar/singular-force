using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth = 100f;
    private float _currentHealth;
    public event Action<float> OnDamageTaken;
    public event Action OnDie;
    private bool _isDead;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (_isDead) return;    // Hasar alındığında TakeDamage() tetiklenir.
                                // Eğer OnDamageTaken event'ine abone bir metot içerisinde oynatılan animasyon klibinde
        _isDead = true;         // yanlışlıkla animation event'ına TakeDamage() metodu abone edilirse TakeDamage() bir kez daha tetiklenir
                                // Bu iki kez tetiklenmeyi _isDead bayrağı engeller.

        _currentHealth -= amount;

        OnDamageTaken?.Invoke(_currentHealth);

        if (_currentHealth <= 0)
        {
            OnDie?.Invoke();
        }
    }

    public void Kill()
    {
        gameObject.SetActive(false);
    }
}
