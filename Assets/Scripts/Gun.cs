using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GunData _gunData;
    private float timeSinceLastShot;
    private LayerMask targetMask;

    private void OnEnable()
    {
        PlayerShoot.OnShootInput += TryShoot;
        PlayerShoot.OnReloadInput += TryReload;

        PlayerShoot.OnShoot += Shoot;
    }

    private void OnDisable()
    {
        PlayerShoot.OnShootInput -= TryShoot;
        PlayerShoot.OnReloadInput -= TryReload;

        PlayerShoot.OnShoot -= Shoot;

        _gunData.reloading = false;
    }

    private void Start()
    {
        targetMask = LayerMask.GetMask("Shootable");
    }
    
    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }

    private bool CanShoot() => _gunData.currentAmmo > 0 && !_gunData.reloading && timeSinceLastShot > 1f / (_gunData.fireRate / 60f);
    private bool CanReload() => _gunData.reserveAmmo > 0 && _gunData.currentAmmo < _gunData.magazineSize && !_gunData.reloading;

    public void TryShoot()
    {
        if (CanShoot())
        {
            PlayerShoot.OnShoot?.Invoke();
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
    }

    private void TryReload()
    {
        if (CanReload())
        {
            PlayerShoot.OnReload?.Invoke();
        }
    }

    // Will subscribe to reload animation event
    private void ReloadAmmo()
    {
        int needAmmo = _gunData.magazineSize - _gunData.currentAmmo;
        int ammoToLoad = Mathf.Min(needAmmo, _gunData.reserveAmmo);

        _gunData.currentAmmo += ammoToLoad;
        _gunData.reserveAmmo -= ammoToLoad;
    }
}
