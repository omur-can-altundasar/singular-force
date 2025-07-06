using System.Collections;
using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GunData _gunData;
    [SerializeField] private AudioSource _gunFireSound;
    [SerializeField] private Transform _muzzle;
    private float timeSinceLastShot;
    private LayerMask targetMask;
    [SerializeField] private TextMeshProUGUI _ammoHUD;


    private void Start()
    {
        PlayerShoot.shootInput += Shoot;
        PlayerShoot.reloadInput += StartReload;

        targetMask = LayerMask.GetMask("Shootable");
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
        if (_gunData.reserveAmmo > 0) {
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
                if (hitInfo.collider.CompareTag("TargetObject"))
                {
                    Destroy(hitInfo.collider.gameObject);
                }
            }

            _gunData.currentAmmo--;
            timeSinceLastShot = 0;
            _gunFireSound.Play();

            UpdateAmmoHUD();

            Debug.Log("Shot gun!");
        }
    }


    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }
}
