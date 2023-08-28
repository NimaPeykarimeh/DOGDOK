using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> Weapons = new();
    [SerializeField] private AimPlayer AimPlayer;
    [SerializeField] private float shootDelayAfterChanging = 1f;
    [SerializeField] private WeaponUIManager weaponUIManager;

    private GameObject CurrentWeaponObject;
    [HideInInspector] public Weapon1 CurrentWeapon1;
    [HideInInspector] public WeaponController CurrentWeaponController;
    private Renderer currentRenderer;

    private float currentAnimationValue = 1f;
    private const float unsolvedValue = -0.1f;
    private const float dissolvedValue = 1f;
    private float generatingDuration;
    private float dissolvingDuration;

    private int currentWeaponIndex = 0;

    private bool isGenerating; //Cismin generatingDuration sürecinin içinde olmasý durumunda true
    private bool isDissolving; //Cismin dissolvingDuration sürecinin içinde olmasý durumunda true

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Weapons.Add(transform.GetChild(i).gameObject);
        }
        UpdateWeaponVisibility();
    }
    void Start()
    {
        
    }

    private void Update()
    {
        if (isGenerating && AimPlayer.isAiming) 
        {
            GenerateWeaponMaterial();
        }
        else if (!AimPlayer.isAiming) 
        {
            CurrentWeaponController.canShoot = false;
            isGenerating = false;
            isDissolving = true;
            DissolveWeaponMaterial();
        }
        else if (!isGenerating) // && AimPlayer.isAiming
        {
            isGenerating = true;
        }
        WeaponSelectionInput();
    }

    private void WeaponSelectionInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0)
        {
            currentRenderer.material.SetFloat("_Dissolve", 1);
            SelectWeapon(currentWeaponIndex + 1);
        }
        else if (scroll < 0)
        {
            currentRenderer.material.SetFloat("_Dissolve", 1);
            SelectWeapon(currentWeaponIndex - 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && currentWeaponIndex != 0)
        {
            currentWeaponIndex = 0;
            SelectWeapon(currentWeaponIndex);
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2) && currentWeaponIndex != 1)
        {
            currentWeaponIndex = 1;
            SelectWeapon(currentWeaponIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && currentWeaponIndex != 2)
        {
            currentWeaponIndex = 2;
            SelectWeapon(currentWeaponIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && currentWeaponIndex != 3)
        {
            currentWeaponIndex = 3;
            SelectWeapon(currentWeaponIndex);
        }
    }

    private void SelectWeapon(int nextWeaponIndex)
    {
        CurrentWeaponObject.SetActive(false);

        currentWeaponIndex = (nextWeaponIndex + Weapons.Count) % Weapons.Count;

        UpdateWeaponVisibility();
    }

    private void UpdateWeaponVisibility()
    {
        CurrentWeaponObject = Weapons[currentWeaponIndex];
        CurrentWeaponObject.SetActive(true);

        CurrentWeaponController = CurrentWeaponObject.GetComponent<WeaponController>();
        CurrentWeapon1 = CurrentWeaponController.Weapon1;
        currentRenderer = CurrentWeaponObject.GetComponent<Renderer>();
        weaponUIManager.SetWeaponImage();

        generatingDuration = CurrentWeaponController.generatingDuration;
        dissolvingDuration = CurrentWeaponController.dissolvingDuration;

        if (AimPlayer.isAiming)
        {
            currentRenderer.material.SetFloat("_Dissolve", 1);
            currentAnimationValue = 1;
        }
        else currentRenderer.material.SetFloat("_Dissolve", currentAnimationValue);
        
        isGenerating = true;
    }
    private void GenerateWeaponMaterial()
    {
        currentAnimationValue = Mathf.MoveTowards(currentAnimationValue, unsolvedValue, (1 / generatingDuration) * Time.deltaTime);
        currentRenderer.material.SetFloat("_Dissolve", currentAnimationValue);
        if (currentAnimationValue == unsolvedValue)
        {
            isGenerating = false;
            CurrentWeaponController.canShoot = true;
        }
        else CurrentWeaponController.canShoot = false;
    }

    private void DissolveWeaponMaterial()
    {
        currentAnimationValue = Mathf.MoveTowards(currentAnimationValue, dissolvedValue, (1 / dissolvingDuration) * Time.deltaTime);
        currentRenderer.material.SetFloat("_Dissolve", currentAnimationValue);
        if (currentAnimationValue == dissolvedValue)
        {
            isDissolving = false;
        }
    }
}
