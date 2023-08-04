using UnityEngine;

[RequireComponent(typeof(NoiseMaker))]
public class CharacterMovement : MonoBehaviour
{
    PlayerController playerController;

    [Header("Movement")]
    float moveX;
    float moveZ;
    [SerializeField] Vector3 move;
    public float currentSpeed;
    public float currentMovementSpeed;
    [SerializeField] float walkingSpeed;
    [SerializeField] float runningSpeed;
    public float turnSpeed = 10f;

    [Header("Acceleration")]
    [SerializeField] float accelerationDuration;
    [SerializeField] float accelerationTimer;
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
            characterController.Move(move * currentMovementSpeed * Time.deltaTime);
            if (!playerController.isAiming && move.magnitude > 0)
            {
                RotateTowards(move);
            }
        }
        else
        {
            characterController.Move(move * currentMovementSpeed * Time.deltaTime);

            RotateTowards(playerController.mainCamera.transform.forward);
        }
        
    }
    //
    private void RotateTowards(Vector3 targetDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        targetRotation.x = 0;
        targetRotation.z = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    void ToggleRunState()
    {
        if (playerController.isRunning)
        {
            playerController.isWalking = true;
            playerController.isRunning = false;
            //movementSpeed = walkSpeed;
            noiseRange = walkingSpeed;
            
            isAccelerating = true;
            firstSpeed = runningSpeed;
            lastSpeed = walkingSpeed;
        }
        else
        {
            playerController.isWalking = false;
            playerController.isRunning = true;
            //movementSpeed = runSpeed;
            noiseRange = runningSpeed;

            isAccelerating = true;
            firstSpeed = walkingSpeed;
            lastSpeed = runningSpeed;
        }   
    }
    float GetMovement()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");

        Vector3 cameraForward = playerController.mainCamera.transform.forward;
        Vector3 cameraRight = playerController.mainCamera.transform.right;
        cameraRight.y = 0;
        cameraForward.y = 0;
        move = (cameraRight * moveX + cameraForward * moveZ);
        move.Normalize();
        return move.magnitude;

    }
    void Update()
    {
        currentSpeed = GetMovement() * currentMovementSpeed;
        playerController.animator.SetFloat("DirX",moveX);
        playerController.animator.SetFloat("DirY", moveZ);
        MovePlayer();
        if (currentSpeed > 0)
        {
            
            playerController.isMoving = true;
        }
        else
        {
            playerController.isMoving = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ToggleRunState();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            ToggleRunState();
        }

        if (isAccelerating)
        {
            accelerationTimer += Time.deltaTime;
            currentMovementSpeed = Mathf.Lerp(firstSpeed,lastSpeed,accelerationTimer/accelerationDuration);
            if (accelerationTimer > accelerationDuration)
            {
                isAccelerating = false;
                accelerationTimer = 0;
                currentMovementSpeed = lastSpeed;
            }
        }

    }


}
