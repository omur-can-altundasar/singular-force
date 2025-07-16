using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwitcher : MonoBehaviour
{
    [SerializeField] private Transform[] _weapons;
    private int _selectedWeaponIndex;
    private int _prevSelectedWeaponIndex;
    [SerializeField] private float _switchTime = 0.5f;
    private float _timeSinceLastSwitch;
    //[SerializeField] private KeyCode[] _keys;
    [SerializeField] private InputActionAsset inputActionsAsset;
    private InputAction[] _actions;

    private void Start()
    {
        SetActions();
        SetWeapons();
        SwitchToWeapon(_selectedWeaponIndex);

        _timeSinceLastSwitch = 0;
    }

    private void SetActions()
    {
        _actions = new[]
        {
            inputActionsAsset.FindAction("Previous"),
            inputActionsAsset.FindAction("Next")
        };

        foreach (var action in _actions)
        {
            action.Enable();
        }
    }

    private void SetWeapons()
    {
        _weapons = new Transform[transform.childCount];

        for (int i = 0; i < _weapons.Length; i++)
        {
            _weapons[i] = transform.GetChild(i);
        }
    }

    private void Update()
    {
        _prevSelectedWeaponIndex = _selectedWeaponIndex;

        for (int i = 0; i < _actions.Length; i++)
        {
            if (_actions[i].WasPressedThisFrame() && _timeSinceLastSwitch >= _switchTime)
            {
                _selectedWeaponIndex = i;
            }
        }

        if (_prevSelectedWeaponIndex != _selectedWeaponIndex)
        {
            SwitchToWeapon(_selectedWeaponIndex);
        }

        _timeSinceLastSwitch += Time.deltaTime;
    }

    private void SwitchToWeapon(int selectedWeaponIndex)
    {
        for (int i = 0; i < _weapons.Length; i++)
        {
            _weapons[i].gameObject.SetActive(i == selectedWeaponIndex);
        }

        _timeSinceLastSwitch = 0;

        OnWeaponSwitched();
    }

    private void OnWeaponSwitched()
    {
        PlayerShoot.isFiring = false;
        Debug.Log("A new weapon has been switched!");
    }
}
