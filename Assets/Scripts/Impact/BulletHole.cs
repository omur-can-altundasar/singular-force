using UnityEngine;

public class BulletHole : MonoBehaviour
{
    [SerializeField] private float disableDelay = 5f;

    private void OnEnable()
    {
        CancelInvoke(nameof(Disable));
        Invoke(nameof(Disable), disableDelay);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}

