using System.Collections;
using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GunData _gunData;
    private AudioSource _gunAudioSource;
    [SerializeField] private AudioClip _gunFireSound;
    [SerializeField] private Transform _muzzle;
    private float timeSinceLastShot;
    private LayerMask targetMask;
    [SerializeField] private TextMeshProUGUI _ammoHUD;


    [Header("Recoil Ayarları")]
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;
    [SerializeField] private float _minY;
    [SerializeField] private float _maxY;
    private Vector3 _currentRotation;
    [SerializeField] private float _recoilReturnSpeed = 3f;
    [SerializeField] private ParticleSystem _muzzleFireParticle;

    private void Awake()
    {
        _gunAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        targetMask = LayerMask.GetMask("Shootable");
    }

    private void OnEnable()
    {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;

        UpdateAmmoHUD();
    }

    private void OnDisable()
    {
        PlayerShoot.shootInput -= Shoot;
        PlayerShoot.reloadInput -= StartReload;

        _gunData.reloading = false;
    }

    public void StartReload()
    {
        if (!_gunData.reloading)
        {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload()
    {
        if (_gunData.reserveAmmo > 0)
        {
            _gunData.reloading = true;

            yield return new WaitForSeconds(_gunData.reloadTime);

            int needAmmo = _gunData.magazineSize - _gunData.currentAmmo;
            int ammoToLoad = Mathf.Min(needAmmo, _gunData.reserveAmmo);

            _gunData.currentAmmo += ammoToLoad;
            _gunData.reserveAmmo -= ammoToLoad;

            UpdateAmmoHUD();

            _gunData.reloading = false;
        }
    }

    private void UpdateAmmoHUD()
    {
        _ammoHUD.text = _gunData.currentAmmo.ToString() + " / " + _gunData.reserveAmmo;
    }

    private bool CanShoot() => _gunData.currentAmmo > 0 && !_gunData.reloading && timeSinceLastShot > 1f / (_gunData.fireRate / 60f);

    public void Shoot()
    {
        if (CanShoot())
        {
            Vector3 viewportPoint = new(0.5f, 0.5f, 0f);
            Ray ray = Camera.main.ViewportPointToRay(viewportPoint);

            bool isHit = Physics.Raycast(ray, out RaycastHit hitInfo, _gunData.maxDistance, targetMask);

            if (isHit)
            {
                Debug.DrawRay(ray.origin, ray.direction * _gunData.maxDistance, Color.green);

                if (hitInfo.collider.gameObject.TryGetComponent<IDamageable>(out var damageable)) {
                    damageable.TakeDamage(_gunData.damage);
                }
            }

            _gunData.currentAmmo--;
            timeSinceLastShot = 0;
            _gunAudioSource.pitch = Random.Range(0.95f, 1.05f);
            _gunAudioSource.PlayOneShot(_gunFireSound);
            _muzzleFireParticle.Play();

            UpdateAmmoHUD();
            Recoil();
            //Debug.Log("Shot gun!");
        }
    }

    private void Recoil()
    {
        float recoilRotationX = Random.Range(_minX, _maxX);
        float recoilRotationY = Random.Range(_minY, _maxY);
        Camera.main.transform.localRotation = Quaternion.Euler(_currentRotation.x - recoilRotationY, _currentRotation.y + recoilRotationX, _currentRotation.z);
        //transform.localRotation = Quaternion.Euler(_currentRotation.x - recoilRotationY, _currentRotation.y + recoilRotationX, _currentRotation.z);
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;


        _currentRotation = Camera.main.transform.localRotation.eulerAngles;

        if (_currentRotation.x != 0 || _currentRotation.y != 0)
        {
            Camera.main.transform.localRotation = Quaternion.Slerp(Camera.main.transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * _recoilReturnSpeed);
        }

    }
}
