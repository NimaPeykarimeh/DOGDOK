using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUIManager : MonoBehaviour
{
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private GameObject Image;

    private void Start()
    {

    }

    public void SetWeaponImage()
    {
        Image.GetComponent<Image>().sprite = weaponManager.CurrentWeapon1.weaponImage;
    }
}
