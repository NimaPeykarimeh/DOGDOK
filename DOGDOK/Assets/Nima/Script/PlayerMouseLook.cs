using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseLook : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] Vector3 _mousePosition;
    [SerializeField] float rotateSpeed;
    [SerializeField] LayerMask groundLayer;
    void Start()
    {
        
    }

    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit,Mathf.Infinity ,groundLayer))
        {
            // Get the point on the ground where the ray hits
            _mousePosition = hit.point;
            Vector3 directionToTarget = _mousePosition - transform.position;
            directionToTarget.y = 0; // Ignore the Y-axis for 2D rotation

            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed* Time.deltaTime);
        }
        //transform.LookAt(_mousePosition);
        //transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
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
