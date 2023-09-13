using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    [Header("Weapon Info")]
    public PlayerController playerController;
    public float generatingDuration = 0.5f;
    public float dissolvingDuration = 1f;
    public WeaponType weaponType;
    public Material weaponMaterial;
    public bool canShoot;
    public Weapon1 Weapon1;

    public enum WeaponType
    {
        OneHanded,
        TwoHanded,
        Melee
    }

    private void Awake()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        weaponMaterial = GetComponent<MeshRenderer>().material;
    }

}
