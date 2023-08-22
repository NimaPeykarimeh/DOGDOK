using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
        public GameObject mainCamera;
        public AimPlayer aimPlayer;
        public CharacterMovement characterMovement;
        //public PlayerMouseLook playerMouseLook;
        public Animator animator;

    [Header("States")]
        public bool isAiming;
        public bool isShooting;
        public bool isMoving;
        public bool isRunning;
        public bool isWalking = true;
        public bool canRun = true;

    [Header("CameraObjects")]
        [SerializeField] GameObject combatCamera;
        [SerializeField] GameObject basicCamera;
        public PlayerStates currentState = PlayerStates.Basic;
        [SerializeField] Vector2 lastMouseValue;

    [Header("Animation Weights")]
    [SerializeField] Rig aimRig;
        public float aimLayerWeightSpeed = 3f;
        [SerializeField] int aimWeightLayerIndex = 1;
    private float currentWeight;
    private float newWeight;

    [Header("Other")]
    [SerializeField] Material weaponMat;
        [SerializeField] float aimDistance;
        
    public enum PlayerStates
    {
        Basic,
        Combat,
        Run
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterMovement = GetComponent<CharacterMovement>();
        aimPlayer = GetComponent<AimPlayer>();
        //playerMouseLook = GetComponent<PlayerMouseLook>();
    }


    public void ChangePlayerState(PlayerStates _state)
    {
        Debug.Log("State Changed");
        if (_state == PlayerStates.Basic)
        {
            //lastMouseValue = new Vector2(combatCamera.m_XAxis.Value,combatCamera.m_YAxis.Value);
            combatCamera.SetActive(false);
            basicCamera.SetActive(true);
            //basicCamera.m_XAxis.Value = lastMouseValue.x;
            //basicCamera.m_YAxis.Value = lastMouseValue.y;

        }
        if (_state == PlayerStates.Combat)
        {
            //lastMouseValue = new Vector2(basicCamera.m_XAxis.Value, basicCamera.m_YAxis.Value);
            basicCamera.SetActive(false);
            combatCamera.SetActive(true);
            //combatCamera.m_XAxis.Value = lastMouseValue.x;
            //combatCamera.m_YAxis.Value = lastMouseValue.y;
            characterMovement.ToggleRunState(CharacterMovement.MoveStates.Walk);
            //chande animaton weight
            currentWeight = animator.GetLayerWeight(aimWeightLayerIndex);
            newWeight = 1f;//Start Aim Animation
            canRun = false;
        }
        if (currentState == PlayerStates.Combat)
        {
            canRun = true;
            currentWeight = animator.GetLayerWeight(aimWeightLayerIndex);
            newWeight = 0f;//end Aim Animation
            if (characterMovement.isRunning)
            {
                
                characterMovement.ToggleRunState(CharacterMovement.MoveStates.Run);
            }
        }
        if (_state == PlayerStates.Run)
        {
            //characterMovement.currentMovementSpeed
        }
        currentState = _state;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (characterMovement.currentVelocity != characterMovement.speedToMove)//when player's changing speed
        {
            animator.SetFloat("MovementSpeed", characterMovement.currentVelocity / characterMovement.runningSpeed);
        }
        //animator.SetFloat("VelocityDir", characterMovement.speedToMove - characterMovement.currentVelocity);

        if (newWeight != currentWeight)
        {
            currentWeight = Mathf.MoveTowards(currentWeight,newWeight,aimLayerWeightSpeed * Time.deltaTime);
            aimRig.weight = currentWeight;
            animator.SetLayerWeight(aimWeightLayerIndex,currentWeight);
            weaponMat.SetFloat("_Dissolve",1-currentWeight);
        }
        else
        {
            Debug.Log("Layer Changed");
        }
    }
}
