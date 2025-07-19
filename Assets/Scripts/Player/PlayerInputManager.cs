using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private PlayerMover _playerMover;
    [SerializeField] private PlayerLooker _playerLooker;
    [SerializeField] private WeaponSway _weaponSway;
    private Vector2 _inputMovement;
    private Vector2 _inputRotation;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        _playerMover.Move(_inputMovement);
        _weaponSway.Sway(_inputRotation);

        if (PlayerShoot.isShooting)
        {
            PlayerShoot.OnShootInput?.Invoke();
        }
    }

    public void OnPlayerMove(InputAction.CallbackContext callbackContext)
    {
        _inputMovement = callbackContext.ReadValue<Vector2>();
    }

    public void OnPlayerJump(InputAction.CallbackContext callbackContext)
    {
        _playerMover.Jump();
    }

    public void OnPlayerLook(InputAction.CallbackContext callbackContext)
    {
        _inputRotation = callbackContext.ReadValue<Vector2>();
        _playerLooker.Look(_inputRotation);
    }

    public void OnPlayerFire(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            PlayerShoot.isShooting = true;
        }
        else if (callbackContext.canceled)
        {
            PlayerShoot.isShooting = false;
        }
    }

    public void OnPlayerReload(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.started)
        {
            PlayerShoot.OnReloadInput?.Invoke();
        }
    }
}
