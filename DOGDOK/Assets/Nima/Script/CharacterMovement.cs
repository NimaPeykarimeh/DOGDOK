using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(NoiseMaker))]
public class CharacterMovement : MonoBehaviour
{
    public float movementThreshold = 0.01f;
    [SerializeField] Animator animator;
    [Header("Movement")]
    public float turnSpeed = 10f;
    public float movementSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;

    public  CharacterController characterController;
    public Vector3 mousePosition;
    [SerializeField] GameObject mainCamera;
    NoiseMaker noiseMaker;
    [SerializeField] float noiseRange;
    [SerializeField] Transform noiseCenter;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        noiseMaker = GetComponent<NoiseMaker>();
    }

    void MovePlayer()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = (mainCamera.transform.right * x + mainCamera.transform.forward * z);
        move.Normalize();
        //move = Vector3.ClampMagnitude(move, 1f);

        characterController.Move(move * movementSpeed * Time.deltaTime);
        RotateTowards(move);
        animator.SetFloat("MovementSpeed",move.magnitude * movementSpeed);
    }
    private void RotateTowards(Vector3 targetDirection)
    {
        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }
    void Update()
    {
        MovePlayer();


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            movementSpeed = runSpeed;
            noiseRange = runSpeed;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            movementSpeed = walkSpeed;
            noiseRange = walkSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

            noiseMaker.MakeNoise(noiseRange, noiseCenter);
        }
    }


}
