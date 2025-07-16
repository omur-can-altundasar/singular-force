using UnityEngine;

public class BreakableBox : MonoBehaviour, IDamageable
{
    [SerializeField] private float _durability = 100f;

    private bool _isBroken;

    public void TakeDamage(float amount)
    {
        if (_isBroken) return;

        _durability -= amount;

        if (_durability <= 0)
        {
            Break();
        }
    }

    private void Break()
    {
        if (_isBroken) return;
        _isBroken = true;

        Destroy(gameObject);
    }
}
