using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> weapons = new();
    [SerializeField] private AimPlayer AimPlayer;

    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 3f;

    private float endAnimationValue = -0.1f;
    private int currentWeaponIndex = 0;
    private float currentAnimationValue = 1f;
    private bool isUpdating;
    private Material currentMaterial;

    void Start()
    {
        UpdateWeaponVisibility();
    }

    private void Update()
    {
        if (isUpdating && AimPlayer.isAiming) //Aim Alýyorken ve Silahýn materyali tam yüklenmemiþken
        {
            print("enter update");
            UpdateWeaponMaterial();
        }
        else if (!AimPlayer.isAiming) // Aim almýyorken silahý kayboldurtma
        {
            isUpdating = false;
            currentMaterial.SetFloat("_Dissolve", 1);
            currentAnimationValue = 1; // Aim almayý býraktýðýnda saydamlýkta default deðere dön.
        }
        else if (AimPlayer.isAiming) // Aim alýyorken silah deðiþtiðinde saydamlýkta deðiþiklik yapmadan devam ettir.
        {
            UpdateWeaponMaterial();
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0)
        {
            currentMaterial.SetFloat("_Dissolve", 1);
            SelectWeapon(currentWeaponIndex + 1);
        }
        else if (scroll < 0)
        {
            currentMaterial.SetFloat("_Dissolve", 1);
            SelectWeapon(currentWeaponIndex - 1);
        }
    }

    private void SelectWeapon(int nextWeaponIndex)
    {
        weapons[currentWeaponIndex].SetActive(false);

        currentWeaponIndex = (nextWeaponIndex + weapons.Count) % weapons.Count;

        UpdateWeaponVisibility();
    }

    private void UpdateWeaponVisibility()
    {
        weapons[currentWeaponIndex].SetActive(true);
        currentMaterial = weapons[currentWeaponIndex].GetComponent<Renderer>().material;
        currentMaterial.SetFloat("_Dissolve", 1);
        currentAnimationValue = 1;
        isUpdating = true;
    }
    private void UpdateWeaponMaterial()
    {
        currentAnimationValue = Mathf.MoveTowards(currentAnimationValue, endAnimationValue, (1 / animationDuration) * Time.deltaTime);
        currentMaterial.SetFloat("_Dissolve", currentAnimationValue);
        if (currentAnimationValue == endAnimationValue)
        {
            print("updated");
            isUpdating = false;
        }
    }

}
