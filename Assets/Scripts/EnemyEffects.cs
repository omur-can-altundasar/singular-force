using UnityEngine;

public class EnemyEffects : MonoBehaviour
{
    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }
    private void OnEnable()
    {
        _health.OnDamageTaken += HandleDamageTaken;
        _health.OnDie += HandleDie;
    }

    private void OnDisable()
    {
        _health.OnDamageTaken -= HandleDamageTaken;
        _health.OnDie -= HandleDie;
    }

    private void HandleDamageTaken(float currentHealth) {
        Debug.Log("Damage taken! Remaining HP of the Enemy: " + currentHealth);
    }

    private void HandleDie() {
        Debug.Log("The enemy is dead.");
    }
}
