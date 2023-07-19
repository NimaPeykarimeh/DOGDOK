using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float movementSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;

    public  CharacterController characterController;
    public Vector3 mousePosition;
    [SerializeField] GameObject mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = (mainCamera.transform.right * x + mainCamera.transform.forward * z);
        move = Vector3.ClampMagnitude(move, 1f);

        characterController.Move(move * movementSpeed * Time.deltaTime);


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            movementSpeed = runSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            movementSpeed = walkSpeed;
        }
    }


}
