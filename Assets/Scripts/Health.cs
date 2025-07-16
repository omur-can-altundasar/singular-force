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
        if (_isDead) return;    // Destroy objeyi hemen yok etmez dolayısıyla aynı kare içerisinde
                                // iki kez Die() tetiklendiğinde _isDead bayrağı tetiklenmeyi engeller

        _currentHealth -= amount;

        OnDamageTaken?.Invoke(_currentHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {                           
        if (_isDead) return;    // yanlışlıkla ölüm animasyonunun event'ına Kill() yerine
                                // yanlışlıkla Die() metodu bağlanırsa iki kere Die() tetiklenmesini 
                                // engellemek için _isDead bayrağı burada da kullanıldı
        _isDead = true;

        OnDie?.Invoke();

        Kill();
    }

    public void Kill()
    {
        gameObject.SetActive(false);
    }
}
