using TMPro;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    private Health _health;
    [SerializeField] private TextMeshProUGUI _healthText;

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
    }

    private void HandleDamageTaken(float currentHealth)
    {
        _healthText.text = currentHealth.ToString();
        Debug.Log("Damage taken! Your remaining HP: " + currentHealth);
    }

    private void HandleDie()
    {
        Debug.Log("You died");
    }
}
