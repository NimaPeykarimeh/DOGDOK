using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    PlayerController playerController;
    [SerializeField] private List<GameObject> Weapons = new();
    [SerializeField] private AimPlayer AimPlayer;
    [SerializeField] private float shootDelayAfterChanging = 1f;
    [SerializeField] private WeaponUIManager weaponUIManager;

    private GameObject CurrentWeaponObject;
    [HideInInspector] public Weapon1 CurrentWeapon1;
    [HideInInspector] public WeaponController CurrentWeaponController;
    private Renderer currentRenderer;

    private float currentAnimationValue = 1f; // silah�n bozulma/generate edilme durumunda hangi say� de�erinde oldu�u
    private const float unsolvedValue = -0.1f; // silah�n generate olmas� i�in gerekli olan de�er
    private const float dissolvedValue = 1f; // silah�n bozulmas� i�in gerekli olan de�er
    private float generatingDuration;
    private float dissolvingDuration;

    private int currentWeaponIndex = 0; //Silah se�me index'i

    private bool isGenerating; //Cismin generatingDuration s�recinin i�inde olmas� durumunda true
    private bool isDissolving; //Cismin dissolvingDuration s�recinin i�inde olmas� durumunda true

    int rifleAimWeightLayerIndex = 1;//change Later
    int pistolAimWeightLayerIndex = 4;//change Later
    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Weapons.Add(transform.GetChild(i).gameObject);
        }
        UpdateToNewWeapon();
        playerController = FindObjectOfType<PlayerController>();
    }
    void Start()
    {
        
    }

    private void Update()
    {
        //swichWeapon hold
        //if (CurrentWeaponController.weaponType == WeaponController.WeaponType.Melee)
        //{
        //    AimPlayer.rigLayers[0].weight = Mathf.MoveTowards(rigLayers[0].weight, 1, (1 / animationDuration) * Time.deltaTime);
        //}

        //else if (CurrentWeaponController.weaponType == WeaponController.WeaponType.OneHanded)
        //{
        //    currentWeight = Mathf.MoveTowards(currentWeight, newWeight, (1 / animationDuration) * Time.deltaTime);
        //    playerController.animator.SetLayerWeight(pistolAimWeightLayerIndex, currentWeight);
        //    rigLayers[1].weight = Mathf.MoveTowards(rigLayers[1].weight, 1, (1 / animationDuration) * Time.deltaTime);
        //}

        //else if (CurrentWeaponController.weaponType == WeaponController.WeaponType.TwoHanded)
        //{
        //    currentWeight = Mathf.MoveTowards(currentWeight, newWeight, (1 / animationDuration) * Time.deltaTime);
        //    rifleAimWeightLayerIndex = 1;
        //    playerController.animator.SetLayerWeight(rifleAimWeightLayerIndex, currentWeight);
        //    rigLayers[2].weight = Mathf.MoveTowards(rigLayers[2].weight, 1, (1 / animationDuration) * Time.deltaTime);
        //}


        if (isGenerating && AimPlayer.isAiming)  // ni�an al�yorken ve silah y�kleniyorken silah� generate et.
        {
            GenerateWeaponMaterial();
        }
        else if (!AimPlayer.isAiming) // ni�an alm�yorken silah� boz
        {
            CurrentWeaponController.canShoot = false;
            isGenerating = false;
            isDissolving = true;
            DissolveWeaponMaterial();
        }
        else if (!isGenerating) // && AimPlayer.isAiming -> Ni�an al�yorken ama silah y�klenmiyorken, E�er ki dissolve komutu verilmediyse 1. if ko�uluna girmesi i�in isGenerating = TRUE yap (optimize de�il), yoksa silah� boz
        {
            if (!isDissolving)
            {
                isGenerating = true;
            }
            else DissolveWeaponMaterial();
        }
        WeaponSelectionInput(); // E�er ki silah de�i�me inputu girildiyse, silah� bozmaya ba�la ve yeni y�klenecek olan silah index'ini bozulma bitene kadar akl�nda tut, bozulma bitince �uanki silah� g�ncelle
    }

    private void WeaponSelectionInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0)
        {
            currentRenderer.material.SetFloat("_Dissolve", 1);
            currentWeaponIndex++;
            CurrentWeaponController.canShoot = false;
            isGenerating = false;
            isDissolving = true;
            DissolveWeaponMaterial();
            //SelectWeapon(currentWeaponIndex + 1);
        }
        else if (scroll < 0)
        {
            currentRenderer.material.SetFloat("_Dissolve", 1);
            currentWeaponIndex--;
            CurrentWeaponController.canShoot = false;
            isGenerating = false;
            isDissolving = true;
            DissolveWeaponMaterial();
            //electWeapon(currentWeaponIndex - 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && currentWeaponIndex != 0)
        {
            currentWeaponIndex = 0;
            CurrentWeaponController.canShoot = false;
            isGenerating = false;
            isDissolving = true;
            DissolveWeaponMaterial();
            //SelectWeapon(currentWeaponIndex);
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2) && currentWeaponIndex != 1)
        {
            currentWeaponIndex = 1;
            CurrentWeaponController.canShoot = false;
            isGenerating = false;
            isDissolving = true;
            DissolveWeaponMaterial();
            //SelectWeapon(currentWeaponIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && currentWeaponIndex != 2)
        {
            currentWeaponIndex = 2;
            CurrentWeaponController.canShoot = false;
            isGenerating = false;
            isDissolving = true;
            DissolveWeaponMaterial();
            //SelectWeapon(currentWeaponIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && currentWeaponIndex != 3)
        {
            currentWeaponIndex = 3;
            CurrentWeaponController.canShoot = false;
            isGenerating = false;
            isDissolving = true;
            DissolveWeaponMaterial();
            //SelectWeapon(currentWeaponIndex);
        }

        if(currentAnimationValue == 1f) // Silah tamamen g�r�nmez olduysa silah� de�i�.
        {
            SelectWeapon(currentWeaponIndex);
        }
    }

    private void SelectWeapon(int nextWeaponIndex)
    {
        CurrentWeaponObject.SetActive(false);

        currentWeaponIndex = (nextWeaponIndex + Weapons.Count) % Weapons.Count;

        UpdateToNewWeapon();
    }

    private void UpdateToNewWeapon()  // Yeni Silah�n t�m gerekli de�erlerini al.
    {
        CurrentWeaponObject = Weapons[currentWeaponIndex];
        CurrentWeaponObject.SetActive(true);

        CurrentWeaponController = CurrentWeaponObject.GetComponent<WeaponController>();
        CurrentWeapon1 = CurrentWeaponController.Weapon1;
        currentRenderer = CurrentWeaponObject.GetComponent<Renderer>();
        weaponUIManager.SetWeaponImage();

        generatingDuration = CurrentWeaponController.generatingDuration;
        dissolvingDuration = CurrentWeaponController.dissolvingDuration;

        currentRenderer.material.SetFloat("_Dissolve", currentAnimationValue);
        
        isGenerating = true;
    }
    private void GenerateWeaponMaterial() // isGenerating == true oldu�u durumda gelir. Silah� g�r�n�r hale getirir.
    {
        currentAnimationValue = Mathf.MoveTowards(currentAnimationValue, unsolvedValue, (1 / generatingDuration) * Time.deltaTime);
        float weaponHoldValue = Mathf.Clamp01(1 - currentAnimationValue * 2);
        //changeWeaponHold
        if (CurrentWeaponController.weaponType == WeaponController.WeaponType.Melee)
        {
            AimPlayer.rigLayers[0].weight = Mathf.Clamp01(weaponHoldValue);
        }
        else if (CurrentWeaponController.weaponType == WeaponController.WeaponType.OneHanded)
        {
            playerController.animator.SetLayerWeight(pistolAimWeightLayerIndex, weaponHoldValue);
            AimPlayer.rigLayers[1].weight = weaponHoldValue;
        }

        else if (CurrentWeaponController.weaponType == WeaponController.WeaponType.TwoHanded)
        {
            playerController.animator.SetLayerWeight(rifleAimWeightLayerIndex, weaponHoldValue);
            AimPlayer.rigLayers[2].weight = weaponHoldValue;
        }

        currentRenderer.material.SetFloat("_Dissolve", currentAnimationValue);
        if (currentAnimationValue == unsolvedValue) // Generating bitince bool'u de�i�tirir, ate� etmeye izin verir.
        {
            isGenerating = false;
            CurrentWeaponController.canShoot = true;
        }
        else CurrentWeaponController.canShoot = false; // Yoksa ate� etmeye izin vermez.
    }

    private void DissolveWeaponMaterial() // isDissolving == true oldu�u durumda gelir. Silah� g�r�nmez hale getirir.
    {
        currentAnimationValue = Mathf.MoveTowards(currentAnimationValue, dissolvedValue, (1 / dissolvingDuration) * Time.deltaTime);

        float weaponHoldValue = Mathf.Clamp01(1 - currentAnimationValue * 2);
        //changeWeaponHold
        if (CurrentWeaponController.weaponType == WeaponController.WeaponType.Melee)
        {
            AimPlayer.rigLayers[0].weight = Mathf.Clamp01(weaponHoldValue);
        }
        else if (CurrentWeaponController.weaponType == WeaponController.WeaponType.OneHanded)
        {
            playerController.animator.SetLayerWeight(pistolAimWeightLayerIndex, weaponHoldValue);
            AimPlayer.rigLayers[1].weight = weaponHoldValue;
        }

        else if (CurrentWeaponController.weaponType == WeaponController.WeaponType.TwoHanded)
        {
            playerController.animator.SetLayerWeight(rifleAimWeightLayerIndex, weaponHoldValue);
            AimPlayer.rigLayers[2].weight = weaponHoldValue;
        }

        currentRenderer.material.SetFloat("_Dissolve", currentAnimationValue);
        if (currentAnimationValue == dissolvedValue) // ��z�nme bitince bool'u de�i�tirir.
        {
            isDissolving = false;
        }
    }
}
