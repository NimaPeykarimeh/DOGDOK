using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;
using static PlayerController;

public class AimPlayer : MonoBehaviour
{
    [Header("Refrences")]
    PlayerController playerController;

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
    [SerializeField] LayerMask aimAssistLayer;
    [SerializeField] float aimAssistSize = 1f;

    [Header("Aim")]
    [SerializeField] LayerMask aimLayer;
    [SerializeField] GameObject aimObject;
    public float rotationX = 0f;
    public float rotationY = 0f;
    [Header("Raycast")]
    public float maxRaycastDistance = 100f;
    [Header("WeaponAnimation")]
    [SerializeField] GameObject currentWeapon;
    [SerializeField] float animationDuration;
    float currentAnimationValue = 0f;
    [SerializeField] Material wepMaterial;
    
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        currentXSensitivity = defaultXSensitivity;
        currentYSensitivity = defalutYSensitivity;
        //mainCamera = playerController.mainCamera;
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

        isAimedOnEnemy = Physics.SphereCast(ray, aimAssistSize, out hitInfo, maxRaycastDistance, aimAssistLayer);
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
            playerController.ChangePlayerState(PlayerStates.Combat);
        }
        if (Input.GetMouseButtonUp(1))
        {
            playerController.ChangePlayerState(PlayerStates.Basic);
        }

        if (isAiming)
        {
            //currentAnimationValue = Mathf.MoveTowards(currentAnimationValue, -0.05f, (1 / animationDuration) * Time.deltaTime);
            //wepMaterial = currentWeapon.GetComponent<Renderer>().material;
            //wepMaterial.SetFloat("_Dissolve", currentAnimationValue);

        }
        else
        {
            //currentAnimationValue = Mathf.MoveTowards(currentAnimationValue, 1, (1 / animationDuration) * Time.deltaTime);
            //wepMaterial = currentWeapon.GetComponent<Renderer>().material;
            //wepMaterial.SetFloat("_Dissolve", currentAnimationValue);
        }
    }
}
