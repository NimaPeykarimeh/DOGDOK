using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMenuMovement : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float movementMagnitude;
    [SerializeField] Vector3 targetPosition;
    [SerializeField] bool isMoving = false;

    [SerializeField] float rotationSpeed;
    [SerializeField] float rotationMagnitute;
    [SerializeField] Quaternion targetRotation;
    [SerializeField] bool isRotating = false;
    void SetNewPosition()
    {
        float posX = Random.Range(-movementMagnitude,movementMagnitude);
        float posY = Random.Range(-movementMagnitude, movementMagnitude);
        float posZ = Random.Range(-movementMagnitude, movementMagnitude);
        targetPosition = new Vector3(posX,posY,posZ);
        isMoving = true;
    }

    void SetNewRotation()
    {
        float rotX = Random.Range(-rotationMagnitute, rotationMagnitute);
        float rotY = Random.Range(-rotationMagnitute, rotationMagnitute);
        float rotZ = Random.Range(-rotationMagnitute, rotationMagnitute);

        targetRotation = Quaternion.Euler(rotX,rotY,rotZ);
        isRotating = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            SetNewPosition();
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition,targetPosition,movementSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.localPosition, targetPosition) <= movementMagnitude/2)
            {
                isMoving = false;
            }
        }

        if (!isRotating)
        {
            SetNewRotation();
        }
        else
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation,targetRotation,rotationSpeed * Time.deltaTime);
            if (Quaternion.Angle(transform.localRotation,targetRotation) <= rotationMagnitute/2)
            {
                isRotating = false;
            }
        }
    }
}
