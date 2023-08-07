using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[RequireComponent(typeof(NoiseMaker))]
public class CharacterMovement : MonoBehaviour
{
    PlayerController playerController;

    [SerializeField] GameObject playerObject;

    [Header("Movement")]
    float moveX;
    float moveZ;
    Vector3 playerInput;
    [SerializeField] float inputMagnitude;
    [SerializeField] Vector3 move;
    MoveStates currentMoveState;
    [Header("Speeds")]
    public float currentVelocity;
    [SerializeField] float speedToMove;
    public float currentMovementSpeed;
    public float walkingSpeed;
    public float runningSpeed;
    public float turnSpeed = 10f;

    [Header("Acceleration")]
    [SerializeField] float accelerationDuration;
    [SerializeField] float accelerationTimer;
    [SerializeField] float accelerationSpeed;
    float firstSpeed;
    float lastSpeed;
    bool isAccelerating;

    [Space]

    public  CharacterController characterController;
    public Vector3 mousePosition;
    [Header("Noise")]
    NoiseMaker noiseMaker;
    [SerializeField] float noiseRange;
    [SerializeField] Transform noiseCenter;

    public enum MoveStates
    {
        Run,
        Walk
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
        noiseMaker = GetComponent<NoiseMaker>();
    }

    void MovePlayer()
    {
        if (playerController.currentState == PlayerController.PlayerStates.Basic)
        {
            characterController.Move(move * currentVelocity * Time.deltaTime);
            if (!playerController.isAiming && move.magnitude > 0)
            {
                RotateTowards(move);
            }
        }
        else
        {
            characterController.Move(move * currentVelocity * Time.deltaTime);

            RotateTowards(playerController.mainCamera.transform.forward);
        }
        
    }
    //
    private void RotateTowards(Vector3 targetDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        targetRotation.x = 0;
        targetRotation.z = 0;
        playerObject.transform.rotation = Quaternion.Slerp(playerObject.transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    public void ToggleRunState(MoveStates _state)
    {
        currentMoveState = _state;
        if (_state == MoveStates.Walk)
        {
            playerController.isWalking = true;
            playerController.isRunning = false;
            //movementSpeed = walkSpeed;
            noiseRange = walkingSpeed;
            //isAccelerating = true;

            currentMovementSpeed = walkingSpeed;
        }
        else if(_state == MoveStates.Run && playerController.currentState != PlayerController.PlayerStates.Combat)//
        {
            playerController.isWalking = false;
            playerController.isRunning = true;
            noiseRange = runningSpeed;

            //isAccelerating = true;

            currentMovementSpeed = runningSpeed;
        }   
    }

    void Accelarate()
    {
        isAccelerating = true;
    }

    void GetMovementDirection()
    {
        
        //get The forward and right direction of camera
        Vector3 cameraForward = playerController.mainCamera.transform.forward;
        Vector3 cameraRight = playerController.mainCamera.transform.right;

        //reset the one to prevent vertical rotation
        cameraRight.y = 0;
        cameraForward.y = 0;

        move = (cameraRight * moveX + cameraForward * moveZ);
        move.Normalize();
        //return move.magnitude;
    }

    void GetPlayerInput()
    {
        //get Player Input
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
        playerInput = new Vector3(moveX,0f,moveZ).normalized;
        inputMagnitude = playerInput.magnitude;
    }
    void Update()
    {
        //currentSpeed = GetMovement() * currentMovementSpeed;
        GetPlayerInput();
        if (inputMagnitude > 0)
        {
            playerController.animator.SetFloat("DirX",moveX, 0.1f, Time.deltaTime);
            playerController.animator.SetFloat("DirY", moveZ,0.1f, Time.deltaTime);
            //currentVelocity = inputMagnitude * currentMovementSpeed;
        }
        speedToMove = inputMagnitude * currentMovementSpeed;
        GetMovementDirection();
        MovePlayer();
        

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ToggleRunState(MoveStates.Run);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            ToggleRunState(MoveStates.Walk);
        }
        Accelarate();
        if (isAccelerating)
        {
            currentVelocity = Mathf.Lerp(currentVelocity, speedToMove, accelerationSpeed * Time.deltaTime);
            //accelerationTimer += Time.deltaTime;
            //currentMovementSpeed = Mathf.Lerp(firstSpeed,lastSpeed,accelerationTimer/accelerationDuration);
            //if (accelerationTimer > accelerationDuration)
            //{
            //    isAccelerating = false;
            //    accelerationTimer = 0;
            //    currentMovementSpeed = lastSpeed;
            //}
        }

    }


}
