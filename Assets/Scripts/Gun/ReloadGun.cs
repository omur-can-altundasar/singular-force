using TMPro;
using UnityEngine;

public class ReloadGun : MonoBehaviour
{
    [SerializeField] private GunData _gunData;
    [SerializeField] private Animator _gunAnimator;
    [SerializeField] private TextMeshProUGUI _ammoHUD;

    private void OnEnable()
    {
        PlayerShoot.OnReloadInput += TryReload;
    }

    private void OnDisable()
    {
        PlayerShoot.OnReloadInput -= TryReload;

        _gunData.reloading = false;
    }

    private bool CanReload() => _gunData.reserveAmmo > 0 && _gunData.currentAmmo < _gunData.magazineSize && !_gunData.reloading;

    private void TryReload()
    {
        if (CanReload())
        {
            PlayReloadAnimation();
        }
    }

    private void PlayReloadAnimation()
    {
        _gunData.reloading = true;
        _gunAnimator.SetTrigger("Reload");
    }

    // Will subscribe to reload animation event
    private void OnReloadAnimationEnd()
    {
        ReloadAmmo();
        UpdateAmmoHUD();
        _gunData.reloading = false;
    }

    private void ReloadAmmo()
    {
        int needAmmo = _gunData.magazineSize - _gunData.currentAmmo;
        int ammoToLoad = Mathf.Min(needAmmo, _gunData.reserveAmmo);

        _gunData.currentAmmo += ammoToLoad;
        _gunData.reserveAmmo -= ammoToLoad;
    }

    private void UpdateAmmoHUD()
    {
        _ammoHUD.text = _gunData.currentAmmo.ToString() + " / " + _gunData.reserveAmmo;
    }
}
