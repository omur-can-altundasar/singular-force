using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Weapon/Gun")]
public class GunData : ScriptableObject
{
    [Header("Info")]
    public new string name;

    [Header("Shooting")]
    public float damage;
    public float maxDistance;

    [Header("Reloading")]
    public int magazineSize;
    public int currentAmmo;
    public int reserveAmmo;
    public float fireRate;

    //[HideInInspector]
    public bool reloading;
}
