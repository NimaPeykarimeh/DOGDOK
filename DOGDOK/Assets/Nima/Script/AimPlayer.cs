using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Rendering;
using static PlayerController;

public class AimPlayer : MonoBehaviour
{
    [Header("Refrences")]
    PlayerController playerController;
    [SerializeField] WeaponManager weaponManager;

    [Header("Other")]
    public bool isAiming;
    [SerializeField] GameObject oriantation;
    [SerializeField] int verticalLimit;

    [Header("Sensitivity")]
    [SerializeField] float currentYSensitivity;
    [SerializeField] float currentXSensitivity;
    [Space(25)]
    [SerializeField] float defalutYSensitivity = 150;
    [SerializeField] float defaultXSensitivity = 150;
    [Space(25)]
    [SerializeField] float aimYSensitivity = 75;
    [SerializeField] float aimXSensitivity = 75;
    [Space(25)]
    [SerializeField] float assistSensitivity = 50;

    [Header("AimAssist")]
    [SerializeField] float aimAssistSpeed;
    [SerializeField] bool isAimedOnEnemy;
    [SerializeField] float aimAssistSize = 1f;

    [Header("Aim")]
    [SerializeField] LayerMask aimLayer;
    [SerializeField] GameObject aimObject;
    public float rotationX = 0f;
    public float rotationY = 0f;

    [Header("Raycast")]
    public float maxRaycastDistance = 100f;

