using TMPro;
using UnityEngine;

public class GunEffects : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GunData _gunData;

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

    private void Awake()
    {
        UpdateAmmoHUD();
    }

    private void OnEnable()
    {
        PlayerShoot.OnShoot += PlayFireSound;
        PlayerShoot.OnShoot += PlayFireParticle;
        PlayerShoot.OnShoot += Recoil;
        PlayerShoot.OnShoot += UpdateAmmoHUD;

        PlayerShoot.OnReload += PlayReloadAnimation;
    }

    private void OnDisable()
    {
        PlayerShoot.OnShoot -= PlayFireSound;
        PlayerShoot.OnShoot -= PlayFireParticle;
        PlayerShoot.OnShoot -= Recoil;
        PlayerShoot.OnShoot -= UpdateAmmoHUD;

        PlayerShoot.OnReload -= PlayReloadAnimation;
    }

    private void Update()
    {
        _currentRotation = Camera.main.transform.localRotation.eulerAngles;

        if (_currentRotation.x != 0 || _currentRotation.y != 0)
        {
            Camera.main.transform.localRotation = Quaternion.Slerp(Camera.main.transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * _recoilReturnSpeed);
        }
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

    private void PlayReloadAnimation()
    {
        _gunData.reloading = true;
        _gunAnimator.SetTrigger("Reload");
    }

    // Will subscribe to reload animation event
    private void OnReloadAnimationEnd()
    {
        _gunData.reloading = false;
        UpdateAmmoHUD();
    }

    private void Recoil()
    {
        float recoilRotationX = Random.Range(_minX, _maxX);
        float recoilRotationY = Random.Range(_minY, _maxY);
        Camera.main.transform.localRotation = Quaternion.Euler(_currentRotation.x - recoilRotationY, _currentRotation.y + recoilRotationX, _currentRotation.z);
        //transform.localRotation = Quaternion.Euler(_currentRotation.x - recoilRotationY, _currentRotation.y + recoilRotationX, _currentRotation.z);
    }

    private void UpdateAmmoHUD()
    {
        _ammoHUD.text = _gunData.currentAmmo.ToString() + " / " + _gunData.reserveAmmo;
    }
}
