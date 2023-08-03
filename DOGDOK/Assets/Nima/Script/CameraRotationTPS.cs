using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotationTPS : MonoBehaviour
{
    public float rotationSpeedX = 3f;
    public float rotationSpeedY = 3f;
    public float minYAngle = -60f;
    public float maxYAngle = 60f;

    private float rotationY = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeedX;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeedY;

        rotationY -= mouseY;
        rotationY = Mathf.Clamp(rotationY, minYAngle, maxYAngle);
        transform.localEulerAngles = new Vector3(rotationY, transform.localEulerAngles.y + mouseX, 0f);
    }
}