    [Header("WeaponAnimation")]
    [SerializeField] float animationDuration;
    float currentAnimationValue = 0f;
    [SerializeField] Rig[] rigLayers;
    float currentWeight;
    float newWeight;
    int aimWeightLayerIndex = 1;//change Later
    [Header("AimDelay")]
    [SerializeField] bool waitForAim;
    [SerializeField] float aimDelayDuration = 0.2f;
    [SerializeField] float aimDelayTimer;
    [SerializeField] bool canAim;
    
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        currentXSensitivity = defaultXSensitivity;
        currentYSensitivity = defalutYSensitivity;
    }

    void RotateCamera()
    {

        float mouseX = Input.GetAxis("Mouse X") * currentXSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * currentYSensitivity * Time.deltaTime;

        // Apply the rotation
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -verticalLimit, verticalLimit);
        rotationY += mouseX;

        oriantation.transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }

    public Vector3 GetAimHitInfo()
    {
        RaycastHit hitInfo;

        // Cast a ray from the camera's position and direction
        Ray ray = new Ray(playerController.mainCamera.transform.position, playerController.mainCamera.transform.forward);
        aimObject.transform.position = ray.GetPoint(maxRaycastDistance);
        // Check if the ray hits anything within the specified distance
        if (Physics.Raycast(ray, out hitInfo, maxRaycastDistance, aimLayer))//sphereCast For Aim Assist
        {
            //isAimedOnEnemy = hitInfo.collider.CompareTag("EnemyBodyPart");
            // Return the RaycastHit 
            return hitInfo.point;
        }
        else
        {
            //isAimedOnEnemy = false;
            // If the ray doesn't hit anything, return an empty RaycastHit
            return ray.GetPoint(maxRaycastDistance);
        }
    }

    void CheckEnemyOnAim()
    {
        RaycastHit hitInfo;

        Ray ray = new Ray(playerController.mainCamera.transform.position, playerController.mainCamera.transform.forward);
        if (Physics.SphereCast(ray, aimAssistSize, out hitInfo, maxRaycastDistance, aimLayer))
        {
            isAimedOnEnemy = hitInfo.collider.CompareTag("EnemyBodyPart");
        }
        //if (Physics.SphereCast(ray, aimAssistSize, out hitInfo, maxRaycastDistance, aimAssistLayer))
        //{
        //oriantation.transform.forward = -(transform.position - hitInfo.point);
        //rotationX = oriantation.transform.localEulerAngles.y;
        //rotationY = oriantation.transform.localEulerAngles.y;
        //}
    }



    // Update is called once per frame
    void Update()
    {
        RotateCamera();
        GetAimHitInfo();
        CheckEnemyOnAim();

        if (playerController.currentState == PlayerStates.Combat)
        {
            if (isAimedOnEnemy)
            {
                currentXSensitivity = Mathf.MoveTowards(currentXSensitivity, assistSensitivity, aimAssistSpeed * Time.deltaTime);
                currentYSensitivity = Mathf.MoveTowards(currentYSensitivity, assistSensitivity, aimAssistSpeed * Time.deltaTime);
            }
            else
            {
                currentXSensitivity = Mathf.MoveTowards(currentXSensitivity, aimXSensitivity, aimAssistSpeed * Time.deltaTime);
                currentYSensitivity = Mathf.MoveTowards(currentYSensitivity, aimYSensitivity, aimAssistSpeed * Time.deltaTime);
            }
            //currentXSensitivity = aimSensitivity;
            //currentYSensitivity = aimSensitivity;
        }
        else
        {
            currentXSensitivity = Mathf.MoveTowards(currentXSensitivity, defaultXSensitivity, aimAssistSpeed * Time.deltaTime);
            currentYSensitivity = Mathf.MoveTowards(currentYSensitivity, defalutYSensitivity, aimAssistSpeed * Time.deltaTime);
            //currentXSensitivity = xSensitivity;
            //currentYSensitivity = ySensitivity;
        }

        if (Input.GetMouseButtonDown(1))
        {
            waitForAim = true;
            
        }
        else if (Input.GetMouseButtonUp(1) && isAiming)
        {
            playerController.ChangePlayerState(PlayerStates.Basic);
            canAim = false;
            waitForAim = false;
            aimDelayTimer = 0;
        }

        if (canAim && waitForAim)
        {
            playerController.ChangePlayerState(PlayerStates.Combat);
            waitForAim = false;
        }
        else if(!canAim)
        {
            aimDelayTimer += Time.deltaTime;
            if (aimDelayTimer >= aimDelayDuration)
            {
                canAim = true;
            }
        }


        if (isAiming)
        {
            
            newWeight = 1;
            if (weaponManager.CurrentWeaponController.weaponType == WeaponController.WeaponType.Melee)
            {
                rigLayers[0].weight = Mathf.MoveTowards(rigLayers[0].weight, 1, (1 / animationDuration) * Time.deltaTime);
            }
            else if (weaponManager.CurrentWeaponController.weaponType == WeaponController.WeaponType.OneHanded)
            {
                rigLayers[1].weight = Mathf.MoveTowards(rigLayers[1].weight, 1, (1 / animationDuration) * Time.deltaTime);
            }
            else if (weaponManager.CurrentWeaponController.weaponType == WeaponController.WeaponType.TwoHanded)
            {
                currentWeight = Mathf.MoveTowards(currentWeight, newWeight, (1 / animationDuration) * Time.deltaTime);
                aimWeightLayerIndex = 1;
                playerController.animator.SetLayerWeight(aimWeightLayerIndex, currentWeight);
                rigLayers[2].weight = Mathf.MoveTowards(rigLayers[2].weight, 1, (1 / animationDuration) * Time.deltaTime);
            }
            //currentAnimationValue = Mathf.MoveTowards(currentAnimationValue, -0.05f, (1 / animationDuration) * Time.deltaTime);
            //wepMaterial = currentWeapon.GetComponent<Renderer>().material;
            //wepMaterial.SetFloat("_Dissolve", currentAnimationValue);

        }
        else
        {
            newWeight = 0;
            if (weaponManager.CurrentWeaponController.weaponType == WeaponController.WeaponType.Melee)
            {
                rigLayers[0].weight = Mathf.MoveTowards(rigLayers[0].weight, 0, (1 / animationDuration) * Time.deltaTime);
            }
            else if (weaponManager.CurrentWeaponController.weaponType == WeaponController.WeaponType.OneHanded)
            {
                rigLayers[1].weight = Mathf.MoveTowards(rigLayers[1].weight, 0, (1 / animationDuration) * Time.deltaTime);
            }
            else if (weaponManager.CurrentWeaponController.weaponType == WeaponController.WeaponType.TwoHanded)
            {
                aimWeightLayerIndex = 1;
                currentWeight = Mathf.MoveTowards(currentWeight, newWeight, (1 / animationDuration) * Time.deltaTime);
                playerController.animator.SetLayerWeight(aimWeightLayerIndex, currentWeight);
                rigLayers[2].weight = Mathf.MoveTowards(rigLayers[2].weight, 0, (1 / animationDuration) * Time.deltaTime);
            }
            //currentAnimationValue = Mathf.MoveTowards(currentAnimationValue, 1, (1 / animationDuration) * Time.deltaTime);
            //wepMaterial = currentWeapon.GetComponent<Renderer>().material;
            //wepMaterial.SetFloat("_Dissolve", currentAnimationValue);
        }
    }
}
