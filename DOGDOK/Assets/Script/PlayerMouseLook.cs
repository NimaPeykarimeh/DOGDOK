using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseLook : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] Vector3 _mousePosition;
    void Start()
    {
        
    }

    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Get the point on the ground where the ray hits
            _mousePosition = hit.point;
            _mousePosition.y = 0;
        }
        transform.LookAt(_mousePosition);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
    }
}
