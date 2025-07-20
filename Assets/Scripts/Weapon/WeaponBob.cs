using UnityEngine;

public class WeaponBob : MonoBehaviour
{
    [Header("Bobbing Settings")]
    public float amplitude = 0.01f;   // Salınım genişliği
    public float frequency = 10f;     // Salınım hızı

    [Header("Required")]
    private Vector3 initialPosition;

    [Header("Referances")]
    [SerializeField] private PlayerInputManager _playerInputManager;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        float horizontalInput = _playerInputManager._inputMovement.x;
        float verticalInput = _playerInputManager._inputMovement.y;
        bool isMoving = Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f;

        if (isMoving)
        {
            // Zamanla sinüs dalgası hesapla
            float bobbingOffsetX = Mathf.Sin(Time.time * frequency) * amplitude;
            float bobbingOffsetY = Mathf.Cos(Time.time * frequency * 2) * amplitude * 0.5f;

            transform.localPosition = initialPosition + new Vector3(bobbingOffsetX, bobbingOffsetY, 0);
        }
        else
        {
            // Hareket yoksa yumuşakça eski pozisyona dön
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, Time.deltaTime * 5f);
        }
    }
}

