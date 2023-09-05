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
    public bool canRun= true;

    [Header("CameraObjects")]
    [SerializeField] GameObject combatCamera;
    [SerializeField] GameObject basicCamera;
    [SerializeField] Cinemachine3rdPersonFollow activeCinemachine;
    public PlayerStates currentState = PlayerStates.Basic;
    [SerializeField] Vector2 lastMouseValue;

    [Header("Animation Weights")]
    Rig[] rigLayers;
    public float aimLayerWeightSpeed = 3f;
    [SerializeField] int aimWeightLayerIndex = 1;
    private float currentWeight;
    private float newWeight;

    [Header("Other")]
    [SerializeField] Material weaponMat;
    [SerializeField] float aimDistance;

    [Header("dof Effect")]
    [SerializeField] Volume volume;
    [SerializeField] VolumeProfile volProf;
    public DepthOfField dofEffect;
    [SerializeField] bool isDepthing;
    [SerializeField] LayerMask focusLayers;
    [SerializeField] float dofEffectSpeed;
    [SerializeField] float currentFocusValue;
    [SerializeField] float focusDistanceToleranse;
    [SerializeField] float focuseDistanceRange = 3f;
    [SerializeField] float focusCastSphereRadius;
    [SerializeField] float maxFocusRange;
    [SerializeField] float farDof = 2f;
    [SerializeField] float nearDef = 2f;

    [Header("Switch Camera Side")]
    [SerializeField] float currentSideValue = 1;
    [SerializeField] float targetSideValue = 1;
    [SerializeField] float shoulderSwitchDuration = 0.3f;
    [SerializeField] bool isSwitching = false;

    public enum PlayerStates
    {
        Basic,
        Combat,
        Run
    }

    void Start()
    {
        volume.profile = volProf;
        volProf.TryGet<DepthOfField>(out dofEffect);

        Cursor.lockState = CursorLockMode.Locked;
        characterMovement = GetComponent<CharacterMovement>();
        aimPlayer = GetComponent<AimPlayer>();
        //playerMouseLook = GetComponent<PlayerMouseLook>();
    }

    float GetFocusDistance()
    {
        if (Physics.SphereCast(mainCamera.transform.position, focusCastSphereRadius, mainCamera.transform.forward, out RaycastHit hitInfo, maxFocusRange, focusLayers))
        {
            return hitInfo.distance;
        }

        return maxFocusRange;
    }

    public void ChangePlayerState(PlayerStates _state)
    {
        Debug.Log(aimPlayer.isAiming);
        if (_state == PlayerStates.Basic)
        {
            dofEffect.active = false;
            //lastMouseValue = new Vector2(combatCamera.m_XAxis.Value,combatCamera.m_YAxis.Value);
            combatCamera.SetActive(false);
            basicCamera.SetActive(true);
            activeCinemachine = basicCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            activeCinemachine.CameraSide = currentSideValue;
            //basicCamera.m_XAxis.Value = lastMouseValue.x;
            //basicCamera.m_YAxis.Value = lastMouseValue.y;

        }
        if (_state == PlayerStates.Combat)
        {
            dofEffect.active = true;
            //lastMouseValue = new Vector2(basicCamera.m_XAxis.Value, basicCamera.m_YAxis.Value);
            basicCamera.SetActive(false);
            combatCamera.SetActive(true);
            activeCinemachine = combatCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<Cinemachine3rdPersonFollow>();//Optimize Edilebilir
            activeCinemachine.CameraSide = currentSideValue;
            //combatCamera.m_XAxis.Value = lastMouseValue.x;
            //combatCamera.m_YAxis.Value = lastMouseValue.y;
            characterMovement.ToggleRunState(CharacterMovement.MoveStates.Walk);
            //chande animaton weight
            currentWeight = animator.GetLayerWeight(aimWeightLayerIndex);
            newWeight = 1f;//Start Aim Animation
            aimPlayer.isAiming = true;
            canRun = false;
        }
        if (currentState == PlayerStates.Combat)
        {
            canRun = true;
            aimPlayer.isAiming = false;
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

    void SwitchAimShoulder()
    {
        if (!isSwitching)
        {
            currentSideValue = activeCinemachine.CameraSide;
            if (targetSideValue == 1)
            {
                targetSideValue = 0;
            }
            else
            {
                targetSideValue = 1;
            }
            isSwitching = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchAimShoulder();
        }
        if (isSwitching)
        {
            currentSideValue = Mathf.MoveTowards(currentSideValue, targetSideValue,(1/shoulderSwitchDuration) * Time.deltaTime);
            activeCinemachine.CameraSide = currentSideValue;
            if (currentSideValue == targetSideValue)
            {
                isSwitching = false;
            }
        }

        if (currentState == PlayerStates.Combat)
        {
            isDepthing = Mathf.Abs(currentFocusValue - GetFocusDistance()) >= focusDistanceToleranse;
            if (isDepthing)
            {

                currentFocusValue = Mathf.Lerp(currentFocusValue, GetFocusDistance(), dofEffectSpeed * Time.deltaTime);
                dofEffect.farFocusStart.value = currentFocusValue + focuseDistanceRange;
                dofEffect.nearFocusEnd.value = currentFocusValue - focuseDistanceRange;
                //dofEffect.nearFocusStart.value = currentFocusValue / nearDef;
                dofEffect.farFocusEnd.value = currentFocusValue * farDof;
            }
            
        }

        if (characterMovement.currentVelocity != characterMovement.speedToMove)//when player's changing speed
        {
            animator.SetFloat("MovementSpeed", characterMovement.currentVelocity / characterMovement.runningSpeed);
        }
        //animator.SetFloat("VelocityDir", characterMovement.speedToMove - characterMovement.currentVelocity);

        if (newWeight != currentWeight)
        {
            //currentWeight = Mathf.MoveTowards(currentWeight,newWeight,aimLayerWeightSpeed * Time.deltaTime);
            //rigLayers[0].weight = currentWeight;
            //animator.SetLayerWeight(aimWeightLayerIndex,currentWeight);
            //weaponMat.SetFloat("_Dissolve",1 - currentWeight);
        }
    }
}
