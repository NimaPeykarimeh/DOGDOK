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

    private float currentAnimationValue = 1f; // silahýn bozulma/generate edilme durumunda hangi sayý deðerinde olduðu
    private const float unsolvedValue = -0.1f; // silahýn generate olmasý için gerekli olan deðer
    private const float dissolvedValue = 1f; // silahýn bozulmasý için gerekli olan deðer
    private float generatingDuration;
    private float dissolvingDuration;

    private int currentWeaponIndex = 0; //Silah seçme index'i

    private bool isGenerating; //Cismin generatingDuration sürecinin içinde olmasý durumunda true
    private bool isDissolving; //Cismin dissolvingDuration sürecinin içinde olmasý durumunda true

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


        if (isGenerating && AimPlayer.isAiming)  // niþan alýyorken ve silah yükleniyorken silahý generate et.
        {
            GenerateWeaponMaterial();
        }
        else if (!AimPlayer.isAiming) // niþan almýyorken silahý boz
        {
            CurrentWeaponController.canShoot = false;
            isGenerating = false;
            isDissolving = true;
            DissolveWeaponMaterial();
        }
        else if (!isGenerating) // && AimPlayer.isAiming -> Niþan alýyorken ama silah yüklenmiyorken, Eðer ki dissolve komutu verilmediyse 1. if koþuluna girmesi için isGenerating = TRUE yap (optimize deðil), yoksa silahý boz
        {
            if (!isDissolving)
            {
                isGenerating = true;
            }
            else DissolveWeaponMaterial();
        }
        WeaponSelectionInput(); // Eðer ki silah deðiþme inputu girildiyse, silahý bozmaya baþla ve yeni yüklenecek olan silah index'ini bozulma bitene kadar aklýnda tut, bozulma bitince þuanki silahý güncelle
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

        if(currentAnimationValue == 1f) // Silah tamamen görünmez olduysa silahý deðiþ.
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

    private void UpdateToNewWeapon()  // Yeni Silahýn tüm gerekli deðerlerini al.
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
    private void GenerateWeaponMaterial() // isGenerating == true olduðu durumda gelir. Silahý görünür hale getirir.
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
        if (currentAnimationValue == unsolvedValue) // Generating bitince bool'u deðiþtirir, ateþ etmeye izin verir.
        {
            isGenerating = false;
            CurrentWeaponController.canShoot = true;
        }
        else CurrentWeaponController.canShoot = false; // Yoksa ateþ etmeye izin vermez.
    }

    private void DissolveWeaponMaterial() // isDissolving == true olduðu durumda gelir. Silahý görünmez hale getirir.
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
        if (currentAnimationValue == dissolvedValue) // çözünme bitince bool'u deðiþtirir.
        {
            isDissolving = false;
        }
    }
}
