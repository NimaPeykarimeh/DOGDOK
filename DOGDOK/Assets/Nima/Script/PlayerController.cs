using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
        public GameObject mainCamera;
        public CharacterMovement characterMovement;
        public PlayerMouseLook playerMouseLook;
        public Animator animator;

    [Header("States")]
        public bool isAiming;
        public bool isShooting;
        public bool isMoving;
        public bool isRunning;
        public bool isWalking;
    
    [Header("CameraObjects")]
        [SerializeField] CinemachineFreeLook combatCamera;
        [SerializeField] CinemachineFreeLook basicCamera;
        public PlayerStates currentState;
        [SerializeField] Vector2 lastMouseValue;



    [Header("Other")]
        public float layerWeightSpeed = 5f;
        [SerializeField] float aimDistance;
        public float maxRaycastDistance = 100f;

    public enum PlayerStates
    {
        Basic,
        Combat
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterMovement = GetComponent<CharacterMovement>();
        playerMouseLook = GetComponent<PlayerMouseLook>();

    }

    public RaycastHit GetAimHitInfo()
    {
        RaycastHit hitInfo;

        // Cast a ray from the camera's position and direction
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);

        // Check if the ray hits anything within the specified distance
        if (Physics.Raycast(ray, out hitInfo, maxRaycastDistance))
        {
            // Return the RaycastHit information
            return hitInfo;
        }
        else
        {
            // If the ray doesn't hit anything, return an empty RaycastHit
            return new RaycastHit();
        }
    }

    public void ChangePlayerState(PlayerStates _state)
    {
        if (_state == PlayerStates.Basic)
        {
            lastMouseValue = new Vector2(combatCamera.m_XAxis.Value,combatCamera.m_YAxis.Value);
            combatCamera.gameObject.SetActive(false);
            basicCamera.gameObject.SetActive(true);
            basicCamera.m_XAxis.Value = lastMouseValue.x;
            basicCamera.m_YAxis.Value = lastMouseValue.y;

        }
        if (_state == PlayerStates.Combat)
        {
            lastMouseValue = new Vector2(basicCamera.m_XAxis.Value, basicCamera.m_YAxis.Value);
            basicCamera.gameObject.SetActive(false);
            combatCamera.gameObject.SetActive(true);
            combatCamera.m_XAxis.Value = lastMouseValue.x;
            combatCamera.m_YAxis.Value = lastMouseValue.y;
        }

        currentState = _state;
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("MovementSpeed", characterMovement.currentSpeed);

        if (Input.GetMouseButtonDown(1))
        {
            ChangePlayerState(PlayerStates.Combat);
        }
        if (Input.GetMouseButtonUp(1))
        {
            ChangePlayerState(PlayerStates.Basic);
        }

        if (currentState == PlayerStates.Combat)
        {
            float newWeight = Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * layerWeightSpeed);
            animator.SetLayerWeight(1, newWeight);
            aimDistance = GetAimHitInfo().distance;
        }
        else
        {
            float newWeight = Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * layerWeightSpeed);
            animator.SetLayerWeight(1, newWeight);
        }
    }
}
