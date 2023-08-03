using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseLook : MonoBehaviour
{
    PlayerController playerController;
    [SerializeField] Camera mainCamera;
    [SerializeField] float rotateSpeed;
    [SerializeField] LayerMask groundLayer;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            playerController.isAiming = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            playerController.isAiming = false;
        }

        if (playerController.isAiming)
        {
            RotateToMouse();
        }
        //transform.LookAt(_mousePosition);
        //transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }
    void RotateToMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            // Get the point on the ground where the ray hits
            
            Vector3 directionToTarget = hit.point - transform.position;
            directionToTarget.Normalize();
            directionToTarget.y = 0; // Ignore the Y-axis for 2D rotation

            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }
    float  LookAt(Vector3 playerPosition, Vector3 mousePosition)
    {
        Vector3 directionToMouse = mousePosition - playerPosition;
        directionToMouse.y = 0; // Ignore the Y-axis to get the angle around Y only
        Quaternion lookRotation = Quaternion.LookRotation(directionToMouse);
        float angleToMouse = lookRotation.eulerAngles.y;

        return angleToMouse;
    }
}
