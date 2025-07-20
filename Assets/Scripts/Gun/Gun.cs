using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Fire Settings")]
    private float timeSinceLastShot;
    [Header("References")]
    [SerializeField] private GunData _gunData;
    private LayerMask targetMask;

    [Header("Effects References")]
    [SerializeField] private AudioSource _gunAudioSource;
    [SerializeField] private AudioClip _gunFireSound;
    [SerializeField] private ParticleSystem _muzzleFireParticle;
    [SerializeField] private Animator _gunAnimator;
    [SerializeField] private TextMeshProUGUI _ammoHUD;

    [Header("Recoil Settings")]
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;
    [SerializeField] private float _minY;
    [SerializeField] private float _maxY;
    private Vector3 _currentRotation;
    [SerializeField] private float _recoilReturnSpeed = 3f;

    private void OnEnable()
    {
        PlayerShoot.OnShootInput += TryShoot;

        UpdateAmmoHUD();
    }

    private void OnDisable()
    {
        PlayerShoot.OnShootInput -= TryShoot;
    }

    private void Start()
    {
        targetMask = LayerMask.GetMask("Shootable");
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;

        HandleRecoilReset();
    }

    private bool CanShoot() => _gunData.currentAmmo > 0 && !_gunData.reloading && timeSinceLastShot > 1f / (_gunData.fireRate / 60f);

    public void TryShoot()
    {
        if (CanShoot())
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        Vector3 viewportPoint = new(0.5f, 0.5f, 0f);
        Ray ray = Camera.main.ViewportPointToRay(viewportPoint);

        bool isHit = Physics.Raycast(ray, out RaycastHit hitInfo, _gunData.maxDistance, targetMask);

        if (isHit)
        {
            Debug.DrawRay(ray.origin, ray.direction * _gunData.maxDistance, Color.green);

            if (hitInfo.collider.gameObject.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(_gunData.damage);
            }
        }

        timeSinceLastShot = 0;
        _gunData.currentAmmo--;

        PlayFireSound();
        PlayFireParticle();
        SetRecoil();
        UpdateAmmoHUD();
    }

    private void PlayFireSound()
    {
        _gunAudioSource.pitch = Random.Range(0.95f, 1.05f);
        _gunAudioSource.PlayOneShot(_gunFireSound);
    }

    private void PlayFireParticle()
    {
        _muzzleFireParticle.Play();
    }

    private void UpdateAmmoHUD()
    {
        _ammoHUD.text = _gunData.currentAmmo.ToString() + " / " + _gunData.reserveAmmo;
    }

    private void HandleRecoilReset()
    {
        _currentRotation = Camera.main.transform.localRotation.eulerAngles;

        if (_currentRotation.x != 0 || _currentRotation.y != 0)
        {
            Camera.main.transform.localRotation = Quaternion.Slerp(Camera.main.transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * _recoilReturnSpeed);
        }
    }
    
    private void SetRecoil()
    {
        float recoilRotationX = Random.Range(_minX, _maxX);
        float recoilRotationY = Random.Range(_minY, _maxY);
        Camera.main.transform.localRotation = Quaternion.Euler(_currentRotation.x - recoilRotationY, _currentRotation.y + recoilRotationX, _currentRotation.z);
        //transform.localRotation = Quaternion.Euler(_currentRotation.x - recoilRotationY, _currentRotation.y + recoilRotationX, _currentRotation.z);
    }
}
