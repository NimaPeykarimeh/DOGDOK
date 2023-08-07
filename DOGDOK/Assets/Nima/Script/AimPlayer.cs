using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;
using static PlayerController;

public class AimPlayer : MonoBehaviour
{
    PlayerController playerController;
    [SerializeField] bool isAiming;

    [SerializeField] GameObject oriantation;
    [SerializeField] int verticalLimit;
    [SerializeField] float ySensitivity;
    [SerializeField] float xSensitivity;

    float rotationX = 0f;
    float rotationY = 0f;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
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
