using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUIManager : MonoBehaviour
{
    public WeaponManager weaponManager;
    [SerializeField] private Image weaponImage;

    private void Awake()
    {
     Debug.Log(weaponImage.gameObject.name);   
    }

    private void Start()
    {

    }

    public void SetWeaponImage()
    {
        weaponImage.sprite = weaponManager.CurrentWeapon1.weaponImage;
    }
}
