using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;
using static PlayerController;

public class AimPlayer : MonoBehaviour
{
    [Header("Refrences")]
    PlayerController playerController;

    [Header("Other")]
    [SerializeField] bool isAiming;
    [SerializeField] GameObject oriantation;
    [SerializeField] int verticalLimit;
    [SerializeField] float ySensitivity;
    [SerializeField] float xSensitivity;

    float rotationX = 0f;
    float rotationY = 0f;
    [Header("Raycast")]
    public float maxRaycastDistance = 100f;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        //mainCamera = playerController.mainCamera;
    }

    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * xSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * ySensitivity * Time.deltaTime;

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

        // Check if the ray hits anything within the specified distance
        if (Physics.Raycast(ray, out hitInfo, maxRaycastDistance))
        {
            // Return the RaycastHit 
            return hitInfo.point;
        }
        else
        {
            // If the ray doesn't hit anything, return an empty RaycastHit
            return ray.GetPoint(maxRaycastDistance);
        }
    }

    // Update is called once per frame
    void Update()
    {
        RotateCamera();

        if (Input.GetMouseButtonDown(1))
        {
            playerController.ChangePlayerState(PlayerStates.Combat);
        }
        if (Input.GetMouseButtonUp(1))
        {
            playerController.ChangePlayerState(PlayerStates.Basic);
        }
    }
}
