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

    private float currentAnimationValue = 1f; // silahýn bozulma/generate edilme durumunda hangi sayý deðerinde olduðu
    private const float unsolvedValue = -0.1f; // silahýn generate olmasý için gerekli olan deðer
    private const float dissolvedValue = 1f; // silahýn bozulmasý için gerekli olan deðer
    private float generatingDuration;
    private float dissolvingDuration;

    private int currentWeaponIndex = 0; //Silah seçme index'i

    private bool isGenerating; //Cismin generatingDuration sürecinin içinde olmasý durumunda true
    private bool isDissolving; //Cismin dissolvingDuration sürecinin içinde olmasý durumunda true

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Weapons.Add(transform.GetChild(i).gameObject);
        }
        UpdateToNewWeapon();
    }
    void Start()
    {
        
    }

    private void Update()
    {
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
        currentRenderer.material.SetFloat("_Dissolve", currentAnimationValue);
        if (currentAnimationValue == dissolvedValue) // çözünme bitince bool'u deðiþtirir.
        {
            isDissolving = false;
        }
    }
}
